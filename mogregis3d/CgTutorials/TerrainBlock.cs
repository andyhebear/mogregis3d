using System;
using System.Collections.Generic;
using System.Text;

using Mogre.Helpers;

namespace Mogre.Demo.CgTutorials
{

    /// <summary>
    /// 
    /// </summary>
    class TerrainBlock1 : Mogre.Demo.ExampleApplication.Example
    {
        const int SIZE = 128 - 1;
        int blockSize = (SIZE + 1) / 4;

        const uint TransXYscaleXY = 3001;
        const uint ColorAndHeightScale = 3002;


        static ManualObject.ManualObjectSection BuildTerrainBlock(ManualObject obj, ushort pointsX, ushort pointsY,
                                              Vector4 translationScale, Vector4 color)
        {

            RenderOperation.OperationTypes operation = RenderOperation.OperationTypes.OT_TRIANGLE_STRIP;

            obj.Begin("Terrain/Terrain_Material", operation);
            /*
             * This function uses one big zigzag triangle strip for the whole grid. 
             * And insert degenerated (invisible) triangles to join 2 rows. For instance:
             * 0  1  2  3
             * 4  5  6  7
             * 8  9  10 11
             * 12 13 14 15
             * 
             * The index buffer would look like :
             * 0, 4, 1, 5, 2, 6, 3, 7, 7, 7, 11, 6, 10, 5, 9, 4, 8, 8, 8, 12, 9, 13, 10, 14, 11, 15
             */
            for (int y = pointsY - 1; y >= 0; y--)
            {
                for (int x = 0; x <= pointsX - 1; x++)
                {
                    obj.Position(x, y, 0.0f);
                    obj.TextureCoord((float)x / (float)(pointsX), (float)y / (float)(pointsY));
                }
            }
            //Console.Write("\n Index:");
            // We have pointsX -1 lines
            for (ushort y = 0; y < pointsY - 1; y++)
            {
                if (y % 2 == 0)
                {
                    for (int x = 0; x < pointsX; x++)
                    {
                        obj.Index((ushort)(y * pointsX + x));            //(x, y + 1, 0.0f);
                        //Console.Write("," + (y * pointsX + x));
                        obj.Index((ushort)(y * pointsX + x + pointsX));  //(x, y, 0.0f);
                        //Console.Write("," + (y * pointsX + x + pointsX));

                    }

                    if (y != pointsY - 2)
                    {
                        obj.Index((ushort)(y * pointsX + pointsX - 1));         //(0, y+1, 0.0f);
                        //Console.Write("," + (y * pointsX + 2 * pointsX - 1));
                    }
                }
                else
                {
                    for (int x = pointsX - 1; x >= 0; x--)
                    {
                        obj.Index((ushort)(y * pointsX + x));           //(x, y + 1, 0.0f);
                        //Console.Write(", " + (y * pointsX + x));
                        obj.Index((ushort)(y * pointsX + x + pointsX));  //(x, y, 0.0f);
                        //Console.Write(", " + (y * pointsX + x + pointsX));
                    }
                    if (y != pointsY - 2)
                    {
                        obj.Index((ushort)(y * pointsX + pointsX));         //(0, y+1, 0.0f);
                        //Console.Write(", " + (y * pointsX + pointsX));
                    }

                }
            }
            //Console.WriteLine(";");
            ManualObject.ManualObjectSection manualObjSec1 = obj.End();
            obj.BoundingBox = new AxisAlignedBox(translationScale.x,
                                                 translationScale.y, -1,
                                                (pointsX * translationScale.z + translationScale.x),
                                                (pointsY * translationScale.w + translationScale.y), 1);
            manualObjSec1.SetCustomParameter(TransXYscaleXY, translationScale);
            manualObjSec1.SetCustomParameter(ColorAndHeightScale, color);
            return manualObjSec1;
        }

        const int scale = 0;
        Vector4 greenBlock = new Vector4(0, 0.7f, 0, scale);
        Vector4 redBlock = new Vector4(0.7f, 0, 0, scale);

        SceneNode[] nodesL0 = new SceneNode[4];
        ManualObject.ManualObjectSection[] objSect = new ManualObject.ManualObjectSection[4];

        public override void CreateScene()
        {
            ushort points = (ushort)(blockSize + 1);

            for (int i = 0; i < nodesL0.Length; i++)
            {
                nodesL0[i] = base.sceneMgr.RootSceneNode.CreateChildSceneNode("TerrainBlockNodeL0_" + i);
            }

            ManualObject[] manualObjsL0 = new ManualObject[nodesL0.Length];
            for (int i = 0; i < manualObjsL0.Length; i++)
            {
                manualObjsL0[i] = sceneMgr.CreateManualObject("TerrainBlockObjectL0_" + i);
            }

            objSect[0] = BuildTerrainBlock(manualObjsL0[0], points, points, new Vector4(-blockSize, 0, 1, 1), greenBlock);
            objSect[1] = BuildTerrainBlock(manualObjsL0[1], points, points, new Vector4(0, 0, 1, 1), redBlock);
            objSect[2] = BuildTerrainBlock(manualObjsL0[2], points, points, new Vector4(-blockSize, -blockSize, 1, 1), redBlock);
            objSect[3] = BuildTerrainBlock(manualObjsL0[3], points, points, new Vector4(0, -blockSize, 1, 1), greenBlock);

            for (int i = 0; i < nodesL0.Length; i++)
            {
                nodesL0[i].AttachObject(manualObjsL0[i]);
            }


        }

        public override void CreateCamera()
        {
            // Create the camera
            camera = sceneMgr.CreateCamera("PlayerCam");

            // Position the camera
            camera.Position = new Vector3(0, -0.3f * (SIZE + 1), SIZE / 2);
            // Look at the center of our block
            camera.LookAt(new Vector3(0, 0, 0));
            camera.NearClipDistance = 1;
            camera.FarClipDistance = 6000f;
        }

        public override void SetupResources()
        {
            base.SetupResources();
            ResourceGroupManager.Singleton.AddResourceLocation("./CgResources/Terrain", "FileSystem", "General");
        }

        public override void CreateViewports()
        {
            base.CreateViewports();
            viewport.BackgroundColour = new ColourValue(0.1f, 0.3f, 0.6f, 0.0f);  /* Blue background */
        }
    }
}

