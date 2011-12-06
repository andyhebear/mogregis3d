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
        String sourceUri;
        String name;
        String workingDirectory;

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

        public string getBaseURI()
        {
            return System.IO.Path.GetFullPath(".");
        }

        public void setSourceURI(string source_uri)
        {
            sourceUri = source_uri;
        }

        public void setName(string p)
        {
            name = p;
        }

        public void setWorkingDirectory(string p)
        {
            workingDirectory = p;
        }
    }
}
