using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if TODO
using Mogre;
using MogreGis;
#endif
namespace MogreGisSimple
{
    class Simple
    {
        static void Main(string[] args)
        {
            parseCommandLine(args);

            // The osgGIS registry is the starting point for loading new featue layers
            // and creating spatial reference systems.
            Registry registry = Registry.instance();

            // Load up the feature layer that we want to clamp to a terrain:
            //TODO NOUT << "Loading feature layer and building spatial index..." << ENDL;
            FeatureLayer layer = registry.createFeatureLayer(input_file);
            if (layer == null)
            {
                die("Failed to create feature layer.");
                return;
            }

            // Create a graph that the compiler will use to build the geometry:
            FilterGraph graph = createFilterGraph();

            SimpleLayerCompiler compiler;
            FeatureCursor cursor = layer.getCursor();
            Node output = compiler.compile(layer, cursor, graph.get());

            if (output == null)
            {
                die("Compilation failed!");
                return;
            }

            if (fade_lods)
            {
                float radius = output.getBound().radius();
                FadeHelper.enableFading(output.getOrCreateStateSet());
                FadeHelper.setOuterFadeDistance(radius * 3.0f, output.getOrCreateStateSet());
                FadeHelper.setInnerFadeDistance(radius * 1.0f, output.getOrCreateStateSet());
            }

            // Launch a viewer to see the results on the reference terrain.
            if (!string.IsNullOrEmpty(output_file))
            {
                osgDB.makeDirectoryForFile(output_file);
                osgDB.writeNodeFile(output, output_file);
            }
            else
            {
                osgViewer.Viewer viewer;
                viewer.setUpViewInWindow(10, 10, 800, 600);
                viewer.setSceneData(output.get());
                viewer.run();
            }
        }

        static int die(string msg)
        {
            //TODO osgGIS::notify( osg::FATAL ) << "ERROR: " << msg << ENDL;
            return -1;
        }

        static void usage(string prog, string msg)
        {
#if TODO
    if ( msg )
        NOUT << msg << std::endl;
    NOUT << prog << " loads a vector file (e.g., a shapefile) and generates OSG geometry." << ENDL;
    NOUT << ENDL;
    NOUT << "Usage:" << ENDL;
    NOUT << "    " << prog << " --input vector_file --output output_file [options...]" << ENDL;
    NOUT << ENDL;
    NOUT << "Required:" << ENDL;
    NOUT << "    --input <filename>     - Vector data to compile" << ENDL;
    NOUT << ENDL;
    NOUT << "Optional:"<< ENDL;
    NOUT << "    --output <filename>    - Output geometry file (omit this to launch a viewer)" << ENDL;
    NOUT << "    --color <r,g,b,a>      - Color of output geometry (0.1)" << ENDL;
    NOUT << "    --random-colors        - Randomly assign feature colors" << ENDL;
    NOUT << "    --fade-lods            - Apply LOD fading" << ENDL;
    //NOUT << "    --buffer <n>           - Apply shape buffering" << ENDL;
#endif
        }

        static void parseCommandLine(string[] args)
        {
#if TODO
            Registry registry = Registry.instance();

            osg.ArgumentParser arguments = new osg.ArgumentParser(args);

            arguments.getApplicationUsage().setApplicationName(arguments.getApplicationName());
            arguments.getApplicationUsage().setDescription(arguments.getApplicationName() + " clamps vector data to a terrain dataset.");
            arguments.getApplicationUsage().setCommandLineUsage(arguments.getApplicationName() + " [--input vector_file [options..]");
            arguments.getApplicationUsage().addCommandLineOption("-h or --help", "Display all available command line paramters");

            // request for help:
            if (arguments.read("-h") || arguments.read("--help"))
            {
                usage(arguments.getApplicationName(), 0);
                exit;
            }

            string str;

            // the input vector file that we will compile:
            while (arguments.read("--input", input_file)) ;

            // output file for vector geometry:
            while (arguments.read("--output", output_file)) ;

            // Color of the output geometry:
            while (arguments.read("--color", str))
                sscanf(str.c_str(), "%f,%f,%f,%f", &color.x(), &color.y(), &color.z(), &color.a());

            while (arguments.read("--random-colors"))
                color.a() = 0.0;

            while (arguments.read("--fade-lods"))
                fade_lods = true;

            while (arguments.read("--buffer", str))
                sscanf(str.c_str(), "%f", &buffer);

            // validate arguments:
            if (input_file.length() == 0)
            {
                arguments.getApplicationUsage().write(
                    std.cout,
                    osg.ApplicationUsage.COMMAND_LINE_OPTION);
                exit(-1);
            }
#endif
        }

        static FilterGraph createFilterGraph()
        {
            Registry registry = Registry.instance();

            // The FilterGraph is a series of filters that will transform the GIS
            // feature data into a scene graph.
            FilterGraph graph = new FilterGraph();

            // Buffer if necessary:
            if (buffer != 0.0f)
                graph.appendFilter(new BufferFilter(buffer));

            // Construct osg::Drawable's from the incoming feature batches:
            BuildGeomFilter gf = new BuildGeomFilter();
            gf.setColor(color);
            if (color.a() == 0)
                gf.setColorScript(new Script("vec4(math.random(),math.random(),math.random(),1)"));
            //gf.setRandomizeColors( color.a() == 0 );
            graph.appendFilter(gf);

            // Bring all the drawables into a single collection so that they
            // all fall under the same osg::Geode.
            graph.appendFilter(new CollectionFilter());

            // Construct a Node that contains the drawables and adjust its state set.
            BuildNodesFilter bnf = new BuildNodesFilter();
            //bnf.setDisableLighting( true );

            bnf.setAlphaBlending(true);
            bnf.setOptimize(false);

            graph.appendFilter(bnf);

            return graph;
        }

        // Variables that are set by command line arguments:
        static string input_file;
        static string output_file;
        static Vector4D color = new Vector4D(1, 1, 1, 1);
        static bool fade_lods = false;
        static float buffer = 0.0f;
    }
}

