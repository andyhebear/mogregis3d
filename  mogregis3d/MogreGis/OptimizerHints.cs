
namespace MogreGis
{
    /**
     * A collection of hints that tells the osgUtil::Optimizer to expressly include or 
     * exclude certain optimization options.
     *
     * Filters that do their own optimization may wish to instruct the general OSG
     * optimizer to include or exclude certain optimization techniques. The FilterEnv
     * carries an OptimizerHints object that can be used for this purpose.
     */
    public class OptimizerHints
    {

        /**
         * Constructs a empty hints object.
         */
        public OptimizerHints()
        {
            included = 0;
            excluded = 0;
        }

        /**
         * Copy constructor.
         */
        public OptimizerHints(OptimizerHints rhs)
        {
            included = rhs.included;
            excluded = rhs.excluded;
        }

#if TODO
        /**
         * Adds optimizer options that the general optimizer should use.
         */
       public  void include( osgUtil::Optimizer::OptimizationOptions options );

        /**
         * Adds optimizer options that the general optimizer should NOT use.
         */
       public  void exclude( osgUtil::Optimizer::OptimizationOptions options );

        /**
         * Gets the mask of options that the optimzer should expressly include.
         */
       public  osgUtil::Optimizer::OptimizationOptions getIncludedOptions() ;

        /**
         * Gets the mask of options that the optimizer should expressly exclude.
         */
       public  osgUtil::Optimizer::OptimizationOptions getExcludedOptions() ;

#endif
        private int included;
        private int excluded;
    }
}

