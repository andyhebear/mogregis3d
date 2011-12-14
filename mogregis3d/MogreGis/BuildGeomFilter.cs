using System;
using System.Collections.Generic;

using Sharp3D.Math.Core;
using Mogre;

using Mogre.Utils.GluTesselator;

using NetTopologySuite.Geometries;
using GeoAPI.Geometries;
using GeoAPI.Coordinates;
using GeoAPI.IO.WellKnownText;
#if !unbuffered
using Coord = NetTopologySuite.Coordinates.Coordinate;
using CoordFac = NetTopologySuite.Coordinates.CoordinateFactory;
using CoordSeqFac = NetTopologySuite.Coordinates.CoordinateSequenceFactory;
//using Mogre.Demo.PolygonExample;
using GeoAPI.Operations.Buffer;
using NetTopologySuite.Coordinates;

#else
using Coord = NetTopologySuite.Coordinates.BufferedCoordinate;
using CoordFac = NetTopologySuite.Coordinates.BufferedCoordinateFactory;
using CoordSeqFac = NetTopologySuite.Coordinates.BufferedCoordinateSequenceFactory;
using Mogre.Demo.PolygonExample;

#endif

namespace MogreGis
{
    /**
     * Assembles feature data into basic fragments (i.e. attributed drawables).
     *
     * This filter takes Feature data as input, and generates Fragment data as output. 
     * (A Fragment is an osg.Drawable with an AttributeTable.) In other words it
     * create basic OSG geometry from GIS feature data.
     */
    public class BuildGeomFilter : FragmentFilter
    {
        //TODO OSGGIS_META_FILTER( BuildGeomFilter );
        public override string getFilterType() { return getStaticFilterType(); }
        public override Filter clone() { return new BuildGeomFilter(this); }
        public new static string getStaticFilterType() { return "BuildGeomFilter"; }
        public new static FilterFactory getFilterFactory() { return new FilterFactoryImpl<BuildGeomFilter>(); }


        /**
         * Constructs a new filter for converting features into geometry.
         */
        public BuildGeomFilter()
        {
            overall_color = new Vector4D(1, 1, 1, 1);
            setRasterOverlayMaxSize(DEFAULT_RASTER_OVERLAY_MAX_SIZE);

            //DefineResources();
            //InitializeResources();

        }

        /**
         * Copy constructor.
         */
        public BuildGeomFilter(BuildGeomFilter rhs)
            : base(rhs)
        {
            overall_color = rhs.overall_color;
            raster_overlay_max_size = rhs.raster_overlay_max_size;
            raster_overlay_script = rhs.raster_overlay_script;
            color_script = rhs.color_script;
            feature_name_script = rhs.feature_name_script;
        }


        //properties

        /**
         * Sets the overall color to assign to generated primitives.
         *
         * @param color
         *      A Vec4 color
         */
        public void setColor(Vector4D _color)
        {
            overall_color = _color;
        }

        /**
         * Gets the overall color to assign to generated primitives.
         *
         * @return OSG color vector
         */
        public Vector4D getColor()
        {
            return overall_color;
        }

        /**
         * Sets the script that evalutes to the color to apply to the geometry.
         *
         * @param script Script that generates the geometry color
         */
        public void setColorScript(Script script)
        {
            color_script = script;
        }

        /**
         * Gets the script that evaluates to the color to apply to the 
         * geometry.
         *
         * @return Script that generates the geometry color
         */
        public Script getColorScript()
        {
            return color_script;
        }

        /**
         * Sets a script that evaluates to the name of the RasterResource
         * to use to texture the geometry.
         *
         * @param script
         *      Script that generates the resource name
         */
        public void setRasterOverlayScript(Script script)
        {
            raster_overlay_script = script;
        }

        /**
         * Gets the script that evaluates to the name of the RasterResource
         * to use to texture the geometry.
         *
         * @return Script that generates the resource name
         */
        public Script getRasterOverlayScript()
        {
            return ((BuildGeomFilter)this).raster_overlay_script;
        }

