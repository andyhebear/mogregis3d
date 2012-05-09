

using MogreGis;
using System;
using Sharp3D.Math.Core;
namespace MogreGis
{
    public interface IScriptEngine
    {
        void install(Script script);
        ScriptResult run(Script script);
        ScriptResult run(Script script, FilterEnv env);
        ScriptResult run(Script script, Feature feature, FilterEnv env);

        string Language { get; }
    }

#if !NUEVO
    public class ScriptResult
    {
        dynamic result;

        private dynamic Result
        {
            get { return result; }
            set { result = value; }
        }

        public ScriptResult(dynamic str)
        {
            result = str;
        }

        public override string ToString()
        {
            return result as string;
        }

          public string asString()
        {
            return result as string;
        }

        public float asFloat(float def)
        {
            return (float)result;
        }

        public float asFloat()
        {
            return (float)result;
        }

        public double asDouble(double def)
        {
            return (double)result;
        }

        public int asInt(int def)
        {
            return (int)result;
        }

        public int asInt()
        {
            return (int)result;
        }

        public bool asBool(bool def)
        {
            return (bool)result;
        }

        public Mogre.Vector3 asVec3()
        {
            if (result is Mogre.Vector3)
            {
                return (Mogre.Vector3)result;
            }
            String[] aux = (result as string).Split();
            float x;
            float y;
            float z;

            float.TryParse(aux[0],out x);
            float.TryParse(aux[1],out y);
            float.TryParse(aux[2],out z);

            Mogre.Vector3 vector3D = new Mogre.Vector3(x, y, z);
            return vector3D;
        }

        public Mogre.Vector4 asVec4()
        {
            String[] aux = (result as string).Split();
            float x;
            float y;
            float z;
            float t;

            float.TryParse(aux[0], out x);
            float.TryParse(aux[1], out y);
            float.TryParse(aux[2], out z);
            float.TryParse(aux[3], out t);

            Mogre.Vector4 vector4D = new Mogre.Vector4(x, y, z, t);
            return vector4D;
        }

        public Object asRef()
        {
            return result as Object;
        }

        public bool isValid ()
        {
            return (result as Object) != null;
        }
    }
}
#endif

#if ANTIGUO
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
#endif
