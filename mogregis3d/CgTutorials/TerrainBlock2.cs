using System;
using System.Collections.Generic;
using System.Text;

using Mogre.Helpers;

namespace Mogre.Demo.CgTutorials
{

    /// <summary>
    /// 
    /// </summary>
    class TerrainBlock2 : Mogre.Demo.ExampleApplication.Example
    {
        const int SIZE = 256 - 1;
        int blockSize = (SIZE + 1) / 4;

        const uint TransXYscaleXY = 3001;
        const uint ColorAndHeightScale = 3002;


        static void BuildPlaneMesh(string name, ushort pointsX, ushort pointsY)
        {
            MeshBuilderHelper mbh = new MeshBuilderHelper(name, "Terrrain", false, 0, (uint)(pointsY * pointsX));

            UInt32 offPos = mbh.AddElement(VertexElementType.VET_FLOAT3,
                                           VertexElementSemantic.VES_POSITION).Offset;

            mbh.CreateVertexBuffer(HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY);

            ///
            // This function uses one big zigzag triangle strip for the whole grid. 
            // And insert degenerated (invisible) triangles to join 2 rows. For instance:
            // 0  1  2  3
            // 4  5  6  7
            // 8  9  10 11
            // 12 13 14 15
            // 
            // The index buffer would look like :
            // 0, 4, 1, 5, 2, 6, 3, 7, 7, 7, 11, 6, 10, 5, 9, 4, 8, 8, 8, 12, 9, 13, 10, 14, 11, 15
            ///
            uint vertexindex = 0;
            for (int y = pointsY - 1; y >= 0; y--)
            {
                for (int x = 0; x <= pointsX - 1; x++)
                {
                    mbh.SetVertFloat(vertexindex, offPos, x, y, 0.0f);      //position
                    //mbh.SetVertFloat(vertexindex, offColor, 0, 0, 1);
                    vertexindex++;
                }
            }

            mbh.CreateIndexBufferForTriStrip((uint)(pointsX * 2 * (pointsY - 1) + pointsY - 2),
                                              HardwareIndexBuffer.IndexType.IT_16BIT,
                                              HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY);
            for (ushort y = 0; y < pointsY - 1; y++)
            {
                if (y % 2 == 0)
                {
                    for (int x = 0; x < pointsX; x++)
                    {
                        mbh.Index16bit(((ushort)(y * pointsX + x)));            //(x, y + 1, 0.0f);
                        mbh.Index16bit((ushort)(y * pointsX + x + pointsX));  //(x, y, 0.0f);
                    }
                    if (y != pointsY - 2)
                    {
                        mbh.Index16bit(((ushort)(y * pointsX + pointsX - 1)));         //(0, y+1, 0.0f);
                    }
                }
                else
                {
                    for (int x = pointsX - 1; x >= 0; x--)
                    {
                        mbh.Index16bit((ushort)(y * pointsX + x));           //(x, y + 1, 0.0f);
                        mbh.Index16bit((ushort)(y * pointsX + x + pointsX));  //(x, y, 0.0f);
                    }
                    if (y != pointsY - 2)
                    {
                        mbh.Index16bit((ushort)(y * pointsX + pointsX));
                    }
                }
            }
            
            MeshPtr m = mbh.Load("Terrain/Terrain_Material");
            m._setBounds(new AxisAlignedBox(0.0f, 0.0f, 0.0f, pointsX -1 , pointsY -1, 10.0f), false);
        }

        public void SetParameters(Entity ent, Vector4 translationScale, Vector4 color)
        {            
            ent.GetSubEntity(0).SetCustomParameter(TransXYscaleXY, translationScale);
            ent.GetSubEntity(0).SetCustomParameter(ColorAndHeightScale, color);
        }

        const int scale = 0;
        Vector4 greenBlock = new Vector4(0, 0.7f, 0, scale);
        Vector4 redBlock = new Vector4(0.7f, 0, 0, scale);

        SceneNode[] nodesL0 = new SceneNode[4];

        public override void CreateScene()
        {
            ushort points = (ushort)(blockSize + 1);
            BuildPlaneMesh("BasicTerrainBlock", points, points);

            for (int i = 0; i < nodesL0.Length; i++)
            {
                nodesL0[i] = base.sceneMgr.RootSceneNode.CreateChildSceneNode("TerrainBlockNodeL0_" + i);
            }

            Entity[] entitiesL0 = new Entity[nodesL0.Length];
            for (int i = 0; i < entitiesL0.Length; i++)
            {
                entitiesL0[i] = base.sceneMgr.CreateEntity("TerrainBlockObjectL0_" + i, "BasicTerrainBlock");
            }

            SetParameters(entitiesL0[0], new Vector4(0, 0, 1, 1), greenBlock);
            SetParameters(entitiesL0[1], new Vector4(0, 0, 1, 1), redBlock);
            SetParameters(entitiesL0[2], new Vector4(0, 0, 1, 1), redBlock);
            SetParameters(entitiesL0[3], new Vector4(0, 0, 1, 1), greenBlock);

            for (int i = 0; i < nodesL0.Length; i++)
            {
                nodesL0[i].AttachObject(entitiesL0[i]);
                nodesL0[i].ShowBoundingBox = true;
            }
            nodesL0[0].Position = new Vector3(- blockSize, 0, 0);
            nodesL0[1].Position = new Vector3(0, 0, 0);
            nodesL0[2].Position = new Vector3(-blockSize, -blockSize, 0);
            nodesL0[3].Position = new Vector3(0, -blockSize, 0);


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

