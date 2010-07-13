using System;
using System.Collections.Generic;


namespace MogreGis
{
    /**
     * Holds the results of a filter graph compilation.
     */
    public class FilterGraphResult
    {

        /**
         * Creates a result object that conveys success.
         */
        public static FilterGraphResult ok(FilterEnv final_env)
        {
            return new FilterGraphResult(true, final_env);
        }


        /**
         * Creates a result object that conveys success.
         */
        public static FilterGraphResult ok()
        {
            return new FilterGraphResult(true, "");
        }


        /**
         * Creates a result object that convey an error.
         */
        public static FilterGraphResult error(string msg)
        {
            return new FilterGraphResult(false, msg);
        }


        /**
         * Default ctor - same as FilterGraphResult.error()
         */
        public FilterGraphResult()
        {
            is_ok = false;
        }

        /**
         * Copy constructor
         */
        public FilterGraphResult(FilterGraphResult rhs)
        {
            is_ok = rhs.is_ok;
            out_env = rhs.out_env;
        }

        /**
         * Gets whether the FilterGraph compilation succeeded.
         */
        public bool isOK()
        {
            return is_ok;
        }


        public SpatialReference getSRS()
        {
            return out_env != null ? out_env.getOutputSRS() : null;
        }


        public FilterEnv getOutputFilterEnv()
        {
            return out_env;
        }

        public string getMessage()
        {
            return msg;
        }

        private FilterGraphResult(bool _ok, string _msg)
        {
            is_ok = _ok;
            msg = _msg;
        }

        private FilterGraphResult(bool _ok, FilterEnv _env)
        {
            is_ok = _ok;
            out_env = _env;
        }


        private bool is_ok;
        private FilterEnv out_env;
        private string msg;
    }

    public class FilterGraphResultList : List<FilterGraphResult> { }


    /**
     * A sequential chain of filters.
     *
     * A FilterGraph is a sequence of discrete data-processing units called Filters.
     * The compiler will run a graph using a set of input features (typically
     * from a FeatureLayer), and a filter environment (FilterEnv) that contains shared
     * context information. The compiler passes data from filter to filter until it reaches
     * the end of the chain. It then results the results in a FilterGraphResult object.
     *
     * Although some basic type validation occurs, the responsibility lies with the caller
     * to build a working graph that passes compatible data down the chain.
     */
    public class FilterGraph
    {

        /**
         * Construct a new, empty filter graph.
         */
        public FilterGraph()
        {
            //NOP
        }


        /**
         * Gets the name of this graph.
         *
         * @return Name string
         */
        public string getName()
        {
            return name;
        }


        /**
         * Sets the name of this graph.
         * 
         * @param name Name of the graph
         */
        public void setName(string value)
        {
            name = value;
        }


        /**
         * Appends a filter to the end of the graph's filter chain.
         *
         * @param filter
         *      Filter to attach to the end of the chain. The filter's input
         *      requirements much be commpatible with the preceding filter's
         *      output specification.
         *
         * @return
         *      True if the filter appended succesfully.
         */
        public bool appendFilter(Filter filter)
        {
            filter_prototypes.Add(filter);
            return true;
        }


        /**
         * Inserts a filter into the graph's filter chain.
         *
         * @param filter
         *      Filter to insert.
         * @param at_index
         *      Index before which to insert the filter.
         *
         * @return
         *      True if the filter inserted succesfully.
         */
        public bool insertFilter(Filter filter, int index)
        {
            index = index < 0 ? 0 : index > filter_prototypes.Count ? filter_prototypes.Count : index;
            filter_prototypes.Insert(index, filter);
            return true;
        }
#if TODO

