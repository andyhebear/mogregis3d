using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using MogreGis;
using System.Globalization;
using SharpMap.Data.Providers;

namespace SharpMapExample
{
    class XmlSerializer
    {
        public XmlSerializer() { }

        public static Project loadProject(string uri)
        {
            XmlSerializer ser = new XmlSerializer();
            XmlDocument doc = ser.load(uri);
            if (doc == null || !doc.HasChildNodes)
                return null; // todo: report error

            Project project = ser.readProject(doc);
            return project;
        }

        public virtual XmlDocument load(Stream instream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(instream);
            return doc;
        }

        public virtual XmlDocument load(string uri)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(uri);
            return doc;
        }

        public virtual Project readProject(XmlDocument doc)
        {
            Project result = null;
            if (doc != null)
            {
                result = decodeProject(doc.DocumentElement, doc.BaseURI);
            }
            return result;
        }

        static Project decodeProject(XmlElement e, string source_uri)
        {
            if (e == null || !e.Name.Equals("project"))
                throw new ApplicationException("First XML element must be <project/> tag");

            Project project = null;
            if (e != null)
            {
                project = new Project();
                project.setSourceURI(source_uri);
                project.setName(e.GetAttribute("name"));
                project.setWorkingDirectory(e.GetAttribute("workdir"));


                // resources
                XmlNodeList resources = e.GetElementsByTagName("resource");
                foreach (XmlNode j in resources)
                {
                    MogreGis.Resource resource = decodeResource((XmlElement)j, project);
                    if (resource != null)
                        project.getResources().Add(resource);
                }

                #region TODO_PH_FILTERGRAPHS
#if TODO_PH
                // graphs
                XmlNodeList graphs = e.GetElementsByTagName("graph");
                foreach (XmlNode j in graphs)
                {
                    FilterGraph graph = decodeFilterGraph((XmlElement)j, project);
                    if (graph != null)
                        project.getFilterGraphs().Add(graph);
                }
#endif
                #endregion

                // sources - 2 passes, since a source can reference another source
                XmlNodeList sources = e.GetElementsByTagName("source");
                foreach (XmlNode j in sources)
                {

                    // TODO Dani, meter esto en un try catch
                    
                    ShapeFile shapeFile = decodeShapeFile((XmlElement)j, project, 0);
                    if (shapeFile != null)
                    {
                        project.getShapeFiles().Add(shapeFile);
                    }
                }
                foreach (XmlNode j in sources)
                {
                    decodeShapeFile((XmlElement)j, project, 1);
                }

                #region TODO_PH_LAYERS
#if TODO_PH_LAYERS
                XmlNodeList layers = e.GetElementsByTagName("layer");
                foreach (XmlNode j in layers)
                {
                    SharpMap.Layers.VectorLayer l = new SharpMap.Layers.VectorLayer(
                    BuildLayer layer = decodeLayer((XmlElement)j, project);
                    if (layer != null)
                    {
                        project.getLayers().Add(layer);

                        // automatically add a target for this layer alone:
                        BuildTarget layer_target = new BuildTarget();
                        layer_target.setName(layer.getName());
                        layer_target.addLayer(layer);
                        project.getTargets().Add(layer_target);
                    }
                }
#endif
                #endregion

            }
            return project;
        }

        static MogreGis.Resource decodeResource(XmlElement e, Project proj)
        {
            //proj = new Project();
            MogreGis.SRSResource a = new SRSResource();
            MogreGis.Resource resource = null;
            if (e != null)
            {
                string type = e.GetAttribute("type");
                resource = MogreGis.Registry.instance().createResourceByType(type);

                // try again with "Resource" suffix
                if (resource == null && !type.EndsWith("Resource", false, CultureInfo.InvariantCulture))
                    resource = MogreGis.Registry.instance().createResourceByType(type + "Resource");

                if (resource != null)
                {
                    resource.BaseUri = proj.getBaseURI();
                    resource.Uri = e.InnerText;

                    resource.Name = e.GetAttribute("name");

                    string csv_tags = e.GetAttribute("tags");
                    if (!string.IsNullOrEmpty(csv_tags))
                    {
                        //std.istringstream iss(csv_tags);
                        //List<string> tokens = new List<string>((std.istream_iterator<string>(iss)), std.istream_iterator<string>());
                        string[] tokens = csv_tags.Split(',');
                        foreach (string i in tokens)
                            resource.addTag(i);
                    }

                    resource.addTag(e.GetAttribute("tags"));
                    XmlNodeList listuri = e.GetElementsByTagName("uri");
                    if (listuri.Count > 0)
                        resource.Uri = listuri[0].InnerText;


                    XmlNodeList prop_els = e.GetElementsByTagName("property");
                    foreach (XmlNode k in prop_els)
                    {
                        XmlElement k_e = (XmlElement)k;
                        string name = k_e.GetAttribute("name");
                        string value = k_e.GetAttribute("value");
                        //resource.setProperty(new Property(name, value));
                    }

                    if (resource != null && resource is SRSResource)
                    {
                        parseSRSResource(e, (SRSResource)resource);
                    }
#if TODO_PH
                    else if (resource != null && resource is RasterResource)
                    {
                        parseRasterResource(e, (RasterResource)resource);
                    }
#endif
                }
                else
                {
                    //TODO osgGIS.notify( osg.WARN ) << "Unknown resource type: " << type << std.endl;
                }
            }
            return resource;
        }

        static void parseSRSResource(XmlElement e, SRSResource resource)
        {
            if (resource.getSRS() == null)
            {
                string wkt = e.InnerText;
                if (!string.IsNullOrEmpty(wkt))
                {
                    SharpMapSpatialReferenceFactory shpsrsf = new SharpMapSpatialReferenceFactory();
                    SpatialReference srs = shpsrsf.createSRSfromWKT(wkt);
                    resource.setSRS(srs);
                    //resource.setSRS(Registry.SRSFactory().createSRSfromWKT(wkt));
                }
            }
        }

        static ShapeFile decodeShapeFile(XmlElement e, Project proj, int pass)
        {
            ShapeFile shapeFile = null;
            if (e != null)
            {
                if (pass == 0)
                {
                    // first pass: create the new source record
                    //source = new Source();
                    //source.setName(e.GetAttribute("name"));
                    //source.setType(e.GetAttribute("type") == "raster" ? Source.SourceType.TYPE_RASTER : Source.SourceType.TYPE_FEATURE);
                    //source.setURI(e.GetElementsByTagName("uri")[0].InnerText);
                    if (e.GetAttribute("provider") == "ShapeFile")
                    {
                        shapeFile = new ShapeFile(e.GetElementsByTagName("uri")[0].InnerText);
                    }
                }
                else
                {
                    // second pass: reference other sources
                    //source = proj.getSource(e.GetAttribute("name"));
                    //source.setParentSource(proj.getSource(e.GetAttribute("parent")));
                }
            }
            return shapeFile;
        }

    }
}
