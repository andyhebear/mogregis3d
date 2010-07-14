using System;
using System.Collections.Generic;
using System.Text;

//using Mogre.Helpers;

namespace Mogre.Demo.Primitives
{
    public class Lines3Dv2
    {
        const uint constantColor = 123;

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

        private void CreateLine(ManualObject mo, Vector3 origin, Vector3 final, int useMaterial, Vector4 color)
        {
            float lineWidth = 0.1f;

            RenderOperation.OperationTypes operation = RenderOperation.OperationTypes.OT_TRIANGLE_FAN;
            switch (useMaterial)
            {
                case 1:
                    mo.Begin("SharpMap/Line3D_v1", operation);
                    break;
                default:
                    MaterialPtr material = MaterialManager.Singleton.CreateOrRetrieve("Test/ColourLines3d",
                                                                            ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME).first;
                    material.GetTechnique(0).GetPass(0).VertexColourTracking =
                                   (int)TrackVertexColourEnum.TVC_AMBIENT;
                    mo.Begin("Test/ColourLines3d", operation);

                    break;
            }

            Vector3 delta = final - origin;
            delta = new Vector3(-delta.y, delta.x, delta.z).NormalisedCopy * lineWidth;
            mo.Position(origin);
            mo.Position(final);
            mo.Position(final + delta);
            mo.Position(origin + delta);
            //lines3d.TextureCoord((float)i / (float)pointsList.Count);

            ManualObject.ManualObjectSection section = lines3d.End();
            section.SetCustomParameter(constantColor, color);
        }

        public ManualObject CreateNode(string name, SceneManager sceneMgr, bool isClosed, int useMaterial, Vector4 color)
        {
            if (lines3d == null)
            {
                lines3d = sceneMgr.CreateManualObject(name);
                for (int i = 0; i < pointsList.Count - 1; i++)
                {
                    CreateLine(lines3d, pointsList[i], pointsList[i + 1], useMaterial, color);
                }
                if (isClosed && pointsList.Count > 1)
                {
                    CreateLine(lines3d, pointsList[pointsList.Count - 1], pointsList[0], useMaterial, color);
                }

            }
            return lines3d;
        }

        protected List<Vector3> pointsList = new List<Vector3>();
        protected ManualObject lines3d;
    }

    public class Lines3DExample2 : Mogre.Demo.ExampleApplication.Example
    {
        public override void CreateScene()
        {
            Vector4 green = new Vector4(0.0f, 1.0f, 0.0f, 1.0f);
            Vector4 red = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);

            Lines3Dv2 lines3d = new Lines3Dv2();
            lines3d.AddPoint(-10, 10, 0);
            lines3d.AddPoint(0, 5, 0);
            lines3d.AddPoint(8, 3, 0);
            lines3d.AddPoint(0, 10, 0);

            SceneNode node1 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Lines3DNode_1");
            node1.AttachObject(lines3d.CreateNode("Lines3d_1", base.sceneMgr, true, 1, green));

            Lines3Dv2 lines3d_2 = new Lines3Dv2();
            lines3d_2.AddPoint(3, 3, 0);
            lines3d_2.AddPoint(8, 8, 0);
            lines3d_2.AddPoint(3+5, 3, 0);
            lines3d_2.AddPoint(5, 3, 0);

            SceneNode node2 = base.sceneMgr.RootSceneNode.CreateChildSceneNode("Lines3DNode_2");
            node2.AttachObject(lines3d_2.CreateNode("Lines3d_2", base.sceneMgr, false, 1, red));

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

        public override void SetupResources()
        {
            base.SetupResources();
            ResourceGroupManager.Singleton.AddResourceLocation("./CgResources/SharpMap", "FileSystem", "General");
        }
    }
}