        /**
         * Sets the maximum size (width or height) of a texture created from
         * the raster referenced by the raster script. Set this to 0 to disable
         * the capping.
         *
         * @param max_size
         *      Maximum width or height of the overlay texture
         */
        public void setRasterOverlayMaxSize(int max_size)
        {
            raster_overlay_max_size = max_size;
        }

        /**
         * Gets the maximum size (width or height) or a texture created from
         * the raster referenced by the raster script. A value less than or
         * equal to 0 means no capping.
         *
         * @return Maximum texture dimension
         */
        public int getRasterOverlayMaxSize()
        {
            return raster_overlay_max_size;
        }

        /**
         * Sets a script that evaluates to a string that this filter set as the
         * OSG node name.
         *
         * Important note: setting a feature name forces the filter to place 
         * each feature's geometry under a separate Geode. This prevents certain
         * optimizations (such as merging geometries) and can ultimatley affect
         * runtime performance.
         *
         * @param script
         *      Script that generates the feature node name
         */
        public void setFeatureNameScript(Script script)
        {
            feature_name_script = script;
        }

        /**
         * Gets a script that evaluates to the string that this filter sets as the
         * OSG node name.
         *
         * @return
         *      Script that generates the feature node name
         */
        public Script getFeatureNameScript()
        {
            return feature_name_script;
        }

        //Set properties
        public void setNameEntityINI(string name)
        {
            nameEntityINI = name;
        }

        public void setNameEntities(string name)
        {
            nameEntities = name;
        }

        public void setNameMaterial(string name)
        {
            nameMaterial = name;
        }

        //Get properties
        public string getNameEntityINI()
        {
            return nameEntityINI;
        }

        public string getNameEntities()
        {
            return nameEntities;
        }

        public string getNameMaterial()
        {
            return nameMaterial;
        }

        public Script Scale
        {
            set
            {
                this.scale = value;
            }

            get
            {
                return scale;
            }
        }

        public Script coordScale;
        public Script CoordScale
        {
            set
            {
                this.coordScale = value;
            }

            get
            {
                return coordScale;
            }
        }


        // Filter overrides    
        public override void setProperty(Property prop)
        {
            if (prop.getName() == "color")
                setColorScript(new Script(prop.getValue()));
            else if (prop.getName() == "raster_overlay")
                setRasterOverlayScript(new Script(prop.getValue()));
            else if (prop.getName() == "raster_overlay_max_size")
                setRasterOverlayMaxSize(prop.getIntValue(DEFAULT_RASTER_OVERLAY_MAX_SIZE));
            else if (prop.getName() == "feature_name")
                setFeatureNameScript(new Script(prop.getValue()));
            else if (prop.getName() == "pointNameEntityINI")
                setNameEntityINI(prop.getValue());
            else if (prop.getName() == "pointNameEntities")
                setNameEntities(prop.getValue());
            else if (prop.getName() == "nameMaterial")
                setNameMaterial(prop.getValue());
            else if (prop.getName() == "pointEntityScale")
            {
                //string s = prop.getValue().Substring(1, (prop.getValue().Length) - 2);
                //Scale = s;
                Scale = new Script(prop.getValue());
            }
            else if (prop.getName() == "distancesScale")
            {
                CoordScale = new Script(prop.getValue());
            }

            base.setProperty(prop);
        }

        public override Properties getProperties()
        {
            Properties p = base.getProperties();
            if (getColorScript() != null)
                p.Add(new Property("color", getColorScript().getCode()));
            if (getRasterOverlayScript() != null)
                p.Add(new Property("raster_overlay", getRasterOverlayScript().getCode()));
            if (getRasterOverlayMaxSize() != DEFAULT_RASTER_OVERLAY_MAX_SIZE)
                p.Add(new Property("raster_overlay_max_size", getRasterOverlayMaxSize()));
            if (getFeatureNameScript() != null)
                p.Add(new Property("feature_name", getFeatureNameScript()));
            return p;
        }


