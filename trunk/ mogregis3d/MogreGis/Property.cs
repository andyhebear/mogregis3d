using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sharp3D.Math.Core;

namespace MogreGis
{

    /**
     * A type-morphing name/value pair.
     *
     * A Property is a name/value pair that will accept various primitives like strings,
     * integers, floats, doubles, and booleans for the value. You can then read the value
     * back as any of the above types as well and the object will give you the best possible
     * representation available.
     *
     * A Property can also hold various non-primitive values, like osg::Vec2, osg::Vec3, osg::Vec4,
     * osg::Matrix, and a general osg::Referenced pointer.
     */
    public class Property
    {

        /**
         * Constructs a new empty property.
         */
        public Property()
        {
            valid = false;
        }

        /**
         * Constructs a new property.
         */
        public Property(string _name, string _value)
        {
            name = normalize(_name);
            value = _value;
            valid = true;
        }

        /**
         * Constructs a new property.
         */
        public Property(string _name, int _value)
        {
            name = normalize(_name);
            value = _value.ToString();
            valid = true;
        }

        /**
         * Constructs a new property.
         */
        public Property(string _name, float _value)
        {
            name = normalize(_name);
            value = _value.ToString();
            valid = true;
        }

        /**
         * Constructs a new property.
         */
        public Property(string _name, double _value)
        {
            name = normalize(_name);
            value = _value.ToString();
            valid = true;
        }


        /**
         * Constructs a new property.
         */
        public Property(string _name, bool _value)
        {
            name = normalize(_name);
            value = _value.ToString();
            valid = true;
        }


        /**
         * Constructs a new property.
         */
        public Property(string _name, Vector2D _value)
        {
            name = normalize(_name);
            value = _value.ToString();
            valid = true;
        }


        /**
         * Constructs a new property.
         */
        public Property(string _name, Vector3D _value)
        {
            name = normalize(_name);
            value = _value.ToString();
            valid = true;
        }


        /**
         * Constructs a new property.
         */
        public Property(string _name, Vector4D _value)
        {
            name = normalize(_name);
            value = _value.ToString();
            valid = true;
        }


        /**
         * Constructs a new property.
         */
        public Property(string _name, Matrix3D _value)
        {
            name = normalize(_name);
            value = _value.ToString();
            valid = true;
        }


        /**
         * Constructs a new property.
         */
        public Property(string _name, Object _value)
        {
            name = normalize(_name);
            value = _value.ToString();
            valid = true;
            ref_value = _value;
        }


        /**
         * Gets the property's name.
         */
        public string getName()
        {
            return name;
        }

        /**
         * Gets the property's value as a string.
         */
        public string getValue()
        {
            return value;
        }

        /**
         * Gets the property's value as an integer (with fallback)
         */
        public int getIntValue(int def)
        {
            return value.Length > 0 ? Int32.Parse(value) : def;
        }

        /**
         * Gets the property's value as a float (with fallback)
         */
        public float getFloatValue(float def)
        {
            return value.Length > 0 ? float.Parse(value) : def;
        }


        /**
         * Gets the property's value as a double (with fallback)
         */
        public double getDoubleValue(double def)
        {
            return value.Length > 0 ? double.Parse(value) : def;
        }

        /**
         * Gets the property's value as a boolean (with fallback)
         */
        public bool getBoolValue(bool def)
        {
            return value.Length > 0 ? bool.Parse(value) : def;
        }

        /**
         * Gets the property's value as an osg::Vec2
         */
        public Vector2D getVec2Value()
        {
            throw new NotImplementedException();
        }

        /**
         * Gets the property's value as an osg::Vec3
         */
        public Vector3D getVec3Value()
        {
            throw new NotImplementedException();
        }

        /**
         * Gets the property's value as an osg::Vec4
         */
        public Vector4D getVec4Value()
        {
            throw new NotImplementedException();
        }

        /**
         * Gets the property's value as an osg::Matrix
         */
        public Matrix3D getMatrixValue()
        {
            throw new NotImplementedException();
        }

        /**
         * Gets the property's value as an osg::Referenced*
         */
        public Object getRefValue()
        {
            return ref_value;
        }

        /**
         * Gets true if the property holds a valid value.
         */
        public bool isValid()
        {
            return valid;
        }

        /**
         * Gets the property's value as a string
         */
        public string asString()
        {
            return value;
        }

        /**
         * Gets the property's value as a double
         */
        public double asDouble()
        {
            return getDoubleValue(0.0);
        }

        /**
         * Gets the property's value as an integer
         */
        public int asInt()
        {
            return getIntValue(0);
        }

        /**
         * Gets the property's value as a boolean
         */
        public bool asBool()
        {
            return getBoolValue(false);
        }

        public static Property invalid()
        {
            return new Property();
        }

        internal static string normalize(string input)
        {
            return input.Replace('-', '_');
        }

        private string name;
        private string value;
        private Object ref_value;
        private bool valid;
        private const int PRECISION = 64;
    }

    /**
     * A collection of Property objects.
     */
    public class Properties : List<Property>
    {

        public bool exists(string key)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    return true;
                }
            }
            return false;
        }

        public int getIntValue(string key, int def)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    return i.getIntValue(def);
                }
            }
            return def;
        }

        public float getFloatValue(string key, float def)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    return i.getFloatValue(def);
                }
            }
            return def;
        }

        public double getDoubleValue(string key, double def)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    return i.getDoubleValue(def);
                }
            }
            return def;
        }

        public bool getBoolValue(string key, bool def)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    return i.getBoolValue(def);
                }
            }
            return def;
        }

        public Vector2D getVec2Value(string key)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    return i.getVec2Value();
                }
            }
            return new Vector2D(0, 0);
        }

        public Vector3D getVec3Value(string key)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    return i.getVec3Value();
                }
            }
            return new Vector3D(0, 0, 0);
        }

        public Vector4D getVec4Value(string key)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    return i.getVec4Value();
                }
            }
            return new Vector4D(0, 0, 0, 1);
        }

        public string getValue(string key, string def)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    return i.getValue();
                }
            }
            return def;
        }

        public Object getRefValue(string key)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    return i.getRefValue();
                }
            }
            return null;
        }

        public void remove(string key)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    this.Remove(i);
                    return;
                }
            }
        }

        public Property get(string key)
        {
            string nkey = Property.normalize(key);
            foreach (Property i in this)
            {
                if (i.getName() == nkey)
                {
                    return i;
                }
            }
            return Property.invalid();
        }

        public void set(Property prop)
        {
            remove(prop.getName());
            Add(prop);
        }
    }

    public interface IObjectWithProperties
    {

        void setProperty(Property prop);
        Properties getProperties();
    }

}
