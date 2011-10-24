#region ANTIGUO
#if TODO
using System;
using System.Collections.Generic;
using System.IO;

namespace MogreGis
{
    /**
    * Pointer to an external piece of data or information.
    *
    * A Resource is anything data that exists externally to the core feature store
    * mechanism. External textures (skins), 3D models, spatial references, or rasters
    * are all examples of Resource types.
    *
    * Concrete classes will implement the Resource interface.
    */
    public class Resource : ObjectWithTags
    {

        /**
         * Gets the name of this resource.
         *
         * @return String
         */
        public string getName()
        {
            return name;
        }

        /**
         * Sets the name of this resource.
         *
         * @param name
         *      Name of the resource (for lookup)
         */
        public virtual void setName(string _name)
        {
            name = _name;
        }

        /**
         * Gets the primary URI of the Resource. This is the location of the external
         * object that the Resource references. (NOTE: You will usually want to call
         * getAbsoluteURI() instead of this method, sine this may return a relative
         * value.)
         *
         * @return Relative (to getBaseURI) or absolute URI
         */
        public virtual string getURI()
        {
            return uri;
        }

        /**
         * Sets the primary URI of the Resource. This is the locaiton of the external
         * object that the Resource references.
         *
         * @param uri
         *      Relative (to getBaseURI) or absolute URI of the external object
         */
        public virtual void setURI(string _uri)
        {
            this.uri = _uri;
            cached_abs_path = "";
        }

        public virtual string getAbsoluteURI()
        {
            if (cached_abs_path.Length == 0)
                this.cached_abs_path = Path.GetFullPath(Path.Combine(getBaseURI(), getURI()));
            return cached_abs_path;
        }


        /**
         * Sets the base URI path to which relative paths should be appended
         *
         * @param base_uri
         *      A URI prefix to which relative primary URIs shall be appended
         */
        public void setBaseURI(string _base_uri)
        {
            this.base_uri = _base_uri;
            cached_abs_path = "";
        }

        /**
         * Gets the base URI for relative paths.
         *
         * @return The base URI to which relative primary URIs shall be appended
         */
        public string getBaseURI()
        {
            return base_uri;
        }


        /**
         * Checks whether this is a one-time-only Resource.
         *
         * @return True if this is marked as a "single-use" resource; i.e. it is unique to
         *         a specific application within the build. The layer compiler will remove
         *         single-use resources from the ResourceLibrary once them are used. An
         *         example would be a rooftop-ortho texture, which is applied one time and
         *         never shared.
         */
        public bool isSingleUse()
        {
            return single_use;
        }

        /**
         * Sets the resource as being "single-use" or not. By default, resources are
         * not single-use; i.e. they are multi-use.
         */
        public void setSingleUse(bool value)
        {
            single_use = value;
        }


        public virtual void setProperty(Property prop)
        {
            //NOP
        }
        public virtual Properties getProperties()
        {
            return new Properties();
        }

        public virtual string getResourceType() { return getStaticResourceType(); }
        public static string getStaticResourceType() { return System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName; }
#if TODO
        public static ResourceFactory getResourceFactory() { return new ResourceFactoryImpl<Resource>(); }
#endif
        protected Resource()
        {
            //mutex = null;
            //owns_mutex = false;
            setSingleUse(false);
        }
        protected Resource(string _name)
        {
            //mutex = null;
            //owns_mutex = false;
            setName(_name);
            setSingleUse(false);
        }

#if TODO
        ReentrantMutex getMutex()
        {
            if (mutex == null)
            {
                mutex = new ReentrantMutex();
                owns_mutex = true;
            }
            return mutex;
        }

        private void setMutex(ReentrantMutex _m)
        {
            mutex = _m;
            owns_mutex = false;
        }
#endif

        private string name;
        private string uri;
        private string base_uri;
        private bool single_use;
        private string cached_abs_path;

#if TODO
        private ReentrantMutex mutex = null;
        private bool owns_mutex=false;
#endif
    }

    public class ResourceList : List<Resource> { }

    public class ResourceNames : HashSet<string> { }

