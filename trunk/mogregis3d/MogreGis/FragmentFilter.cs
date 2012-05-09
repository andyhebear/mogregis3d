using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /**
     * A filter that generates fragments from either features or other fragments.
     *
     * A FragmentFilter can accept either features or other fragments as input (i.e.,
     * you can append a FragmentFilter, a FeatureFilter, or a CollectionFilter.)
     *
     * The subclass implementation should override either one/both of the
     * feature-based process() methods, or one/both of the fragment-based
     * process() methods(). The compiler will call the appropriate method based
     * on the preceding filter in the graph.
     */
    public class FragmentFilter : Filter
    {
        public override string getFilterType() { return getStaticFilterType(); }
        public override Filter clone() { return new FragmentFilter(this); }
        public static string getStaticFilterType() { return "FragmentFilter"; }
        public static FilterFactory getFilterFactory() { return new FilterFactoryImpl<FragmentFilter>(); }     

        /**
         * Processes a single feature into fragment(s). Override this method in
         * your implementation to convert individual features into fragments.         
         *
         * @param input
         *      Input feature
         * @param env
         *      Contextual compilation information
         * @return
         *      The converted input data. The default implementation of this
         *      method returns an empty set.
         */
        public virtual FragmentList process(Feature input, FilterEnv env)
        {
            return new FragmentList();
        }


        /**
         * Processes a collection of features into fragments. Override this
         * method in your implementation to convert batches of features into
         * fragments.
         *
         * @param input
         *      Batch of features to convert into drawables
         * @param env
         *      Contextual compilation information
         * @return
         *      The converted input data. The default implementation of this
         *      method returns an empty set.
         */
        public virtual FragmentList process(FeatureList input, FilterEnv env)
        {
            FragmentList output = new FragmentList();
            foreach (Feature i in input)
            {
                FragmentList interim = process(i, env);
                output.InsertRange(output.Count, interim);
            }
            return output;
        }


        /**
         * Processes a single fragment into fragment(s). Override this method
         * in your implementation to process individual drawables into more
         * fragments.
         *
         * @param input
         *      Single fragment to convert
         * @param env
         *      Contextual compilation information
         * @return
         *      The converted input data. The default implementation of this
         *      method returns the input data.
         */
        public virtual FragmentList process(Fragment drawable, FilterEnv env)
        {
            FragmentList output = new FragmentList();
            output.Add(drawable);
            return output;
        }


        /**
         * Processes a collection of fragments into fragments. Override this
         * method in your implementation to process batches of fragments.
         *
         * @param input
         *      Batch of fragments to process
         * @param env
         *      Contextual compilation information
         * @return
         *      The converted input data. The default implementation of this
         *      method returns the input data.
         */
        public virtual FragmentList process(FragmentList input, FilterEnv env)
        {
            FragmentList output = new FragmentList();
            foreach (Fragment i in input)
            {
                FragmentList interim = process(i, env);
                output.InsertRange(output.Count, interim);
            }
            return output;
        }

        // Filter overrides (internal)

        public override FilterState newState()
        {
            return new FragmentFilterState((FragmentFilter)clone());
        }



        public FragmentFilter()
        {
            //NOP
        }
        protected FragmentFilter(FragmentFilter rhs)
            : base(rhs)
        {
            //NOP
        }

    }
}
