using System;
#if TODO
using MogreGis;

namespace osgGISProjects
{

    public class CellCompiler : FeatureLayerCompiler
    {

        public enum OutputStatus
        {
            OUTPUT_UNKNOWN,
            OUTPUT_ALREADY_EXISTS,
            OUTPUT_NON_EMPTY,
            OUTPUT_EMPTY
        };


        public CellCompiler(
            string _cell_id,
             string _abs_output_uri,
            FeatureLayer layer,
            FilterGraph graph,
            float _min_range,
            float _max_range,
            FilterEnv env,
            ResourcePackager _packager,
            osgDB.Archive _archive, // =NULL,
            object user_data) : // =NULL );
            base(_abs_output_uri, layer, graph, env)
        {
            packager = _packager;
            cell_id = _cell_id;
            abs_output_uri = _abs_output_uri;
            min_range = _min_range;
            max_range = _max_range;
            archive = _archive;

            //TODO: maybe the FilterEnv should just have one of these by default.
            SmartReadCallback smart = new SmartReadCallback();
            smart.setMinRange(min_range);
            env.setTerrainReadCallback(smart);
            setUserData(_user_data);
            output_status = CellCompiler.OutputStatus.OUTPUT_UNKNOWN;
        }

        public virtual void run()
        {
            // first check to see whether this cell needs compiling:
            need_to_compile = archive.valid() || !osgDB.fileExists(abs_output_uri);

            output_status = CellCompiler.OutputStatus.OUTPUT_UNKNOWN;
            //has_drawables = false;

            if (need_to_compile)
            {
                // Compile the cell:
                FeatureLayerCompiler.run();

                // Write the resulting node graph to disk, first ensuring that the output folder exists:
                // TODO: consider whether this belongs in the runSynchronousPostProcess() method
                if (getResult().isOK())
                {
                    if (getResultNode() != null && GeomUtils.hasDrawables(getResultNode()))
                    {
                        output_status = CellCompiler.OutputStatus.OUTPUT_NON_EMPTY;
                    }
                    else
                    {
                        output_status = CellCompiler.OutputStatus.OUTPUT_EMPTY;
                    }
                }
            }
            else
            {
                result = FilterGraphResult.ok();
                output_status = CellCompiler.OutputStatus.OUTPUT_ALREADY_EXISTS;
                //has_drawables = true;
            }
        }

        public virtual void runSynchronousPostProcess(Report report)
        {
            if (need_to_compile)
            {
                if (!getResult().isOK())
                {
                    // osgGIS.notice() << getName() << " failed to compile: " << getResult().getMessage() << std.endl;
                    return;
                }

                if (output_status == CellCompiler.OutputStatus.OUTPUT_EMPTY) //!getResultNode() || !has_drawables )
                {
                    // osgGIS.info() << getName() << " resulted in no geometry" << std.endl;
                    result_node = null;
                    return;
                }

                if (packager.valid())
                {
                    // TODO: we should probably combine the following two calls into one:

                    // update any texture/model refs in preparation for packaging:
                    packager.rewriteResourceReferences(getResultNode());

                    // copy resources to their final destination
                    packager.packageResources(env.getResourceCache(), report);

                    // write the node data itself
                    osg.Node node_to_package = getResultNode();

                    if (!packager.packageNode(node_to_package.get(), abs_output_uri)) //, env.getCellExtent(), min_range, max_range ) )
                    {
                        //osgGIS.warn() << getName() << " failed to package node to output location" << std.endl;
                        result = FilterGraphResult.error("Cell built OK, but failed to deploy to disk/archive");
                    }
                }
            }
        }

        public string getCellId()
        {
            return cell_id;
        }

        public string getLocation()
        {
            return abs_output_uri;
        }

        public OutputStatus getOutputStatus()
        {
            return output_status;
        }


        private string cell_id;
        private string abs_output_uri;
        private osgDB.Archive archive;
        private ResourcePackager packager;
        private bool need_to_compile;
        //bool has_drawables;
        private float min_range, max_range;
        private OutputStatus output_status;
    }
}
#endif