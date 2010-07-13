using System;
using System.Collections.Generic;

namespace MogreGis
{
    /**
     * Interface for executing dynamic codes scripts and returning the results.
     *
     * @see Script for more information on scripting
     */
    public abstract class ScriptEngine
    {

        /**
         * Installs a script into the global namespace. This is a good way to add
         * shared code or functions. Scripts that run via the run() method will be able
         * to access functions or variables installed using this method.
         *
         * @param script
         *      Script containing code to install
         */
        public abstract void install(Script script);

        /**
         * Runs a script and returns the result.
         *
         * @param script
         *      Script to execute
         * @return
         *      State and data resulting from Script execution
         */
        public abstract ScriptResult run(Script script);

        /**
         * Runs a script and returns the result.
         *
         * @param script
         *      Script to execute
         * @param env
         *      Filter environment/context to pass to the script code
         * @return
         *      State and data resulting from Script execution
         */
        public abstract ScriptResult run(Script script, FilterEnv env);

        /**
         * Runs a script and returns the result.
         *
         * @param script
         *      Script to execute
         * @param feature
         *      Feature to pass to the script code
         * @param env
         *      Filter environment/context to pass to the script code
         * @return
         *      State and data resulting from Script execution
         */
        public abstract ScriptResult run(Script script, Feature feature, FilterEnv env);


        protected ScriptEngine() { }
    }
}
