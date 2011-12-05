using System;
using System.Collections.Generic;


using Sharp3D.Math.Core;

namespace MogreGis
{
    /**
     * Code that the scripting engine can execute at compile-time.
     *
     * A Script is a piece of dynamically executed source code that you can embed in the
     * feature compilation process. Many filters accept Scripts as property values. A Script
     * has access to feature attributes and other data that is only available at compile-time.
     * This awards a great deal of flexibility to the build process.
     */
    public class Script
    {


        /**
         * Constructs a new, empty script.
         */
        public Script()
        {
            //NOP
        }


        /**
         * Constructs a new script with code in the default language.
         *
         * @param code
         *      Source code/expression to be executed
         */
        public Script(string _code)
        {
            setCode(_code);
        }


        /**
         * Constructs a new script with code in the specified language.
         *
         * @param name
         *      Name of the script
         * @param language
         *      Language in which the script is written (e.g., "lua")
         * @param code
         *      Source code/expression to be executed
         */
        public Script(string _name, string _language, string _code)
        {
            setName(_name);
            setLanguage(_language);
            setCode(_code);
        }


        /**
         * Sets the name of this script.
         *
         * @param name
         *      Name of the script
         */
        public void setName(string _name)
        {
            name = _name;
        }


        /**
         * Gets the name of this script.
         *
         * @return Name of the script
         */
        public string getName()
        {
            return name;
        }

        /** 
         * Sets the scripting language used by the code in this script.
         *
         * @param language
         *      Language in which the script is written (e.g., "lua")
         */
        public void setLanguage(string _language)
        {
            language = _language;
        }

        /**
         * Gets the scripting language used by this script.
         *
         * @return Lanuage in which the script is written (e.g., "lua")
         */
        public string getLanguage()
        {
            return language;
        }

        /**
         * Sets the script code to execute.
         *
         * @param code
         *      Source code/expression to execute
         */
        public void setCode(string _code)
        {
            code = _code;
        }

        /**
         * Gets the source code to execute.
         *
         * @return Source code/expression
         */
        public string getCode()
        {
            return code;
        }


        private string name;
        private string language;
        private string code;
    }

    public class ScriptList : List<Script> { }
    public class ScriptsByName : Dictionary<string, Script> { }


    /**
     * Status and return data resulting from the execution of a Script.
     */
    public class ScriptResult
    {

        /**
         * The data type of the return value of the Script.
         */
        public enum ReturnType
        {
            TYPE_BOOL,
            TYPE_INT,
            TYPE_DOUBLE,
            TYPE_STRING,
            TYPE_VEC3,
            TYPE_VEC4,
            TYPE_REF
        }


        public static ScriptResult Error(string _msg)
        {
            return new ScriptResult(false, _msg);
        }

        public ScriptResult()
        {
            valid = false;
        }

        public ScriptResult(string val)
        {
            prop = new Property("", val);
            valid = true;
        }

        public ScriptResult(float val)
        {
            prop = new Property("", val);
            valid = true;
        }

        public ScriptResult(double val)
        {
            prop = new Property("", val);
            valid = true;
        }

        public ScriptResult(int val)
        {
            prop = new Property("", val);
            valid = true;
        }

        public ScriptResult(bool val)
        {
            prop = new Property("", val);
            valid = true;
        }

        public ScriptResult(Object refer)
        {
            prop = new Property("", refer);
            valid = true;
        }

        public bool isValid()
        {
            return valid;
        }

        public string asString()
        {
            return prop.getValue();
        }

        public float asFloat(float def)
        {
            return prop.getFloatValue(def);
        }

        public double asDouble(double def)
        {
            return prop.getDoubleValue(def);
        }

        public int asInt(int def)
        {
            return prop.getIntValue(def);
        }

        public bool asBool(bool def)
        {
            return prop.getBoolValue(def);
        }

        public Vector3D asVec3()
        {
            return prop.getVec3Value();
        }

        public Vector4D asVec4()
        {
            return prop.getVec4Value();
        }

        public Object asRef()
        {
            return prop.getRefValue();
        }


        private ScriptResult(bool r1, string _msg)
        {
            valid = false;
            prop = new Property("", _msg);
        }

        private ReturnType type;
        private bool valid;
        private Property prop;
    }
}
