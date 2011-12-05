using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpMap.Data.Providers;


namespace SharpMapExample
{
    public class Project
    {
        List <ShapeFile> shapeFiles;
        List<MogreGis.Resource> resources;

        public Project()
        {
            shapeFiles = new List<ShapeFile>();
            resources = new List<MogreGis.Resource>();
        }

        public List<ShapeFile> getShapeFiles()
        {
            return shapeFiles;
        }



        public List<MogreGis.Resource> getResources()
        {
            return resources;
        }
    }
}
