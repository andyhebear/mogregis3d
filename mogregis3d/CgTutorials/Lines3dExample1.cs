using System;
using System.Collections.Generic;
using System.Text;

//using Mogre.Helpers;

namespace Mogre.Demo.Primitives
{
    public class Lines3D
    {
        public void AddPoint(float x, float y, float z)
        {
            pointsList.Add(new Vector3(x, y, z));
        }

        public void AddPoint(Vector3 point)
        {
            pointsList.Add(point);
        }

        public Vector3 GetPoint(ushort index)
        {
            return pointsList[index];
        }

        public ushort NumPoints
        {
            get { return (ushort)pointsList.Count; }
        }

        public void UpdatePoint(ushort index, Vector3 newPoint)
        {
            pointsList[index] = newPoint;
        }

        public ManualObject CreateNode(string name, SceneManager sceneMgr, bool isClosed)
        {
            if (lines3d == null)
            {
                lines3d = sceneMgr.CreateManualObject(name);
                MaterialPtr material = MaterialManager.Singleton.Create("Test/ColourLines3d",
                         ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
                material.GetTechnique(0).GetPass(0).VertexColourTracking =
                               (int)TrackVertexColourEnum.TVC_AMBIENT;
                lines3d.Begin("Test/ColourLines3d", RenderOperation.OperationTypes.OT_LINE_STRIP);
                for (int i = 0; i < pointsList.Count; i++)
                {
                    lines3d.Position(pointsList[i]);
                }
                if (isClosed && pointsList.Count > 1)
                    lines3d.Position(pointsList[0]);
                lines3d.End();

            }
            return lines3d;
        }

        protected List<Vector3> pointsList = new List<Vector3>();
        protected ManualObject lines3d;
    }

    public class Lines3DExample1 : Mogre.Demo.ExampleApplication.Example
    {
        public override void CreateScene()
        {
            Lines3D lines3d = new Lines3D();
            lines3d.AddPoint(0, 0, 0);
            lines3d.AddPoint(5, 0, 0);
            lines3d.AddPoint(5, 5, 0);
            lines3d.AddPoint(0, 5, 0);

            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Lines3DNode");
            node1.AttachObject(lines3d.CreateNode("Lines3d", base.sceneMgr, true));
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
    }
}