        // FragmentFilter overrides
        public override FragmentList process(FeatureList input, FilterEnv env)
        {
            FragmentList output = new FragmentList();

            //cuidado con las entidades dentro del for

            int i = 0;
            Vector3 scale;
            Vector3 distanceScale;

            if (Scale != null)
            {
                scale = Registry.instance().GetEngine("Python").run(Scale).asVec3();
            }
            else
            {
                scale = new Vector3(1, 1, 1);
            }

            if (CoordScale != null)
            {
                distanceScale = Registry.instance().GetEngine("Python").run(CoordScale).asVec3();
            }
            else
            {
                distanceScale = new Vector3(1, 1, 1);
            }
            

            SceneNode nodeIni = point3d(env.getName(), i, 0, 0, 0, null, env.getSceneMgr());
#if ESCALA_NODO_INICIAL
            if (Scale != null)
            {
                nodeIni.SetScale(Registry.instance().GetEngine("Python").run(Scale).asVec3());
            }
            if (coordScale != null)
            {
                Vector3 vec3 = Registry.instance().GetEngine("Python").run(Scale).asVec3();
                nodeIni.SetPosition(nodeIni.Position.x * vec3.x, nodeIni.Position.y * vec3.y, nodeIni.Position.z * vec3.z);
#if TRACE_BUILDGEOMFILTER
                        System.Console.WriteLine("(" + n.Position.x + "," + n.Position.y + ")");
#endif
            }
#endif
            Fragment fIni = new Fragment(nodeIni);
            output.Add(fIni);

            foreach (Feature feature in input)
            {
                //if type of features is Point
                if (feature.row.Geometry is SharpMap.Geometries.Point)
                {
                    SharpMap.Geometries.Point p = (SharpMap.Geometries.Point)feature.row.Geometry;

                    i++;
                    SceneNode n = point3d(env.getName(), i, (float)p.X, (float)p.Y, 0, nodeIni, env.getSceneMgr());

                    n.SetScale(scale);
                    n.SetPosition(n.Position.x * distanceScale.x, n.Position.y * distanceScale.y, n.Position.z * distanceScale.z);

                    Fragment f = new Fragment(n);
                    output.Add(f);
                }

                //if type of features is Polygon
                else if (feature.row.Geometry is SharpMap.Geometries.Polygon)
                {
                    SharpMap.Geometries.Polygon polygon = (SharpMap.Geometries.Polygon)feature.row.Geometry;

                    ManualObject polygonNode = null;

                    if (polygonNode == null)
                    {
                        polygonNode = env.getSceneMgr().CreateManualObject(env.getName() + "Node_" + i);
                        MaterialPtr material = MaterialManager.Singleton.Create("Test/ColourPolygon",
                                     ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                        material.GetTechnique(0).GetPass(0).VertexColourTracking =
                                       (int)TrackVertexColourEnum.TVC_AMBIENT;

                        Vector3 v = Registry.instance().GetEngine("Python").run(Color,feature,null).asVec3();
                        MogreTessellationCallbacks callback = new MogreTessellationCallbacks(polygonNode, v);

                        GLUtessellatorImpl Glu = (GLUtessellatorImpl)GLUtessellatorImpl.gluNewTess();
                        Glu.gluTessCallback(GLU.GLU_TESS_VERTEX, callback);
                        Glu.gluTessCallback(GLU.GLU_TESS_BEGIN, callback);
                        Glu.gluTessCallback(GLU.GLU_TESS_END, callback);
                        Glu.gluTessCallback(GLU.GLU_TESS_ERROR, callback);
                        Glu.gluTessCallback(GLU.GLU_TESS_COMBINE, callback);
                        Glu.gluTessBeginPolygon(null);
                        Glu.gluTessBeginContour();

                        int numVertices = polygon.ExteriorRing.NumPoints/*/10+1*/;
                        int numValores = 3;
                        double[][] data = new double[numVertices][];

                        for (int j = 0; j < numVertices; j++)
                        {
                            data[j] = new double[numValores];
                        }

                        int k = 0;
                        //1 polygon = N vertices
                        foreach (SharpMap.Geometries.Point point in polygon.ExteriorRing.Vertices)
                        {
                            //if (k % 10 == 0)
                            {
                                data[k/*/10*/][0] = point.X;
                                data[k/*/10*/][1] = point.Y;
                                data[k/*/10*/][2] = 0;
                            }
                            k++;

                            //SceneNode n = point3d(env.getName(), k + 10, (float)point.X * 10.0f, (float)point.Y * 10.0f, 0, nodeIni, env.getSceneMgr());

                        } 
                        for (int j = 0; j < data.GetLength(0); j++)
                        {
                            Glu.gluTessVertex(data[j], 0, new Vector3((float)(data[j][1] * distanceScale.y), (float)(data[j][2] * distanceScale.z), (float)(data[j][0] * distanceScale.x)));
                        }

                        Glu.gluTessEndContour();
                        Glu.gluTessNormal(0, 0, 1);
                        Glu.gluTessEndPolygon();

                        //polygonNode.SetMaterialName((uint)0, nameMaterial);
                       // polygonNode.Begin(nameMaterial);
                   
                        nodeIni.AttachObject(polygonNode);

                    }
                    i++;
                }

                //if type of features is MultiPolygon
                else if (feature.row.Geometry is SharpMap.Geometries.MultiPolygon)
                {
                    SharpMap.Geometries.MultiPolygon mp = (SharpMap.Geometries.MultiPolygon)feature.row.Geometry;

                    // 1 MultiPolygon = N polygon
                    foreach (SharpMap.Geometries.Polygon polygon in mp.Polygons)
                    {

                        ManualObject polygonNode = null;

                        if (polygonNode == null)
                        {
                            polygonNode = env.getSceneMgr().CreateManualObject(env.getName() + "Node_" + i);
                            MaterialPtr material = MaterialManager.Singleton.Create("Test/ColourPolygon",
                                     ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                            material.GetTechnique(0).GetPass(0).VertexColourTracking =
                                           (int)TrackVertexColourEnum.TVC_AMBIENT;

                            Vector3 v = Registry.instance().GetEngine("Python").run(Color, feature, null).asVec3();
                            MogreTessellationCallbacks callback = new MogreTessellationCallbacks(polygonNode, v);

                            GLUtessellatorImpl Glu = (GLUtessellatorImpl)GLUtessellatorImpl.gluNewTess();
                            Glu.gluTessCallback(GLU.GLU_TESS_VERTEX, callback);
                            Glu.gluTessCallback(GLU.GLU_TESS_BEGIN, callback);
                            Glu.gluTessCallback(GLU.GLU_TESS_END, callback);
                            Glu.gluTessCallback(GLU.GLU_TESS_ERROR, callback);
                            Glu.gluTessCallback(GLU.GLU_TESS_COMBINE, callback);
                            Glu.gluTessBeginPolygon(null);
                            Glu.gluTessBeginContour();

                            int numVertices = polygon.ExteriorRing.NumPoints;
                            int numValores = 3;
                            double[][] data = new double[numVertices][];

                            for (int j = 0; j < numVertices; j++)
                            {
                                data[j] = new double[numValores];
                            }

                            int k = 0;
                            //1 polygon = N vertices
                            foreach (SharpMap.Geometries.Point point in polygon.ExteriorRing.Vertices)
                            {

                                data[k][0] = point.X;
                                data[k][1] = point.Y;
                                data[k][2] = 0;

                                k++;

                                //SceneNode n = point3d(env.getName(), i, (float)point.X, (float)point.Y, 0, nodeIni, env.getSceneMgr());

                            }
                            for (int j = 0; j < data.GetLength(0); j++)
                            {
                                Glu.gluTessVertex(data[j], 0, new Vector3(((float)data[j][1]) * distanceScale.y, ((float)data[j][2]) * distanceScale.z, ((float)data[j][0]) * distanceScale.x));
                            }

                            Glu.gluTessEndContour();
                            Glu.gluTessNormal(0, 0, 1);
                            Glu.gluTessEndPolygon();

                            //polygonNode.SetMaterialName((uint)0, nameMaterial);
                            //polygonNode.Begin(nameMaterial);

                            nodeIni.AttachObject(polygonNode);
                            

                        }
                        i++;
                    }
                }

                //if type of features is Line
                else if (feature.row.Geometry is SharpMap.Geometries.LineString)
                {
                    ManualObject lineNode = env.getSceneMgr().CreateManualObject("line" + i);
                    MaterialPtr material = MaterialManager.Singleton.Create("Test/ColourPolygon",
                             ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                    material.GetTechnique(0).GetPass(0).VertexColourTracking =
                                   (int)TrackVertexColourEnum.TVC_AMBIENT;

                    int nSeg = 5; // Number of segments on the cap or join pieces
                    BufferParameters param = new BufferParameters(nSeg, BufferParameters.BufferEndCapStyle.CapRound, BufferParameters.BufferJoinStyle.JoinRound, 2);
                    IGeometryFactory<Coord> geometryFactory = new GeometryFactory<Coord>(new CoordSeqFac(new CoordFac(PrecisionModelType.DoubleFloating)));
                    IWktGeometryReader<Coord> reader = geometryFactory.WktReader;
                    ILineString line1 = (ILineString<Coord>)reader.Read(feature.row.Geometry.AsText());
                    IGeometry coordBuffer = line1.Buffer(0.5, param);
                    ICoordinateSequence coords = coordBuffer.Coordinates;
                    Vector3 v = Registry.instance().GetEngine("Python").run(Color, feature, null).asVec3();
                    MogreTessellationCallbacks callback = new MogreTessellationCallbacks(lineNode, v);

                    GLUtessellatorImpl Glu = (GLUtessellatorImpl)GLUtessellatorImpl.gluNewTess();
                    Glu.gluTessCallback(GLU.GLU_TESS_VERTEX, callback);
                    Glu.gluTessCallback(GLU.GLU_TESS_BEGIN, callback);
                    Glu.gluTessCallback(GLU.GLU_TESS_END, callback);
                    Glu.gluTessCallback(GLU.GLU_TESS_ERROR, callback);
                    Glu.gluTessCallback(GLU.GLU_TESS_COMBINE, callback);
                    Glu.gluTessBeginPolygon(null);
                    Glu.gluTessBeginContour();
                    foreach (Coord coord in coords)
                    {
                        double[] data = new double[] { coord.X * distanceScale.x, coord.Y * distanceScale.y, (double.IsNaN(coord.Z) ? 0 : coord.Z)*distanceScale.z };

                        Glu.gluTessVertex(data, 0, new Vector3((float)data[1], (float)data[2], (float)data[0]));
                    }
                    Glu.gluTessEndContour();
                    Glu.gluTessNormal(0, 0, 1);
                    Glu.gluTessEndPolygon();
                    i++;
                    nodeIni.AttachObject(lineNode);
                    //SceneNode node1 = env.getSceneMgr().RootSceneNode.CreateChildSceneNode("Line3Node");
                    //node1.AttachObject(line.CreateNode("Line3", base.sceneMgr, true));
                }
                if ((feature.row.Geometry is SharpMap.Geometries.Polygon) | (feature.row.Geometry is SharpMap.Geometries.MultiPolygon))
                {
                    Fragment f = new Fragment(nodeIni);
                    output.Add(f);
                }

            }

            i = 0;//breakpoint

            /*foreach (Fragment fragment in output)
            {
                fragment.Node.Scale(0,0,0);
            }*/

#if TODO
            // if features are arriving in batch, resolve the color here.
            // otherwise we will resolve it later in process(feature,env).
            is_batch = input.Count > 1;
            batch_feature_color = overall_color;
            if (is_batch && getColorScript() != null)
            {
                ScriptResult r = env.getScriptEngine().run(getColorScript(), env);
                if (r.isValid())
                    batch_feature_color = r.asVec4();
                else
                    env.getReport().error(r.asString());
            }

            return base.process(input, env);
#endif
            //throw new NotImplementedException();

            if (successor != null)
            {
                if (successor is FeatureFilter)
                {
                    FeatureFilter filter = (FeatureFilter)successor;
                    FeatureList l = filter.process(input, env);
                    //FeatureList l = successor.process(input, env);
                }
                else if (successor is FragmentFilter)
                {
                    FragmentFilter filter = (FragmentFilter)successor;
                    FragmentList l = filter.process(output, env);
                }
            }

            return output;
        }

        public SceneNode point3d(string name, int id, float x, float y, float z, SceneNode node, SceneManager sceneMgr)
        {
            Entity ent;
            if (node == null)//point of reference 0,0,0
            {
                ent = sceneMgr.CreateEntity(name + id, getNameEntityINI());
                node = sceneMgr.RootSceneNode.CreateChildSceneNode(name + id + "Node", new Vector3(y, z, x));
                node.AttachObject(ent);
                return node;
            }
            else//create new point
            {
                float xAux = x;
                float yAux = y;

                SceneNode nodeAux;
                
                ent = sceneMgr.CreateEntity(name + id, getNameEntities());
                nodeAux = node.CreateChildSceneNode(name + "Node_" + id, new Vector3(yAux, z, xAux));

                nodeAux.AttachObject(ent);
                return nodeAux;
            }
        }

        public class MogreTessellationCallbacks : GLUtessellatorCallback
        {
            ManualObject manualObj;
            Vector3 vec3;

            public MogreTessellationCallbacks(ManualObject mo, Vector3 color)
            {
                manualObj = mo;

                vec3 = color;
            }

            #region GLUtessellatorCallback Members

            public void begin(int type)
            {
                string typeName;
                switch (type)
                {
                    case GL.GL_LINE_LOOP:
                        typeName = "GL_LINE_LOOP";
                        break;
                    case GL.GL_TRIANGLE_FAN:
                        typeName = "GL_TRIANGLE_FAN";
                        manualObj.Begin("Test/ColourPolygon", RenderOperation.OperationTypes.OT_TRIANGLE_FAN);
                        break;
                    case GL.GL_TRIANGLE_STRIP:
                        typeName = "GL_TRIANGLE_STRIP";
                        manualObj.Begin("Test/ColourPolygon", RenderOperation.OperationTypes.OT_TRIANGLE_STRIP);
                        break;
                    case GL.GL_TRIANGLES:
                        typeName = "GL_TRIANGLES";
                        manualObj.Begin("Test/ColourPolygon", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
                        break;
                    default:
                        typeName = "Unknown";
                        break;
                }
#if TESSELLATION_TRACE
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod() + "Primitive type = " + typeName);
#endif
            }

            public void beginData(int type, object polygonData)
            {
#if TESSELLATION_TRACE
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
#endif
            }

            public void edgeFlag(bool boundaryEdge)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            }

            public void edgeFlagData(bool boundaryEdge, object polygonData)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            }

            public void vertex(object vertexData)
            {
#if TESSELLATION_TRACE
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod() + " vertex=" + vertexData);
#endif
                manualObj.Position((Vector3)vertexData);
                
                manualObj.Colour(vec3.x, vec3.y, vec3.z);
            }

            public void vertexData(object vertexData, object polygonData)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            }