    public abstract class ResourceFactory
    {
        public abstract Resource createResource();
    }

    public class ResourceFactoryImpl<T> : ResourceFactory where T : Resource, new()
    {
        public override Resource createResource() { return new T(); }
    }

    public class ResourceFactoryMap : Dictionary<string, ResourceFactory> { }

#if TODO
//#define OSGGIS_META_RESOURCE(name) \
    public: \
        virtual std::string getResourceType() { return getStaticResourceType(); } \
        static std::string getStaticResourceType() { return #name; } \
        static ResourceFactory* getResourceFactory() { return new ResourceFactoryImpl<name>(); }        

//#define OSGGIS_DEFINE_RESOURCE(name) \
    static bool _osggsis_dr = osgGIS::Registry::instance()->addResourceType( name::getStaticResourceType(), name::getResourceFactory() )

#endif
}
#endif
#endregion

using System;
using System.Collections.Generic;
using System.IO;

namespace MogreGis
{
    /**
    * Pointer to an external piece of data or information.
    *
    * A Resource is anything data that exists externally to the core feature store
    * mechanism. External textures (skins), 3D models, spatial references, or rasters
    * are all examples of Resource types.
    *
    * Concrete classes will implement the Resource interface.
    */

    public class Resource : ObjectWithTags
    {
        public string Name
        {
            /**
             * Gets the name of this resource.
             */
            get
            {
                return Name;
            }

            /**
             * Sets the name of this resource.
             */
            set
            {
                Name = value;
            }
        }

        public string Uri
        {
            /**
             * Gets the primary URI of the Resource. This is the location of the external
             * object that the Resource references. (NOTE: You will usually want to call 
             * getAbsoluteURI() instead of this method, sine this may return a relative
             * value.
             */
            get
            {
                return Uri;
            }

            /**
             * Setes the primary URI of the Resource. This is the location of the external
             * object that the REsource references.
             */
            set
            {
                Uri = value;
                cachedAbsPath = "";
            }
        }

        public string BaseUri
        {
            /**
             * Sets the base URI path to which relative paths should be appended
             */
            set
            {
                BaseUri = value;
                cachedAbsPath = "";
            }

            /**
             * Gets the base URI for relative paths.
             */
            get
            {
                return BaseUri;
            }
        }

        public bool SingleUse
        {
            /**
             * Checks whether this is a one-time-only Resource.
             * 
             * Is true if this is marked as a "single-use" resource; i.e. it is unique to a 
             * specific application within the build. Thelayer compiler will remove single-use
             * resources from the ResourceLibrary once them are used. An example would be a
             * rooftop-ortho texture, which is applied one time an never shared.
             */
            get
            {
                return SingleUse;
            }

            /**
             * Sets the resource as bing "single-use" or not. By default, resources are
             * not single-use; e.e.they are multi-use.
             */
            set
            {
                SingleUse = value;
            }
        }

        private string cachedAbsPath;

        private bool ownsMutex;

        public Object SincronizedFlag
        {
            get
            {
                if (SincronizedFlag == null)
                {
                    SincronizedFlag = new Object();
                    ownsMutex = true;
                }
                return SincronizedFlag;
            }

            set
            {
                SincronizedFlag = value;
                ownsMutex = false;
            }
        }

        private String getAbsoluteURI()
        {
            if (cachedAbsPath.Length == 0)
            {
                //hace falta PathUtils.getAbsPath (getBaseURI(), getURI())
            }
            return cachedAbsPath;
        }

        public virtual Properties getProperties () {
            return new Properties();
        }

        public virtual void setProperty (Property prop) {
            //NOP
        }

    }

    public class ResourceList : List<Resource> { }

    public class ResourceNames : HashSet<string> { }

    public abstract class ResourceFactory
    {
        public abstract Resource createResource();
    }

    public class ResourceFactoryImpl<T> : ResourceFactory where T : Resource, new()
    {
        public override Resource createResource() { return new T(); }
    }

    public class ResourceFactoryMap : Dictionary<string, ResourceFactory> { }
}

