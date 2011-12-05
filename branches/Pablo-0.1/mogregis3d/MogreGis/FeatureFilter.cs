using System;
using System.Collections.Generic;

namespace MogreGis
{
    /**
     * Base class for a Filter that processes Feature instances.
     *
     * FeatureFilter implementations subclass this class and override one or both of
     * the process() methods.
     */
    public abstract class FeatureFilter : Filter
    {
        /**
         * Processes a single Feature.
         *
         * @param input
         *      Individual Feature to process.
         * @param env
         *      Runtime processing environment.
         * @return
         *      A collection of Feature instances. The default implementation
         *      of this method just returns the input in a list.
         */
        public virtual FeatureList process(Feature input, FilterEnv env)
        {
            FeatureList output = new FeatureList();
            output.Add(input);
            return output;
        }


        /**
         * Processes a batch of Feature objects.
         *
         * @param input
         *      Batch of features to process.
         * @param env
         *      Runtime processing environment.
         * @return
         *      A collection of Feature instances. The default implementation
         *      of this method just returns the input.
         */
        public virtual FeatureList process(FeatureList input, FilterEnv env)
        {
            FeatureList output = new FeatureList();
            foreach (Feature i in input)
            {
                FeatureList interim = process(i, env);
                output.InsertRange(output.Count, interim);
            }
            return output;
        }


        // Internal compilation methods

        /* (no api docs)
         * Generates a new state object for this filter.
         */
        public override FilterState newState()
        {
            return new FeatureFilterState((FeatureFilter)clone());
        }



        protected FeatureFilter()
        {
        }

    }
}
