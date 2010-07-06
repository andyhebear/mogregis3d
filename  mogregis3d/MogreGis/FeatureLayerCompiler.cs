namespace MogreGis
{
    /**
     * Compiles a feature layer into a node graph.
     */
    public class FeatureLayerCompiler : Task
    {

        public FeatureLayerCompiler(
                                    string _name,
                                    FeatureLayer _layer,
                                    FilterGraph _graph,
                                    FilterEnv _env)
            : base(_name)
        {
            layer = _layer;
            filter_graph = _graph;
            env = _env;
        }


        public FeatureLayerCompiler(
                                    FeatureLayer _layer,
                                    FilterGraph _graph,
                                    FilterEnv _env)
        {
            layer = _layer;
            filter_graph = _graph;
            env = _env;
        }

        public FilterGraphResult getResult()
        {
            return result;
        }

        public osg.Node getResultNode()
        {
            return result_node;
        }

        public FilterEnv getFilterEnv()
        {
            return env;
        }

        public FeatureLayer getFeatureLayer()
        {
            return layer;
        }


        public virtual void run()
        {
            if (layer == null)
            {
                result = FilterGraphResult.error("Illegal: null feature layer");
            }
            else if (filter_graph == null)
            {
                result = FilterGraphResult.error("Illegal: null filter graph");
            }
            else if (env == null)
            {
                result = FilterGraphResult.error("Illegal: null filter environment");
            }
            else
            {
                env.getReport().markStartTime();

                // ensure the input SRS matches that of the layer:
                env.setInputSRS(layer.getSRS());

                // retrieve the features in the given extent:
                FeatureCursor cursor = layer.getCursor(env.getExtent());

                // and compile the filter graph:
                osg.Group temp = null;
                result = filter_graph.computeNodes(cursor, env, temp);
                result_node = temp;

                env.getReport().markEndTime();
            }
        }


        protected FeatureLayer layer;
        protected FilterGraph filter_graph;
        protected FilterEnv env;

        protected FilterGraphResult result;
        protected osg.Node result_node;
    }
}