            public void end()
            {
#if TESSELLATION_TRACE
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
#endif
                manualObj.End();
            }

            public void endData(object polygonData)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            }

            public void combine(double[] coords, object[] data, float[] weight, object[] outData)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            }

            public void combineData(double[] coords, object[] data, float[] weight, object[] outData, object polygonData)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            }

            public void error(int errnum)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            }

            public void errorData(int errnum, object polygonData)
            {
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod());
            }

            #endregion
        }

        public override FragmentList process(Feature input, FilterEnv env)
        {

            FragmentList output;

            // LIMITATION: this filter assumes all feature's shapes are the same
            // shape type! TODO: sort into bins of shape type and create a separate
            // geometry for each. Then merge the geometries.
            bool needs_tessellation = false;

            Fragment frag = new Fragment();

            GeoShapeList shapes = input.getShapes();
#if TODO
            // if we're in batch mode, the color was resolved in the other process() function.
            // otherwise we still need to resolve it.
            Vector4D color = getColorForFeature(input, env);
#endif
#if TODO
            foreach (GeoShape s in shapes)
            {
                GeoShape shape = s;

                if (shape.getShapeType() == GeoShape.ShapeType.TYPE_POLYGON)
                {
                    needs_tessellation = true;
                }
                

                osg.Geometry geom = new osg.Geometry();

                // TODO: pre-total points and pre-allocate these arrays:
                osg.Vec3Array verts = new osg.Vec3Array();
                geom.setVertexArray(verts);
                uint vert_ptr = 0;

                // per-vertex coloring takes more memory than per-primitive-set coloring,
                // but it renders faster.
                osg.Vec4Array colors = new osg.Vec4Array();
                geom.setColorArray(colors);
                geom.setColorBinding(osg.Geometry.BIND_PER_VERTEX);

                //osg.Vec3Array* normals = new osg.Vec3Array();
                //geom.setNormalArray( normals );
                //geom.setNormalBinding( osg.Geometry.BIND_OVERALL );
                //normals.push_back( osg.Vec3( 0, 0, 1 ) );


                Mogre.PixelFormat prim_type =
                    shape.getShapeType() == GeoShape.ShapeType.TYPE_POINT ? osg.PrimitiveSet.POINTS :
                    shape.getShapeType() == GeoShape.ShapeType.TYPE_LINE ? osg.PrimitiveSet.LINE_STRIP :
                    osg.PrimitiveSet.LINE_LOOP;
#endif
#if TODO
                for (int pi = 0; pi < shape.getPartCount(); pi++)
                {
                    int part_ptr = vert_ptr;
                    GeoPointList points = shape.getPart(pi);
                    for (int vi = 0; vi < points.Count; vi++)
                    {
                        verts.Add(points[vi]);
                        vert_ptr++;
                        colors.Add(color);
                    }
                    geom.addPrimitiveSet(new osg.DrawArrays(prim_type, part_ptr, vert_ptr - part_ptr));
                }

                // tessellate all polygon geometries. Tessellating each geometry separately
                // with TESS_TYPE_GEOMETRY is much faster than doing the whole bunch together
                // using TESS_TYPE_DRAWABLE.
                if (needs_tessellation)
                {
                    osgUtil.Tessellator tess;
                    tess.setTessellationType(osgUtil.Tessellator.TESS_TYPE_GEOMETRY);
                    tess.setWindingType(osgUtil.Tessellator.TESS_WINDING_POSITIVE);
                    tess.retessellatePolygons(*geom);

                    applyOverlayTexturing(geom, input, env);
                }

                generateNormals(geom);

                frag.addDrawable(geom);
            }

            frag.addAttributes(input.getAttributes());
            applyFragmentName(frag, input, env);

            output.Add(frag);

            return output;
#endif
            throw new NotImplementedException();
        }

