using System;
using System.Collections.Generic;

namespace MogreGis
{
    /* (internal class - no public api docs)
     *
     * State object for FeatureFilter implementations. See FilterState for more
     * information on FilterGraph state objects.
     */
    public class FeatureFilterState : FilterState
    {
        /**
         * Constructs a new state object for the specified Filter.
         *
         * @param filter
         *      FeatureFilter for which to track state
         */
        public FeatureFilterState(FeatureFilter _filter)
        {
            filter = _filter;
        }

        /**
         * Pushes a Feature onto the input list.
         */
        public void push(Feature input)
        {
            if (input != null && input.hasShapeData())
                in_features.Add(input);
        }

        /**
         * Pushes a list of Features onto the input list.
         */
        public void push(FeatureList input)
        {
            foreach (Feature i in input)
                if (i.hasShapeData())
                    in_features.Add(i);
        }

        /**
         * Instructs this state's Filter to process its input.
         *
         * @param env
         *      Runtime processing environment
         */
        public override FilterStateResult traverse(FilterEnv in_env)
        {
            FilterStateResult result = new FilterStateResult();

            // clone a new environment:
            current_env = in_env.advance();

            FeatureList output = filter.process(in_features, current_env);

            FilterState next = getNextState();
            if (next != null)
            {
                if (output.Count > 0)
                {
                    if (next is FeatureFilterState)
                    {
                        FeatureFilterState state = (FeatureFilterState)next;
                        state.push(output);
                    }
                    else if (next is FragmentFilterState)
                    {
                        FragmentFilterState state = (FragmentFilterState)next;
                        state.push(output);
                    }
                    else if (next is NodeFilterState)
                    {
                        NodeFilterState state = (NodeFilterState)next;
                        state.push(output);
                    }
                    else if (next is CollectionFilterState)
                    {
                        CollectionFilterState state = (CollectionFilterState)next;
                        state.push(output);
                    }

                    result = next.traverse(current_env);
                }
                else
                {
                    result.set(FilterStateResult.Status.STATUS_NODATA, filter);
                }
            }

            // clean up
            in_features.Clear();

            return result;
        }


        protected FeatureList in_features;
        protected FeatureFilter filter;
    }
}
