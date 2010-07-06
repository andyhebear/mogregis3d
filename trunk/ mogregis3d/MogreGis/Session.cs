using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
