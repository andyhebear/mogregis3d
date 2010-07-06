using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;

namespace MogreGis
{
    /**
     * A Resource that references an external 3D model.
     */
    public class ModelResource : Resource
    {
        //TODO 3OSGGIS_META_RESOURCE(ModelResource);


        /**
         * Creates a new, empty model resource.
         */
        public ModelResource()
        {
            init();
        }

        /**
         * Creates a new names model resource.
         */
        public ModelResource(string name)
            : base(name)
        {
            init();
        }


        /**
         * Loads the model from its URI and returns it.
         *
         * @return Root node of the model. The caller is responsible for
         *         deleting the returned object.
         */
        public Node createNode()
        {
            Node node = osgDB.readNodeFile(getAbsoluteURI());
            return node;
        }

        /**
         * Creates an returns a new osg::ProxyNode that references the
         * URI of this model resource.
         *
         * @return A new osg::ProxyNode. The caller is responsible for
         *         deleting the returned object.
         */
        public Node createProxyNode()
        {
            osg.ProxyNode proxy = new osg.ProxyNode();
            proxy.setDataVariance(osg.Object.STATIC);
            proxy.setFileName(0, getAbsoluteURI());
            return proxy;

            //osg::PagedLOD* plod = new osg::PagedLOD();
            //plod->addChild( dummy, 0.0f, 15000.0f, getAbsoluteURI() );
            //return plod;
        }

        public virtual void setProperty(Property prop)
        {
            if (prop.getName() == "model_path" || prop.getName() == "path")
                setURI(prop.getValue()); // backwards compat - use <uri>
            else
                base.setProperty(prop);
        }
        public virtual Properties getProperties()
        {
            Properties props = base.getProperties();
            //props.push_back( Property( "model_path", getModelPath() ) );
            return props;
        }

        private void init()
        {
            //nop
        }
    }

    public class ModelResources : List<ModelResource> { }

    /**
     * Structure used to query the resource library for model resources.
     */
    public class ModelResourceQuery
    {

        /**
         * Creates a new model query structure.
         */
        public ModelResourceQuery()
        {
            //nop
        }

        /** 
         * Adds a tag to include in the search.
         */
        public void addTag(string tag)
        {
            tags.Add(tag);
        }

        /**
         * Gets the collection of tags to use in the model search.
         */
        public TagList getTags()
        {
            return tags;
        }

        public string getHashCode()
        {
            //TODO
            return "";
        }


        private TagList tags;
        private string hash_code;
    }
}
