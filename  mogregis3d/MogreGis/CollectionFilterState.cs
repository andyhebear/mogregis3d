using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /* (internal class - no public docs)
     * State object for filters derived from CollectionFilter.
     *
     * Please refer to FilterState for more information on filter state objects.
     */
    public class CollectionFilterState : FilterState
    {

        public CollectionFilterState(CollectionFilter _filter)
        {
            filter = _filter;
        }

        public override FilterStateResult traverse(FilterEnv env)
        {
            // just save a copy of the env for checkpoint time.
            current_env = env.advance();
            return new FilterStateResult();
        }
        public override FilterStateResult signalCheckpoint()
        {
#if TODO
            FilterStateResult result;

            FilterState next = getNextState();
            if (next != null)
            {
                int metering = filter.getMetering();

                if (next is FeatureFilterState)
                {
                    if (features.Count > 0)
                    {
                        FeatureGroups feature_groups;
                        foreach (Feature i in features)
                            feature_groups[filter.assign(i, current_env)].Add(i);

                        FeatureFilterState state = (FeatureFilterState)next;
                        result = meterGroups(filter, feature_groups, state, metering, current_env);
                    }
                    else
                    {
                        result.set(FilterStateResult.Status.STATUS_NODATA, filter);
                    }
                }
                else if (next is FragmentFilterState)
                {
                    FragmentFilterState state = (FragmentFilterState)next;
                    if (features.Count > 0)
                    {
                        FeatureGroups groups;
                        foreach (Feature i in features)
                            groups[filter.assign(i, current_env)].Add(i);
                        result = meterGroups(filter, groups, state, metering, current_env);
                    }
                    else if (fragments.Count > 0)
                    {
                        FragmentGroups groups;
                        foreach (Fragment i in fragments)
                            groups[filter.assign(i, current_env)].Add(i);
                        result = meterGroups(filter, groups, state, metering, current_env);
                    }
                    else
                    {
                        result.set(FilterStateResult.Status.STATUS_NODATA, filter);
                    }
                }
                else if (next is NodeFilterState)
                {
                    NodeFilterState state = (NodeFilterState)next;
                    if (features.Count > 0)
                    {
                        FeatureGroups feature_groups;
                        foreach (Feature i in features)
                            feature_groups[filter.assign(i, current_env)].Add(i);
                        result = meterGroups(filter, feature_groups, state, metering, current_env);
                    }
                    else if (fragments.Count > 0)
                    {
                        FragmentGroups groups;
                        foreach (Fragment i in fragments)
                            groups[filter.assign(i, current_env)].Add(i);
                        result = meterGroups(filter, groups, state, metering, current_env);
                    }
                    else if (nodes.Count > 0)
                    {
                        NodeGroups groups;
                        foreach (AttributedNode i in nodes)
                            groups[filter.assign(i, current_env)].Add(i);
                        result = meterGroups(filter, groups, state, metering, current_env);
                    }
                    else
                    {
                        result.set(FilterStateResult.Status.STATUS_NODATA, filter);
                    }
                }
                else if (next is CollectionFilterState)
                {
                    CollectionFilterState state = (CollectionFilterState)next;
                    if (features.Count > 0)
                    {
                        FeatureGroups feature_groups;
                        foreach (Feature i in features)
                            feature_groups[filter.assign(i, current_env)].Add(i);
                        result = meterGroups(filter, feature_groups, state, metering, current_env);
                    }
                    else if (fragments.Count > 0)
                    {
                        FragmentGroups groups;
                        foreach (Fragment i in fragments)
                            groups[filter.assign(i, current_env)].Add(i);
                        result = meterGroups(filter, groups, state, metering, current_env);
                    }
                    else if (nodes.Count > 0)
                    {
                        NodeGroups groups;
                        foreach (AttributedNode i in nodes)
                            groups[filter.assign(i, current_env)].Add(i);
                        result = meterGroups(filter, groups, state, metering, current_env);
                    }
                    else
                    {
                        result.set(FilterStateResult.Status.STATUS_NODATA, filter);
                    }
                }

                if (result.isOK())
                {
                    result = next.signalCheckpoint();
                }
            }

            // clean up the input:
            features.Clear();
            fragments.Clear();
            nodes.Clear();

            current_env = null;

            return result;
#endif
            throw new NotImplementedException();
        }

        public void push(Feature input)
        {
            if (input != null && input.hasShapeData())
                features.Add(input);
        }
        public void push(FeatureList input)
        {
            foreach (Feature i in input)
                if (i.hasShapeData())
                    features.Add(i);
        }
        public void push(Fragment input)
        {
            fragments.Add(input);
        }
        public void push(FragmentList input)
        {
            fragments.InsertRange(fragments.Count, input);
        }
        public void push(AttributedNode attr)
        {
            nodes.Add(attr);
        }
        public void push(AttributedNodeList nodes)
        {
            nodes.InsertRange(nodes.Count, nodes);
        }

#if TODO
        private static FilterStateResult meterData<A, B>(A source, B state, uint metering, FilterEnv env)
        {
            FilterStateResult result;

            if (metering == 0)
            {
                state.push(source);
                result = state.traverse(env);
            }
            else
            {
                uint batch_size = 0;
                foreach (A i in source)// TODO.begin(); i < source.end() && result.isOK(); i += batch_size )
                {
                    uint remaining = source.end() - i;
                    batch_size = Math.Min(remaining, metering);
                    A partial;
                    partial.insert(partial.end(), i, i + batch_size);
                    state.push(partial);
                    result = state.traverse(env);
                }
            }
            return result;
        }

        private static FilterStateResult meterGroups<A, B>(CollectionFilter filter, A groups, B state, int metering, FilterEnv env)
        {
            FilterStateResult result;

            string prop = filter.getAssignmentNameProperty();

            foreach (A i in groups) //TODO.begin(); i != groups.end() && result.isOK(); i++ )
            {
                if (prop.Length > 0)
                {
                    env.setProperty(new Property(prop, i.first));
                    //TODO  osgGIS::debug() << "[CollectionFilterState] Metering group '" << i.first << "', prop='" << prop << "'" << std::endl;
                }

                filter.preMeter(i.second, env);
                result = meterData(i.second, state, metering, env);
            }
            return result;
        }
#endif
        protected CollectionFilter filter;
        //osg::ref_ptr<FilterEnv> saved_env;

        protected FeatureList features;
        protected FragmentList fragments;
        protected AttributedNodeList nodes;
    }

    internal class FeatureGroups : Dictionary<string, FeatureList> { }
    internal class FragmentGroups : Dictionary<string, FragmentList> { }
    internal class NodeGroups : Dictionary<string, AttributedNodeList> { }

}
