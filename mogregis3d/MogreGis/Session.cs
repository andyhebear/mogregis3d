
#region ANTIGUO
#if ANTIGUO
using System;
using System.Collections.Generic;

namespace MogreGis
{
    /**
    * A state object that exists throughout the life of one or more related compilations.
    *
    * A Session holds shared, re-usable data elements (like Scripts and Resources) that can
    * be accessed at compile time by Filters. 
    *
    * Whereas a FilterEnv exists thoughout the life of a single compilation (a single
    * FilterGraph/FilterEnv), a Session exists one level above this and governs any number of
    * "related" compilations (e.g., the compilation of many grid cells comprising a single
    * feature layer).
    *
    * A Session may be shared across compilations that exist in separate threads -- therefore
    * access to the session's resources is protected by mutex.
    */
    public class Session
    {

#if TODO
        /**
         * Constructs a new session.
         */
        public Session();

        /**
         * Creates a shallow clone of this session that references the same scripts
         * and resource library (but not the transient elements like the properties
         * and resource-use information).
         *
         * @return A derived Session. Caller is responsible for deleting the return object.
         */
        public Session derive();
#endif
        /**
         * Creates a new scripting engine that incorporates any scripts stored in this
         * session.
         *
         * @return A script evaluation engine. Caller is responsible for deleting the
         *         return object.
         */
        public ScriptEngine createScriptEngine()
        {
            throw new NotImplementedException();
        }

#if TODO
        /**
         * Creates a new filter environment context that will operate under this session.
         *
         * @return A Filter context. Caller is responsible for deleting the return object.
         */
        public FilterEnv createFilterEnv();

        /**
         * Adds a Script to this session. The script will be available in ScriptEngine's created
         * by calling createScriptEngine() (but NOT to a ScriptEngine created before adding the
         * new Script).
         *
         * @param script
         *      Script to install
         */
        public void addScript(Script script);

        /**
         * Gets a read-only reference to the list of scripts that have been added to this
         * session.
         *
         * @return A read-only list of scripts
         */
        public ScriptList getScripts();

        /**
         * Gets a read-only reference to the resource library that manages shared resources
         * for this session.
         *
         * @return Read-only resource library reference
         */
        public ResourceLibrary getResources();

        /**
         * Stores an indicator that the specified resource was used during the course of
         * this Session.
         *
         * @param resource
         *      Resource to mark as "used"
         */
        public void markResourceUsed(Resource resource);

        /**
         * Gets a collection of the resources that have been marked as "used" in this
         * session (by calling markResourceUsed).
         *
         * @param reset
         *      Whether to clear the "used" list after returning from this method
         * @return
         *      List of resources marked as "used"
         */
        public ResourceList getResourcesUsed()
        {
            return getResourcesUsed(false);
        }
        public ResourceList getResourcesUsed(bool reset);
#endif

#if TODO
        /**
         * Accesses the session-wide mutex. Filter can use this to perform
         * session-thread-safe operations (like opening or writing to a shared
         * file, for example).
         *
         * Use this sparingly or risk hurting performance!
         *
         * @return A mutex
         */
        public ReentrantMutex getSessionMutex();
#endif
#if TODO
        /**
         * Gets the session-level user data.
         */
        public virtual Property getProperty(string name);

        /**
         * Sets a session-level user data property.
         */
        public virtual void setProperty(Property prop);

        private ScriptList scripts;
        private ResourceLibrary resources;
#endif
#if TODO
        private ReentrantMutex session_mtx;
#endif
        

        private Properties properties;
        private ResourceList resources_used;
        private bool localize_resources;
    }
}
#endif
#endregion

using System;
using System.Collections.Generic;

namespace MogreGis
{
    /**
    * A state object that exists throughout the life of one or more related compilations.
    *
    * A Session holds shared, re-usable data elements (like Scripts and Resources) that can
    * be accessed at compile time by Filters. 
    *
    * Whereas a FilterEnv exists thoughout the life of a single compilation (a single
    * FilterGraph/FilterEnv), a Session exists one level above this and governs any number of
    * "related" compilations (e.g., the compilation of many grid cells comprising a single
    * feature layer).
    *
    * A Session may be shared across compilations that exist in separate threads -- therefore
    * access to the session's resources is protected by mutex.
    */
    public class Session
    {
#if TODO_PH
        public ScriptList Scripts
        {
            /**
             * Gets a read-only reference to the list of scripts that have been aded to this
             * session.
             */
            get
            {
                throw new NotImplementedException();
            }
        }
#endif

        private ResourceLibrary resources;
        public ResourceLibrary Resources
        {
            /**
             * Gets a read-only reference to the resource library that manaes shared resources
             * for this session.
             */
            get
            {
                return resources;
            }
        }

        public Session()
        {
            resources = new ResourceLibrary();
        }
#if TODO_PH
        private Properties properties;

        private ResourceList ResourcesUsed;

        private bool LocalizeResources;

        public Property getProperty(string name);

        public void setProperty(Property prop);

        public Object sincronizedFlag{get; set;}

        /**
         * Constructs a new session
         */
        public Session()
        {
            throw new NotImplementedException();
            sincronizedFlag = new Object();
            //Resources = new ResourceLibrary(sincronizedFlag);
        }

        #region METODOS
        /**
         * Creates a shallow clone of this session that references the same scripts
         * and resource library (but not the trasient elements like the properties 
         * and resource-use information).
         */
        public Session derive();//hace falta el mutex recursivo

        /**
         * Creates a new scriptin engine that incorporates any scrips stored in this
         * session.
         * 
         * @return A script evalution engine. Caller is responsible for deleting the
         *          return object.
         */
        public ScriptEngine createScriptEngine();//hace falta el mutex recursivo

        /**
         * Creates a new filter environment context that will operate under this session.
         * 
         * @return A filter context. Caller is responsible for deleting the return object.
         */
        public FilterEnv createFilterEnv();

        /**
         * Adds a Script to this session. The script will be available in ScriptEngine's created
         * by calling createScriptEngine() ( but NOT to a ScriptEngine created before added the
         * new Script).
         * 
         * @param script
         *          Script to install
         */
        public void addScript(Script script);

        /**
         * Stores an indicator that the specified resource was used during the course of 
         * this Session.
         * 
         * @param resource
         *          Resource to mark as "used"
         */
        public void markResourceUsed(Resource resource) { }

        /**
         * Gets a collection of the resorces that have been marked as "used" in  this
         * session (by calling markResourceUsed).
         * 
         * @param reset 
         *          Whether to clear the "used" list after returning from this method.
         * @return
         *          List of resorces marked as "used"
         */
        public ResourceList getResourceUsed(bool reset = false) 
        {
            throw new NotImplementedException();
        }

        /**
         * Accesses the session-wide mutex. Filter can use this to perform
         * session-thread-safe operations (like opening or writing to a shared 
         * file, for example).
         *
         * Use this sparingly or risk hurting performance!
         * 
         * @return A mutex
         */
        public Object getSessionMutex()
        {
            return sincronizedFlag;
        }
        #endregion
        
#endif
    }
}