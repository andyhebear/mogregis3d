using System;
using System.Collections.Generic;


namespace MogreGis
{
    /* (internal - no public api docs)
     *
     */
    public class NodeFilterState : FilterState
    {

        public NodeFilterState(NodeFilter _filter)
        {
            filter = _filter;
        }

        public override FilterStateResult traverse(FilterEnv in_env)
        {
            FilterStateResult result = new FilterStateResult();

            current_env = in_env.advance();

            if (in_features.Count > 0)
            {
                out_nodes = filter.process(in_features, current_env);
            }
            else if (in_fragments.Count > 0)
            {
                out_nodes = filter.process(in_fragments, current_env);
            }
            else if (in_nodes.Count > 0)
            {
                out_nodes = filter.process(in_nodes, current_env);
            }

            FilterState next = getNextState();
            if (next != null)
            {
                if (out_nodes.Count > 0)
                {
                    if (next is NodeFilterState)
                    {
                        NodeFilterState state = (NodeFilterState)next;
                        state.push(out_nodes);
                    }
                    else if (next is CollectionFilterState)
                    {
                        CollectionFilterState state = (CollectionFilterState)next;
                        state.push(out_nodes);
                    }

                    out_nodes.Clear();
                    result = next.traverse(current_env);
                }
                else
                {
                    result.set(FilterStateResult.Status.STATUS_NODATA, filter);
                }
            }

            in_features.Clear();
            in_fragments.Clear();
            in_nodes.Clear();

            return result;
        }

        /**
         * Pushes a feature onto this filter's input data queue.
         */
        public void push(Feature input)
        {
            if (input != null && input.hasShapeData())
                in_features.Add(input);
        }

        /**
         * Pushes a collection of features onto this filter's input data queue.
         */
        public void push(FeatureList input)
        {
            foreach (Feature i in input)
                if (i.hasShapeData())
                    in_features.Add(i);
        }

        /**
         * Pushes a Fragment onto this filter's input data queue.
         */
        public void push(Fragment input)
        {
            in_fragments.Add(input);
        }

        /**
         * Pushes a collection of Fragments onto this filter's input data queue.
         */
        public void push(FragmentList input)
        {
            in_fragments.InsertRange(in_fragments.Count, input);
        }

        /**
         * Pushes a single node onto this filter's input data queue.
         */
        public void push(AttributedNode input)
        {
            in_nodes.Add(input);
        }

        /**
         * Pushes a collection of nodes onto this filter's input data queue.
         */
        public void push(AttributedNodeList input)
        {
            in_nodes.InsertRange(in_nodes.Count, input);
        }


        /**
         * Gets the output node collection from this filter. Since a NodeFilter
         * can be the final node in a graphs's filter chain, this method allows
         * the compiler to retrieve the final results.
         */
        public AttributedNodeList getOutput()
        {
            return out_nodes;
        }


        protected NodeFilter filter;
        protected FeatureList in_features;
        protected FragmentList in_fragments;
        protected AttributedNodeList in_nodes;
        protected AttributedNodeList out_nodes;
    }
}