        /**
         * Runs the graph to generate a scene graph.
         *
         * Executes the graph by passing features to the first filter in the
         * chain. That filter will process the data, pass the results along to
         * the next filter, and so on until completion.
         *
         * @param cursor
         *      Source cursor for features to process
         * @param env
         *      Contextual compilation environment
         * @param output
         *      Group node that, upon success, contains resulting nodes of compiled scene
         *      as its children
         * @return
         *      A structure describing the result of the compilation.
         */
        public FilterGraphResult computeNodes(FeatureCursor cursor, FilterEnv env, osg.Group output)
        {
            FilterStateResult state_result;
            output = null;

            NodeFilterState output_state;

            // first build a new state chain corresponding to our filter prototype chain.
            FilterState first = null;
            foreach (Filter i in filter_prototypes)
            {
                FilterState next_state = i.newState();
                if (first == null)
                {
                    first = next_state;
                }
                else
                {
                    first.appendState(next_state);
                }

                if (next_state is NodeFilterState)
                {
                    output_state = (NodeFilterState)next_state;
                }
            }

            // now traverse the states.
            if (first != null)
            {
                int count = 0;
                osg.Timer_t start = osg.Timer.instance().tick();

                env.setOutputSRS(env.getInputSRS());

                if (first is FeatureFilterState)
                {
                    FeatureFilterState state = (FeatureFilterState)first;
                    while (state_result.isOK() && cursor.hasNext())
                    {
                        state.push(wind(cursor.next()));
                        state_result = state.traverse(env);
                        count++;
                    }
                    if (state_result.isOK())
                    {
                        state_result = state.signalCheckpoint();
                    }
                }
                else if (first is FragmentFilterState)
                {
                    FragmentFilterState state = (FragmentFilterState)first;
                    while (state_result.isOK() && cursor.hasNext())
                    {
                        state.push(wind(cursor.next()));
                        state_result = state.traverse(env);
                        count++;
                    }
                    if (state_result.isOK())
                    {
                        state_result = state.signalCheckpoint();
                    }
                }
                else if (first is CollectionFilterState)
                {
                    CollectionFilterState state = (CollectionFilterState)first;
                    while (state_result.isOK() && cursor.hasNext())
                    {
                        state.push(wind(cursor.next()));
                        state_result = state.traverse(env);
                        count++;
                    }
                    if (state_result.isOK())
                    {
                        state_result = state.signalCheckpoint();
                    }
                }

                osg.Timer_t end = osg.Timer.instance().tick();

                double dur = osg.Timer.instance().delta_s(start, end);
                //osgGIS.notify( osg.ALWAYS ) << std.endl << "Time = " << dur << " s; Per Feature Avg = " << (dur/(double)count) << " s" << std.endl;
            }
            else
            {
                state_result.set(FilterStateResult.Status.STATUS_NODATA);
            }

            if (output_state != null && state_result.hasData())
            {
                output = new osg.Group();
                foreach (AttributedNode i in output_state.getOutput())
                {
                    output.addChild(i.getNode());
                }
            }

            if (state_result.isOK())
            {
                return FilterGraphResult.ok(output_state.getLastKnownFilterEnv());
            }
            else
            {
                return FilterGraphResult.error("Filter graph failed to compute any nodes");
            }
        }


#endif
        /**
         * Runs the graph to generate a feature store. The graph should only
         * contain FeatureFilter and CollectionFilter type filters.
         *
         * Executes the graph by passing features to the first filter in the
         * chain. That filter will process the data, pass the results along to
         * the next filter, and so on until completion.
         *
         * @param cursor
         *      Source cursor for features to process
         * @param env
         *      Contextual compilation environment
         * @param output_uri
         *      URI of a feature store to create and in which to store the results
         * @return
         *      A structure describing the result of the compilation.
         */
        public FilterGraphResult computeFeatureStore(FeatureCursor cursor, FilterEnv env, string output_uri)
        {
#if TODO
            bool ok = false;

            // first build the filter state chain, validating that there are ONLY feature filters
            // present. No other filter type is permitted when generating a feature store.
            FilterState first = null;
            foreach (Filter i in filter_prototypes)
            {
                Filter filter = i;

                if (!(filter is FeatureFilter))
                {
                    //TODO osgGIS.notify(osg.WARN) << "Error: illegal filter of type \"" << filter.getFilterType() << "\" in graph. Only feature features are allowed." << std.endl;
                    return FilterGraphResult.error("Illegal first filter type in filter graph");
                }

                FilterState next_state = filter.newState();
                if (first == null)
                {
                    first = next_state;
                }
                else
                {
                    first.appendState(next_state);
                }
            }

            if (first == null)
            {
                //TODO osgGIS.notify(osg.WARN) << "Error: filter graph \"" << getName() << "\" is empty." << std.endl;
                return FilterGraphResult.error("Illegal: empty filter graph");
            }

            // next, append a WriteFeatures filter that will generate the output
            // feature store.
            WriteFeaturesFilter writer = new WriteFeaturesFilter();
            writer.setOutputURI(output_uri);
            //writer.setAppendMode( WriteFeaturesFilter.OVERWRITE );

            FilterState output_state = writer.newState();
            first.appendState(output_state);

            // now run the graph.
            FilterStateResult state_result;
            int count = 0;
            osg.Timer_t start = osg.Timer.instance().tick();

            env.setOutputSRS(env.getInputSRS());

            FeatureFilterState state = (FeatureFilterState)first;
            while (state_result.isOK() && cursor.hasNext())
            {
                state.push(cursor.next());
                state_result = state.traverse(env);
                count++;
            }

            if (state_result.isOK())
            {
                state_result = state.signalCheckpoint();
            }

            osg.Timer_t end = osg.Timer.instance().tick();
            double dur = osg.Timer.instance().delta_s(start, end);

            if (state_result.isOK())
            {
                return FilterGraphResult.ok(output_state.getLastKnownFilterEnv());
            }
            else
            {
                return FilterGraphResult.error("Filter graph failed to compute feature store");
            }
#endif
            throw new NotImplementedException();
        }


        /**
         * Finds a filter by its name. 
         * 
         * @param name
         *      Name of the filter to find
         * @return
         *      Filter found, or NULL if it was not found
         */
        public Filter getFilter(string name)
        {
            foreach (Filter i in filter_prototypes)
            {
                if (i.getName() == name)
                    return i;
            }
            return null;
        }


        /**
         * Gets the collection of filters in the graph.
         *
         * @return a sequential collection of filters
         */
        public FilterList getFilters()
        {
            return filter_prototypes;
        }
        // ensures that all single-part shapes have their verts wound CCW.
        static Feature wind(Feature input)
        {
#if TODO
            if (input.getShapeType() == GeoShape.ShapeType.TYPE_POLYGON)
            {
                foreach (GeoShape i in input.getShapes())
                {
                    GeoShape shape = i;
                    if (shape.getPartCount() == 1)
                    {
                        GeoPointList part = shape.getPart(0);
                        GeomUtils.openPolygon(part);
                        if (!GeomUtils.isPolygonCCW(part))
                            std.reverse(part.begin(), part.end());
                    }
                }
            }

            return input;
#endif
            throw new NotImplementedException();
        }



        private string name;
        private FilterList filter_prototypes = new FilterList();
    }

    public class FilterGraphList : List<FilterGraph> { }
}
