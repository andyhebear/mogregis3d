using System;
using System.Collections.Generic;
using System.Text;

using Mogre.Utils.GluTesselator;
using Mogre.Helpers;

using NetTopologySuite.Geometries;
using GeoAPI.Geometries;
using GeoAPI.Coordinates;
using GeoAPI.IO.WellKnownText;
#if !unbuffered
using Coord = NetTopologySuite.Coordinates.Coordinate;
using CoordFac = NetTopologySuite.Coordinates.CoordinateFactory;
using CoordSeqFac = NetTopologySuite.Coordinates.CoordinateSequenceFactory;
using Mogre.Demo.PolygonExample;
using GeoAPI.Operations.Buffer;
using NetTopologySuite.Coordinates;

#else
using Coord = NetTopologySuite.Coordinates.BufferedCoordinate;
using CoordFac = NetTopologySuite.Coordinates.BufferedCoordinateFactory;
using CoordSeqFac = NetTopologySuite.Coordinates.BufferedCoordinateSequenceFactory;
using Mogre.Demo.PolygonExample;

#endif

namespace Mogre.Demo.Primitives
{
    public class Lines3Dv3
    {
        private static IGeometryFactory<Coord> geometryFactory;
        private static ICoordinateFactory<Coord> coordFactory;
        private static IWktGeometryReader<Coord> reader;

        static Lines3Dv3()
        {
            geometryFactory = new GeometryFactory<Coord>(new CoordSeqFac(new CoordFac(PrecisionModelType.DoubleFloating)));
            reader = geometryFactory.WktReader;
            coordFactory = geometryFactory.CoordinateFactory;
        }

        ILineString<Coord> line1 = (ILineString<Coord>)reader.Read("LINESTRING (-10 10, 10 10, 10 -10, 0 0)");
        ILineString<Coord> line2;

        public void AddPoint(float x, float y, float z)
        {
            pointsList.Add(coordFactory.Create(x, y, z));
        }

        public ManualObject CreateNode(string name, SceneManager sceneMgr, bool isClosed)
        {
            if (lineNode == null)
            {
                lineNode = sceneMgr.CreateManualObject(name);
                MaterialPtr material = MaterialManager.Singleton.Create("Test/ColourPolygon",
                         ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                material.GetTechnique(0).GetPass(0).VertexColourTracking =
                               (int)TrackVertexColourEnum.TVC_AMBIENT;

                int nSeg = 5; // Number of segments on the cap or join pieces
                BufferParameters param = new BufferParameters(nSeg, BufferParameters.BufferEndCapStyle.CapRound, BufferParameters.BufferJoinStyle.JoinRound, 2);
                IGeometry coordBuffer = line1.Buffer(0.5, param);

                Tesselate(coordBuffer.Coordinates);
            }
            return lineNode;
        }

        private void Tesselate(ICoordinateSequence coords)
        {
            MogreTessellationCallbacks callback = new MogreTessellationCallbacks(lineNode);

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
                double[] data = new double[] { coord.X, coord.Y, (double.IsNaN(coord.Z) ? 0 : coord.Z) };

                Glu.gluTessVertex(data, 0, new Vector3((float)data[0], (float)data[1], (float)data[2]));
            }
            Glu.gluTessEndContour();
            Glu.gluTessNormal(0, 0, 1);
            Glu.gluTessEndPolygon();
        }

        protected ManualObject lineNode;
        protected List<ICoordinate> pointsList = new List<ICoordinate>();
    }

    public class Lines3DExample3 : Mogre.Demo.ExampleApplication.Example
    {

        public override void CreateScene()
        {
            Lines3Dv3 line = new Lines3Dv3();

            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Line3Node");
            node1.AttachObject(line.CreateNode("Line3", base.sceneMgr, true));
        }

        public override void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("PlayerCam");

            // Position it at 30 in Z direction
            camera.Position = new Vector3(0, 0, 30);
            // Look at our line
            camera.LookAt(new Vector3(5, 5, 5));
            camera.NearClipDistance = 1;
        }

        public override void CreateFrameListener()
        {
            base.CreateFrameListener();

            root.FrameStarted += new FrameListener.FrameStartedHandler(FrameStartedHandler);
        }

        bool FrameStartedHandler(FrameEvent evt)
        {
            //if(ExampleFrameListener.frameStarted(evt) == false)
            //return false;
            return true;
        }
    }
}