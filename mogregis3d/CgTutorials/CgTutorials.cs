using System;
using System.Collections.Generic;
using System.Text;

using Mogre.Demo.ExampleApplication;
namespace Mogre.Demo.CgTutorials
{
    class Program
    {

        static void Main(string[] args)
        {
            Dictionary<string, Example> tutorials = new Dictionary<string, Example>();

            tutorials.Add("NoVertex", new T00VertexProgram()); //Simple triangle with defatult material
            tutorials.Add("C2E1", new C2E1Tutorial());   // 01_vertex_program
            tutorials.Add("C2E2", new C2E2Tutorial());   // 02_vertex_and_fragment_program
            tutorials.Add("C3E1", new C3E1Tutorial());   // 03_uniform_parameter
            tutorials.Add("C3E2", new C3E2Tutorial());   // 04_varying_parameter
            tutorials.Add("C3E3", new C3E3Tutorial());   // 05_texture_sampling
            tutorials.Add("C3E4", new C3E4Tutorial());   // 06_vertex_twisting
            tutorials.Add("C3E5", new C3E5Tutorial());   // 07_two_texture_accesses
            tutorials.Add("C3E5v2", new C3E5Tutorialv2());   // 07_two_texture_accesses
            tutorials.Add("C4E1", new C4E1Tutorial());   // 08_vertex_transform
            tutorials.Add("C5E1", new C5E1Tutorial());   // 09_vertex_lighting
            tutorials.Add("C5E2", new C5E2Tutorial());   // 10_fragment_lighting

            tutorials.Add("TerrainBlock1", new TerrainBlock1());   // Terrain block using ManualObject
            tutorials.Add("TerrainBlock2", new TerrainBlock2());   // Terrain block using Mesh

            tutorials.Add("RenderToTexture", new RenderToTextureTutorial());   // Render to Texture

            tutorials.Add("Polygon", new Mogre.Demo.PolygonExample.PolygonExample1());   // Polygon tesselation example
            tutorials.Add("Lines3Dv1", new Mogre.Demo.Primitives.Lines3DExample1()); // Simple lines3d plotting
            tutorials.Add("Lines3Dv2", new Mogre.Demo.Primitives.Lines3DExample2()); // lines3d plotting with a material
            tutorials.Add("Lines3Dv3", new Mogre.Demo.Primitives.Lines3DExample3()); // An example using NTS's operation Buffer

            //tutorials.Add("Scenicv1", new Mogre.Demo.Primitives.ScenicTestExample1()); // Simple lines3d plotting
            tutorials.Add("TextExample", new TextExample());

<<<<<<< .mine
            string tutorialToRun = "TextExample";
=======
            string tutorialToRun = "C2E1";
>>>>>>> .r58

            try
            {
                Mogre.Demo.ExampleApplication.Example app = tutorials[tutorialToRun];
                app.Go();
            }
            catch
            {
                // Check if it's an Ogre Exception
                if (Mogre.OgreException.IsThrown)
                    Mogre.Demo.ExampleApplication.Example.ShowOgreException();
                else
                    throw;
            }
        }
    }
}