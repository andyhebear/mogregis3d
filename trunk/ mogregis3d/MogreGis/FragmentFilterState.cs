using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    public class FragmentFilterState : FilterState
    {

        public FragmentFilterState(FragmentFilter _filter)
        {
            filter = _filter;
        }

        public override FilterStateResult traverse(FilterEnv in_env)
        {
            FilterStateResult result = new FilterStateResult();

            current_env = in_env.advance();

            FilterState next = getNextState();
            if (next != null)
            {
                FragmentList output =
                    in_features.Count > 0 ? filter.process(in_features, current_env) :
                    in_fragments.Count() > 0 ? filter.process(in_fragments, current_env) :
                    new FragmentList();

                if (output.Count > 0)
                {
                    if (next is NodeFilterState)
                    {
                        NodeFilterState state = (NodeFilterState)next;
                        state.push(output);
                    }
                    else if (next is FragmentFilterState)
                    {
                        FragmentFilterState state = (FragmentFilterState)next;
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

            in_features.Clear();
            in_fragments.Clear();

            return result;
        }

        public void push(Feature input)
        {
            if (input != null && input.hasShapeData())
                in_features.Add(input);
        }

        public void push(FeatureList input)
        {
            foreach (Feature i in input)
                if (i.hasShapeData())
                    in_features.Add(i);
        }

        public void push(Fragment input)
        {
            in_fragments.Add(input);
        }

        public void push(FragmentList input)
        {
            in_fragments.InsertRange(in_fragments.Count, input);
        }


        protected FragmentFilter filter;
        protected FeatureList in_features;
        protected FragmentList in_fragments;
    }
}
