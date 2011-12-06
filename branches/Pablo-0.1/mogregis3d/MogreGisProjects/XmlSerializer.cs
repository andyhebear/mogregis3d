using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Globalization;
using MogreGis;

namespace osgGISProjects
{
    public class XmlSerializer
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


        public static bool writeProject(Project project, string uri)
        {
            throw new NotImplementedException();
#if TODO
	std::ofstream output;
	output.open( uri.c_str() );
	if ( output.is_open() )
	{
		XmlSerializer ser;
		Document* doc = ser.writeProject(project);
		if (doc) {
			XmlDocument *xmldoc = static_cast<XmlDocument *>(doc);
			xmldoc->store(output);
			return true;
		}
		else
		{
			osgGIS::notify(osg::WARN) << "unable to encode project" << std::endl;
		}
		output.close();
	}
	else
	{
		osgGIS::notify(osg::WARN) << "unable to open URI : " << uri << std::endl;
	}

    return false;
#endif
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

        public virtual bool store(XmlDocument doc, Stream outstream)
        {
            doc.Save(outstream);

            return true;
        }


        public virtual FilterGraph readFilterGraph(XmlDocument doc)
        {
            FilterGraph result = null;
            if (doc != null)
            {
                result = decodeFilterGraph(doc.GetElementById("graph"), null);
            }
            return result;
        }

        public virtual XmlDocument writeFilterGraph(FilterGraph graph)
        {
            XmlDocument doc = new XmlDocument();

            if (graph != null)
            {
                XmlElement graph_e = encodeFilterGraph(doc, graph);
                doc.AppendChild(graph_e);
            }

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

#if TODO_DANI
        public virtual XmlDocument writeProject(Project project)
        {
            XmlDocument doc = new XmlDocument();
            if (project != null)
            {
                doc.AppendChild(encodeProject(doc, project));
            }
            return doc;
        }


        static RuntimeMapLayer decodeRuntimeMapLayer(XmlElement e, Project proj)
        {
            RuntimeMapLayer layer = null;
            if (e != null)
            {
                layer = new RuntimeMapLayer();
                layer.setBuildLayer(proj.getLayer(e.GetAttribute("layer")));
                layer.setSearchLayer(proj.getLayer(e.GetAttribute("searchlayer")));
                if (e.GetAttribute("searchable") == "true")
                    layer.setSearchable(true);
                if (e.GetAttribute("visible") == "false")
                    layer.setVisible(false);
            }
            return layer;
        }

        static XmlElement encodeRuntimeMapLayer(XmlDocument doc, RuntimeMapLayer rmaplayer)
        {
            XmlElement e = null;
            if (rmaplayer != null)
            {
                e = doc.CreateElement("maplayer");
                e.SetAttribute("layer", rmaplayer.getBuildLayer().getName());
                e.SetAttribute("searchlayer", rmaplayer.getSearchLayer().getName());
                e.SetAttribute("visible", (rmaplayer.getVisible() ? "true" : "false"));
                e.SetAttribute("searchable", (rmaplayer.getSearchable() ? "true" : "false"));
            }
            return e;
        }
#endif
#if TODO_DANI //decodeRuntimeMap
        static RuntimeMap decodeRuntimeMap(XmlElement e, Project proj)
        {
            RuntimeMap map = null;
            if (e != null)
            {
                map = new RuntimeMap();
                map.setName(e.GetAttribute("name"));
                map.setTerrain(proj.getTerrain(e.GetAttribute("terrain")));

                XmlNodeList map_layers = e.GetElementsByTagName("maplayer");
                foreach (XmlNode i in map_layers)
                {
                    XmlElement e2 = (XmlElement)i;
                    RuntimeMapLayer map_layer = decodeRuntimeMapLayer(e2, proj);
                    if (map_layer != null)
                        map.getMapLayers().Add(map_layer);
                }
            }
            return map;
        }
#endif

        static XmlElement encodeRuntimeMap(XmlDocument doc, RuntimeMap rmap)
        {
#if TODO
            XmlElement e = null;
            if (rmap != null)
            {
                e = doc.CreateElement("map");
                e.SetAttribute("name", rmap.getName());
                if (rmap.getTerrain() != null) e.SetAttribute("terrain", rmap.getTerrain().getName());

                foreach (RuntimeMapLayer it in rmap.getMapLayers())
                {
                    e.AppendChild(encodeRuntimeMapLayer(doc, it));
                }
            }
            return e;
#endif
            throw new NotImplementedException();
        }

        static FilterGraph decodeFilterGraph(XmlElement e, Project proj)
        {
#if TODO_DANI
#endif
            FilterGraph graph = null;
            if (e != null)
            {
                string name = e.GetAttribute("name");
                //TODO: assert name
#if TODO_DANI
                string parent_name = e.GetAttribute("inherits");
                if (!string.IsNullOrEmpty(parent_name))
                {
                    FilterGraph parent_graph = proj.getFilterGraph(parent_name);
                    if (parent_graph == null)
                    {
                        //osgGIS.notify( osg.WARN )
                        //    << "Parent graph \"" << parent_name << "\" not found for graph \""
                        //    << name << "\"" << std.endl;
                    }
                    else
                    {
                        graph = (FilterGraph)parent_graph;
                        //TODO...
                    }
                }
                else
                {
#endif
                graph = new FilterGraph();
                graph.setName(name);

                XmlNodeList filter_els = e.GetElementsByTagName("filter");
                foreach (XmlNode i in filter_els)
                {
                    XmlElement f_e = (XmlElement)i;

                    string type = f_e.GetAttribute("type");

                    Filter f = MogreGis.Registry.instance().createFilterByType(type);

                    // try again with "Filter" suffix
                    if (f == null && !type.EndsWith("Filter", false, System.Globalization.CultureInfo.InvariantCulture))
                        f = MogreGis.Registry.instance().createFilterByType(type + "Filter");
                    //(f as BuildGeomFilter).setNameEntityINI ( f_e.GetAttribute("nameentityini"));
                    //(f as BuildGeomFilter).setNameEntities (f_e.GetAttribute("nameentities"));
                    if (f != null)
                    {
                        XmlNodeList prop_els = f_e.GetElementsByTagName("property");
                        foreach (XmlNode k in prop_els)
                        {
                            XmlElement k_e = (XmlElement)k;
                            string name_ = k_e.GetAttribute("name");
                            string value_ = k_e.GetAttribute("value");
                            f.setProperty(new Property(name_, value_));
                        }
                        graph.appendFilter(f);
                    }
                }
#if TODO_DANI
                }
#endif
            }

            return graph;
#if TODO_DANI
#endif
            throw new NotImplementedException();
        }


        static XmlElement encodeProperty(XmlDocument doc, Property property)
        {
            XmlElement e = doc.CreateElement("property");
            e.SetAttribute("name", property.getName());
            e.SetAttribute("value", property.getValue());
            return e;
        }

        static XmlElement encodeFilterGraph(XmlDocument doc, FilterGraph graph)
        {
#if TODO
            XmlElement graph_e = doc.CreateElement("graph");

            graph_e.SetAttribute("name", graph.getName());

            foreach (Filter f in graph.getFilters())
            {

                XmlElement filter_e = doc.CreateElement("filter");
                //XmlAttributeCollection attrs = new XmlAttributeCollection(filter_e);
                //attrs["type"] = f.getFilterType();

                XmlAttribute xmlAttrib = doc.CreateAttribute("type");
                xmlAttrib.Value = f.getFilterType();
                filter_e.Attributes.Append(xmlAttrib);

                Properties props = f.getProperties();
                foreach (Property i in props)
                {
                    filter_e.AppendChild(encodeProperty(doc, i));
                }

                graph_e.AppendChild(filter_e);
            }
            return graph_e;
#endif
            throw new NotImplementedException();
        }

        static Script decodeScript(XmlElement e, Project proj)
        {
            Script script = null;
            if (e != null)
            {
                script = new Script(e.GetAttribute("name"), e.GetAttribute("language"), e.InnerText);
            }
            return script;
        }
#if TODO
        static XmlElement encodeScript(XmlDocument doc, Script script)
        {
            XmlElement e = null;
            if (script != null)
            {
                e = doc.CreateElement("", "script", "");
                e.SetAttribute("name", script.getName());
                e.SetAttribute("language", script.getLanguage());
                e.AppendChild(doc.CreateTextNode(script.getCode()));
            }
            return e;
        }
#endif

        static void parseSRSResource(XmlElement e, SRSResource resource)
        {
            if (resource.getSRS() == null)
            {
                string wkt = e.InnerText;
                if (!string.IsNullOrEmpty(wkt))
                {
                    resource.setSRS(Registry.SRSFactory().createSRSfromWKT(wkt));
                }
            }
        }

        static SRSResource findSRSResource(ResourceList list, string name)
        {
            foreach (Resource i in list)
            {
                if (i != null && i.Name == name)
                    return (SRSResource)i;
            }
            return null;
        }
#if TODO
        static void parseRasterResource(XmlElement e, RasterResource resource)
        {
            XmlNodeList parts = e.GetElementsByTagName("uri");
            foreach (XmlNode i in parts)
            {
                resource.addPartURI(((XmlElement)i).InnerText);
            }
        }
#endif
        static Resource decodeResource(XmlElement e, Project proj)
        {
            SRSResource a = new SRSResource();
            Resource resource = null;
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
                        resource.setProperty(new Property(name, value));
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
        static XmlElement encodeURI(XmlDocument doc, string uri)
        {
            XmlElement e = doc.CreateElement("", "uri", "");
            e.AppendChild(doc.CreateTextNode(uri));
            return e;
        }

#if TODO_DANI
        static XmlElement encodeResource(XmlDocument doc, Resource resource)
        {
            XmlElement e = null;
            if (resource && resource.getResourceType() != FeatureLayerResource.getStaticResourceType())
            {
                e = doc.CreateElement("", "resource", "");
                e.SetAttribute("type", resource.getResourceType());
                e.SetAttribute("name", resource.getName());
                string tags;
                foreach (string it in resource.getTags())
                {
                    tags += it + " ";
                }
                if (!string.IsNullOrEmpty(tags))
                    e.SetAttribute("tags", tags);

                e.AppendChild(encodeURI(doc, resource.getURI()));

                foreach (Property it in resource.getProperties())
                {
                    e.AppendChild(encodeProperty(doc, it));
                }

                if (resource.getResourceType() == SRSResource.getStaticResourceType())
                {
                    SRSResource srs = (SRSResource)resource;
                    e.AppendChild(doc.CreateTextNode(srs.getSRS().getWKT()));
                }
            }
            return e;
        }
#endif

        static Source decodeSource(XmlElement e, Project proj, int pass)
        {
            Source source = null;
            if (e != null)
            {
                if (pass == 0)
                {
                    // first pass: create the new source record
                    source = new Source();
#if TODO_DANI
                    source.setBaseURI(proj.getBaseURI());
#endif
                    source.setName(e.GetAttribute("name"));
                    source.setType(e.GetAttribute("type") == "raster" ? Source.SourceType.TYPE_RASTER : Source.SourceType.TYPE_FEATURE);
                    source.setURI(e.GetElementsByTagName("uri")[0].InnerText);
#if TODO_DANI
                    source.setFilterGraph(proj.getFilterGraph(e.GetAttribute("graph")));
#endif
                }
                else
                {
                    // second pass: reference other sources
                    source = proj.getSource(e.GetAttribute("name"));
                    source.setParentSource(proj.getSource(e.GetAttribute("parent")));
                }
            }
            return source;
        }


#if TODO_DANI
        static XmlElement encodeSource(XmlDocument doc, Source source)
        {
            XmlElement e = null;
            if (source != null)
            {
                e = doc.CreateElement("source");
                e.SetAttribute("name", source.getName());
                e.SetAttribute("type", (source.getType() == Source.SourceType.TYPE_RASTER ? "raster" : "feature"));
                if (source.getFilterGraph() != null)
                    e.SetAttribute("graph", source.getFilterGraph().getName());
                if (source.getParentSource() != null)
                    e.SetAttribute("parent", source.getParentSource().getName());

                e.AppendChild(encodeURI(doc, source.getURI()));
            }
            return e;
        }
#endif
#if TODO_DANI //decodeTerrain
        static Terrain decodeTerrain(XmlElement e, Project proj)
        {
            Terrain terrain = null;
            if (e != null)
            {
                terrain = new Terrain();
                terrain.setBaseURI(proj.getBaseURI());
                terrain.setName(e.GetAttribute("name"));
                terrain.setURI(e.GetElementsByTagName("uri")[0].InnerText);

                SRSResource resource = findSRSResource(proj.getResources(), e.GetAttribute("srs"));
                if (resource != null)
                    terrain.setExplicitSRS(resource.getSRS());
            }
            return terrain;
        }
#endif

#if TODO_DANI //encodeTerrain
        static XmlElement encodeTerrain(XmlDocument doc, Terrain terrain)
        {
            XmlElement e = null;
            if (terrain != null)
            {
                e = doc.CreateElement("", "terrain", "");
                e.SetAttribute("name", terrain.getName());
                e.AppendChild(encodeURI(doc, terrain.getURI()));
            }
            return e;
        }
#endif

        static BuildLayerSlice decodeSlice(XmlElement e, Project proj)
        {
            BuildLayerSlice slice = null;
            if (e != null)
            {
                slice = new BuildLayerSlice();

                if (!string.IsNullOrEmpty(e.GetAttribute("min_range")))
                    slice.setMinRange(float.Parse(e.GetAttribute("min_range")));
                if (!string.IsNullOrEmpty(e.GetAttribute("max_range")))
                    slice.setMaxRange(float.Parse(e.GetAttribute("max_range")));

#if TODO_DANI
                // required filter graph:
                string graph = e.GetAttribute("graph");
                slice.setFilterGraph(proj.getFilterGraph(graph)); //TODO: warning?
#endif

                // optional source:
                slice.setSource(proj.getSource(e.GetAttribute("source")));

#if TODO_DANI
                // properties particular to this slice:
                XmlNodeList props = e.GetElementsByTagName("property");
                foreach (XmlNode i in props)
                {
                    XmlElement k_e = (XmlElement)i;
                    string name = k_e.GetAttribute("name");
                    string value = k_e.GetAttribute("value");
                    slice.getProperties().Add(new Property(name, value));
                }
#endif

                // now decode sub-slices:
                XmlNodeList slices = e.GetElementsByTagName("slice");
                foreach (XmlNode i in slices)
                {
                    BuildLayerSlice child = decodeSlice((XmlElement)i, proj);
                    if (child != null)
                        slice.getSubSlices().Add(child);
                }
            }
            return slice;
        }

#if TODO_DANI

        static XmlElement encodeSlice(XmlDocument doc, BuildLayerSlice slice)
        {
            XmlElement e = null;
            if (slice != null)
            {
                e = doc.CreateElement("slice");
                if (slice.getMinRange() > 0)
                {
                    Property p = new Property("ignore", slice.getMinRange());
                    e.SetAttribute("min_range", p.getValue());
                }
                if (slice.getMaxRange() < float.MaxValue)
                {
                    Property p = new Property("ignore", slice.getMaxRange());
                    e.SetAttribute("max_range", p.getValue());
                }
                e.SetAttribute("graph", slice.getFilterGraph().getName());

                if (slice.getSource() != null)
                    e.SetAttribute("source", slice.getSource().getName());

                foreach (Property it in slice.getProperties())
                {
                    e.AppendChild(encodeProperty(doc, it));
                }

                foreach (BuildLayerSlice it in slice.getSubSlices())
                {
                    e.AppendChild(encodeSlice(doc, it));
                }
            }
            return e;
        }
#endif

        static BuildLayer decodeLayer(XmlElement e, Project proj)
        {
            BuildLayer layer = null;
            if (e != null)
            {
                layer = new BuildLayer();

#if TODO_DANI
                layer.setBaseURI(proj.getBaseURI());
#endif

                layer.setName(e.GetAttribute("name"));

                string type = e.GetAttribute("type");
                if (type == "correlated")
                    layer.setType(BuildLayer.LayerType.TYPE_CORRELATED);
                else if (type == "gridded")
                    layer.setType(BuildLayer.LayerType.TYPE_GRIDDED);
                else if (type == "quadtree" || type == "new")
                    layer.setType(BuildLayer.LayerType.TYPE_QUADTREE);

                string source = e.GetAttribute("source");
                layer.setSource(proj.getSource(source));

                string terrain = e.GetAttribute("terrain");
                layer.setTerrain(proj.getTerrain(terrain));

                layer.setTargetPath(e.GetAttribute("target"));

                XmlNodeList slices = e.GetElementsByTagName("slice");
                foreach (XmlNode i in slices)
                {
                    BuildLayerSlice slice = decodeSlice((XmlElement)i, proj);
                    if (slice != null)
                        layer.getSlices().Add(slice);
                }

#if TODO_DANI
                XmlNodeList props = e.GetElementsByTagName("property");
                foreach (XmlNode i in props)
                {
                    XmlElement k_e = (XmlElement)i;
                    string name = k_e.GetAttribute("name");
                    string value = k_e.GetAttribute("value");
                    layer.getProperties().Add(new Property(name, value));
                }

                XmlNodeList env_props = e.GetElementsByTagName("env_property");
                foreach (XmlNode i in env_props)
                {
                    XmlElement k_e = (XmlElement)i;
                    string name = k_e.GetAttribute("name");
                    string value = k_e.GetAttribute("value");
                    layer.getEnvProperties().Add(new Property(name, value));
                }
#endif
            }
            return layer;
        }

#if TODO_DANI //encodeLayer
        static XmlElement encodeLayer(XmlDocument doc, BuildLayer layer)
        {
            XmlElement e = null;
            if (layer != null)
            {
                e = doc.CreateElement("layer");
                e.SetAttribute("name", layer.getName());

                switch (layer.getType())
                {
                    case BuildLayer.LayerType.TYPE_CORRELATED:
                        e.SetAttribute("type", "correlated");
                        break;
                    case BuildLayer.LayerType.TYPE_GRIDDED:
                        e.SetAttribute("type", "gridded");
                        break;
                    case BuildLayer.LayerType.TYPE_QUADTREE:
                        e.SetAttribute("type", "quadtree");
                        break;
                }
                e.SetAttribute("source", layer.getSource().getName());
                e.SetAttribute("terrain", layer.getTerrain().getName());
                e.SetAttribute("target", layer.getTargetPath());

                foreach (BuildLayerSlice it in layer.getSlices())
                {
                    e.AppendChild(encodeSlice(doc, it));
                }

                foreach (Property it in layer.getProperties())
                {
                    e.AppendChild(encodeProperty(doc, it));
                }

            }
            return e;
        }
#endif

#if TODO_DANI //decodeTarget
        static BuildTarget decodeTarget(XmlElement e, Project proj)
        {
            BuildTarget target = null;
            if (e != null)
            {
                target = new BuildTarget();
                target.setName(e.GetAttribute("name"));

                Terrain terrain = proj.getTerrain(e.GetAttribute("terrain"));
                target.setTerrain(terrain);

                XmlNodeList layers = e.GetElementsByTagName("layer");
                foreach (XmlNode i in layers)
                {
                    XmlElement elem = (XmlElement)i;
                    string layer_name = elem.InnerText;
                    if (!string.IsNullOrEmpty(layer_name))
                    {
                        BuildLayer layer = proj.getLayer(layer_name);
                        if (layer != null)
                            target.addLayer(layer);
                    }
                }
            }
            return target;
        }


        static XmlElement encodeTarget(XmlDocument doc, BuildTarget target, Project project)
        {
            XmlElement e = null;
            // don't output targets that are in fact layer targets
            if (target != null && project.getLayer(target.getName()) == null)
            {
                e = doc.CreateElement("target");
                e.SetAttribute("name", target.getName());

                if (target.getTerrain() != null)
                    e.SetAttribute("terrain", target.getTerrain().getName());

                foreach (BuildLayer it in target.getLayers())
                {
                    XmlElement layer = doc.CreateElement("layer");
                    layer.AppendChild(doc.CreateTextNode(it.getName()));
                    e.AppendChild(layer);
                }
            }
            return e;
        }
#endif
#if TODO_DANI //decodeInclude
        static Project decodeInclude(XmlElement e, Project proj)
        {
            if (e != null)
            {
                string uri = e.InnerText;
                if (!string.IsNullOrEmpty(uri))
                {
                    uri = PathUtils.getAbsPath(proj.getBaseURI(), uri);

                    Project include_proj = XmlSerializer.loadProject(uri);
                    if (include_proj != null)
                    {
                        proj.merge(include_proj);
                    }
                }
            }
            return proj;
        }
#endif


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

#if TODO_DANI //includes
                // includes
                XmlNodeList includes = e.GetElementsByTagName("include");
                foreach (XmlNode j in includes)
                {
                    decodeInclude((XmlElement)j, project);
                }
#endif

#if !TODO_DANI //scripts
                // scripts
                XmlNodeList scripts = e.GetElementsByTagName("script");
                foreach (XmlNode j in scripts)
                {
                    Script script = decodeScript((XmlElement)j, project);
                    if (script != null)
                        project.getScripts().Add(script);
                }
#endif

#if !TODO_DANI //resources

                // resources
                XmlNodeList resources = e.GetElementsByTagName("resource");
                foreach (XmlNode j in resources)
                {
                    Resource resource = decodeResource((XmlElement)j, project);
                    if (resource != null)
                        project.getResources().Add(resource);
                }

#endif

                // graphs
                XmlNodeList graphs = e.GetElementsByTagName("graph");
                foreach (XmlNode j in graphs)
                {
                    FilterGraph graph = decodeFilterGraph((XmlElement)j, project);
                    if (graph != null)
                        project.getFilterGraphs().Add(graph);
                }

#if TODO_DANI //terrains

                // terrains (depends on resources)
                XmlNodeList terrains = e.GetElementsByTagName("terrain");
                foreach (XmlNode j in terrains)
                {
                    Terrain terrain = decodeTerrain((XmlElement)j, project);
                    if (terrain != null)
                        project.getTerrains().Add(terrain);
                }

#endif

                // sources - 2 passes, since a source can reference another source
                XmlNodeList sources = e.GetElementsByTagName("source");
                foreach (XmlNode j in sources)
                {

                    // TODO Dani, meter esto en un try catch

                    Source source = decodeSource((XmlElement)j, project, 0);
                    if (source != null)
                    {
                        project.getSources().Add(source);

#if TODO_DANI
                        // also add each source as a feature layer resource
                        Resource resource = MogreGis.Registry.instance().createResourceByType("FeatureLayerResource");
                        resource.setBaseURI(project.getBaseURI());
                        resource.setURI(source.getURI());
                        resource.setName(source.getName());
                        project.getResources().Add(resource);
#endif
                    }
                }
                foreach (XmlNode j in sources)
                {
                    decodeSource((XmlElement)j, project, 1);
                }

                //#if TODO_DANI //layers

                // layers
                XmlNodeList layers = e.GetElementsByTagName("layer");
                foreach (XmlNode j in layers)
                {
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

                //#endif

#if TODO_DANI //targets

                // targets
                XmlNodeList targets = e.GetElementsByTagName("target");
                foreach (XmlNode j in targets)
                {
                    BuildTarget target = decodeTarget((XmlElement)j, project);
                    if (target != null)
                        project.getTargets().Add(target);
                }

#endif

#if TODO_DANI //maps

                // maps
                XmlNodeList maps = e.GetElementsByTagName("map");
                foreach (XmlNode j in maps)
                {
                    RuntimeMap map = decodeRuntimeMap((XmlElement)j, project);
                    if (map != null)
                        project.getMaps().Add(map);
                }

#endif

            }
            return project;
        }

#if TODO_DANI //encodeProject
        static XmlElement encodeProject(XmlDocument doc, Project project)
        {
            XmlElement e = doc.CreateElement("project");
            if (e != null)
            {
                e.SetAttribute("name", project.getName());
                e.SetAttribute("workdir", project.getWorkingDirectory());

                /*
                includes merges another project into this one, not possible to write it back
                // includes
                XmlNodeList includes = e.GetElementsByTagName( "include" );
                for( XmlNodeList.const_iterator j = includes.begin(); j != includes.end(); j++ )
                {
                    decodeInclude( XmlElement( j.get() ), project );
                }
                */

                foreach (Script it in project.getScripts())
                {
                    XmlElement sub_e = encodeScript(doc, it);
                    if (sub_e != null) e.AppendChild(sub_e);
                }

                foreach (Resource it in project.getResources())
                {
                    XmlElement sub_e = encodeResource(doc, it);
                    if (sub_e != null) e.AppendChild(sub_e);
                }

                foreach (FilterGraph it in project.getFilterGraphs())
                {
                    XmlElement sub_e = encodeFilterGraph(doc, it);
                    if (sub_e != null) e.AppendChild(sub_e);
                }

                foreach (Terrain it in project.getTerrains())
                {
                    XmlElement sub_e = encodeTerrain(doc, it);
                    if (sub_e != null) e.AppendChild(sub_e);
                }

                foreach (Source it in project.getSources())
                {
                    XmlElement sub_e = encodeSource(doc, it);
                    if (sub_e != null) e.AppendChild(sub_e);
                }

                foreach (BuildLayer it in project.getLayers())
                {
                    XmlElement sub_e = encodeLayer(doc, it);
                    if (sub_e != null) e.AppendChild(sub_e);
                }

                foreach (BuildTarget it in project.getTargets())
                {
                    XmlElement sub_e = encodeTarget(doc, it, project);
                    if (sub_e != null) e.AppendChild(sub_e);
                }

                foreach (RuntimeMap it in project.getMaps())
                {
                    XmlElement sub_e = encodeRuntimeMap(doc, it);
                    if (sub_e != null) e.AppendChild(sub_e);
                }

            }

            return e;
        }
#endif

    }
}