#if TODO
        protected virtual Vector4D getColorForFeature(Feature input, FilterEnv env)
        {

            Vector4D result = overall_color;

            if (is_batch)
            {
                result = batch_feature_color;
            }
            else if (getColorScript() != null)
            {
                ScriptResult r = env.getScriptEngine().run(getColorScript(), input, env);
                if (r.isValid())
                    result = r.asVec4();
                else
                    env.getReport().error(r.asString());
            }

            return result;
        }
        protected void applyFragmentName(Fragment frag, Feature feature, FilterEnv env)
        {
            if (getFeatureNameScript() != null)
            {
                ScriptResult r = env.getScriptEngine().run(getFeatureNameScript(), feature, env);
                if (r.isValid())
                    frag.setName(r.asString());
                else
                    env.getReport().error(r.asString());
            }
        }

        protected void applyOverlayTexturing(osg.Geometry geom, Feature input, FilterEnv env)
        {
            GeoExtent tex_extent;

            if (getRasterOverlayScript() != null)
            {
                // if there's a raster script for this filter, we're applying textures per-feature:
                tex_extent = new GeoExtent(
                    input.getExtent().getSouthwest().getAbsolute(),
                    input.getExtent().getNortheast().getAbsolute());
            }
            else
            {
                // otherwise prepare the geometry for an overlay texture covering the entire working extent:
                tex_extent = env.getExtent();
            }

            float width = (float)tex_extent.getWidth();
            float height = (float)tex_extent.getHeight();

            // now visit the verts and calculate texture coordinates for each one.
            osg.Vec3Array verts = (osg.Vec3Array)(geom.getVertexArray());
            if (verts != null)
            {
                // if we are dealing with geocentric data, we will need to xform back to a real
                // projection in order to determine texture coords:
                GeoExtent tex_extent_geo;
                if (env.getInputSRS().isGeocentric())
                {
                    tex_extent_geo = new GeoExtent(
                        tex_extent.getSRS().getGeographicSRS().transform(tex_extent.getSouthwest()),
                        tex_extent.getSRS().getGeographicSRS().transform(tex_extent.getNortheast()));
                }

                osg.Vec2Array texcoords = new osg.Vec2Array(verts.size());
                for (int j = 0; j < verts.size(); j++)
                {
                    // xform back to raw SRS w.o. ref frame:
                    GeoPoint vert = new GeoPoint(verts[j], env.getInputSRS());
                    GeoPoint vert_map = vert.getAbsolute();
                    float tu, tv;
                    if (env.getInputSRS().isGeocentric())
                    {
                        tex_extent_geo.getSRS().transformInPlace(vert_map);
                        tu = (vert_map.X - tex_extent_geo.getXMin()) / width;
                        tv = (vert_map.Y - tex_extent_geo.getYMin()) / height;
                    }
                    else
                    {
                        tu = (vert_map.X - tex_extent.getXMin()) / width;
                        tv = (vert_map.Y - tex_extent.getYMin()) / height;
                    }
                    (*texcoords)[j].set(tu, tv);
                }
                geom.setTexCoordArray(0, texcoords);
            }

            // if we are applying the raster per-feature, do so now.
            // TODO: deprecate? will we ever use this versus the BuildNodesFilter overlay? maybe
            if (getRasterOverlayScript() != null)
            {
                ScriptResult r = env.getScriptEngine().run(getRasterOverlayScript(), input, env);
                if (r.isValid())
                {
                    RasterResource raster = env.getSession().getResources().getRaster(r.asString());
                    if (raster != null)
                    {
                        Image image = null;
                        std.stringstream builder;
                        builder << "rtex_" << input.getOID() << ".jpg"; //TODO: dds with DXT1 compression

                        osg.StateSet raster_ss = new osg.StateSet();
                        if (raster.applyToStateSet(raster_ss.get(), tex_extent, getRasterOverlayMaxSize(), out image))
                        {
                            image.setFileName(builder.str());
                            geom.setStateSet(raster_ss.get());

                            // add this as a skin resource so the compiler can properly localize and deploy it.
                            env.getResourceCache().addSkin(raster_ss.get());
                        }
                    }
                }
                else
                {
                    env.getReport().error(r.asString());
                }
            }
        }

        protected void generateNormals(osg.Geometry geom)
        {
            if (geom != null)
            {
                osgUtil.SmoothingVisitor smoother;
                smoother.smooth(out geom);
            }
        }
#endif
        public Script Color { get { return color_script; } }

        protected Script color_script;
        protected Script feature_name_script;
        protected Vector4D overall_color;
        protected Script raster_overlay_script;
        protected int raster_overlay_max_size;
        protected bool embed_attributes;

        // transients
        private bool is_batch;
        private Vector4D batch_feature_color;

        //TODO OSGGIS_DEFINE_FILTER( BuildGeomFilter );

        private const int DEFAULT_RASTER_OVERLAY_MAX_SIZE = 0;

        protected string nameEntityINI;
        protected string nameEntities;
        protected string nameMaterial;
        protected Script scale;
    }
}
