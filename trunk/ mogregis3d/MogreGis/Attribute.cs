using System;
using System.Collections.Generic;
using System.Linq;

namespace MogreGis
{

    /**
     * A single name/value pair, representing a GIS feature attribute record.
     */
    public class Attribute
    {
        public enum AttrType
        {
            TYPE_UNSPECIFIED,
            TYPE_STRING,
            TYPE_INT,
            TYPE_DOUBLE,
            TYPE_BOOL
        }

        /**
         * Constructs an "invalid" attribute that can be used to convey an
         * error status for a method returning type Attribute.
         */
        public Attribute()
        {
            valid = false;
        }

        /**
         * Constructs a new string attribute.
         */
        public Attribute(string _key, string _value)
        {
            this.key = _key.ToLowerInvariant();
            this.string_value = _value;
            this.type = AttrType.TYPE_STRING;
            this.valid = true;
        }


        /**
         * Constructs a new integer attribute.
         */
        public Attribute(string _key, int _value)
        {
            this.key = _key.ToLowerInvariant();
            this.int_value = _value;
            this.type = AttrType.TYPE_INT;
            this.valid = true;
        }


        /**
         * Constructs a new double attribute.
         */
        public Attribute(string _key, double _value)
        {
            this.key = _key.ToLowerInvariant();
            this.double_value = _value;
            this.type = AttrType.TYPE_DOUBLE;
            this.valid = true;
        }


        /**
         * Constructs a new boolean attribute.
         */
        public Attribute(string _key, bool _value)
        {
            this.key = _key.ToLowerInvariant();
            this.int_value = (_value ? 1 : 0);
            this.type = AttrType.TYPE_BOOL;
            this.valid = true;
        }


        /**
         * Gets the key (i.e. name) of this attribute.
         */
        public string getKey()
        {
            return key;
        }

        /**
         * Checks whether this attribute represents a valid name/value pair.
         */
        public bool isValid()
        {
            return valid;
        }

        /**
         * Gets the data type of this attribute's value.
         */
        public AttrType getType()
        {
            return type;
        }

        /**
         * Gets the value of this attribute as a string.
         */
        public string asString()
        {
            if (type != AttrType.TYPE_STRING && String.IsNullOrEmpty(string_value))
            {
                if (type == AttrType.TYPE_INT)
                    string_value = int_value.ToString();
                else if (type == AttrType.TYPE_DOUBLE)
                    string_value = double_value.ToString();
            }
            return string_value;
        }


        /**
         * Gets the value of this attribute as an integer.
         */
        public int asInt()
        {
            if (type == AttrType.TYPE_INT)
                return int_value;
            else if (type == AttrType.TYPE_DOUBLE)
                return (int)double_value;
            else if (type == AttrType.TYPE_BOOL)
                return int_value;
            else
                return Int32.Parse(this.string_value);
        }

        /**
         * Gets the value of this attribute as a double.
         */
        public double asDouble()
        {
            if (type == AttrType.TYPE_DOUBLE)
                return double_value;
            else if (type == AttrType.TYPE_INT)
                return (double)int_value;
            else if (type == AttrType.TYPE_BOOL)
                return (double)int_value;
            else
                return Double.Parse(this.string_value);
        }


        /**
         * Gets the value of this attribute as a boolean.
         */
        public bool asBool()
        {
            if (type == AttrType.TYPE_BOOL)
                return int_value != 0;
            else if (type == AttrType.TYPE_INT)
                return int_value != 0;
            else if (type == AttrType.TYPE_DOUBLE)
                return double_value != 0.0;
            else
            {
                string_value = string_value.ToLowerInvariant();
                return string_value == "true" || string_value == "yes" || string_value == "on" ||
                       string_value == "t" || string_value == "y";
            }
        }


        /**
         * Creates an "invalid" (error condition) attribute.
         */
        public static Attribute invalid()
        {
            return new Attribute();
        }

        private bool valid = false;
        private string key;
        private AttrType type;
        private string string_value;
        private int int_value;
        private double double_value;
    }

    /**
     * A lookup-table of attributes indexed by name.
     */
    public class AttributeTable : Dictionary<string, Attribute> { }

    /**
     * A linked list of Attribute objects.
     */
    public class AttributeList : List<Attribute> { }

    /**
     * Describes an attribute by name and type.
     *
     * This is basically just an Attribute without the value; but this class can
     * also convey custom domain-specific properties.
     */
    public class AttributeSchema
    {

        /**
         * Constructs a new, empty attribute schema.
         */
        public AttributeSchema() { }

        /**
         * Constructs an attribute schema.
         */
        public AttributeSchema(string _attr_name, Attribute.AttrType _attr_type)
        {
            name = _attr_name.ToLowerInvariant();
            type = _attr_type;
        }


        /**
         * Constructs a new attribute schema with custom properties.
         */
        public AttributeSchema(string _attr_name, Attribute.AttrType _attr_type, Properties _props)
        {
            name = _attr_name.ToLowerInvariant();
            type = _attr_type;
            props = _props;
        }


        /**
         * Gets the name of the attribute.
         */
        public string getName()
        {
            return name;
        }

        /**
         * Gets the value type of the attribute.
         */
        public Attribute.AttrType getType()
        {
            return type;
        }

        /**
         * Gets the set of custom properties associated with the attribute.
         */
        public Properties getProperties()
        {
            return props;
        }

        private string name;
        private Attribute.AttrType type;
        private Properties props;
    }

    /**
     * A lookup table that maps names to AttributeSchema instances.
     */
    public class AttributeSchemaTable : Dictionary<string, AttributeSchema> { }

    /**
     * A linked-list of AttributeSchema objects.
     */
    public class AttributeSchemaList : List<AttributeSchema> { }


    /**
     * Convenience base-class interface for any class that has a list of Attributes.
     */
    public interface Attributed
    {

        /**
         * Gets a copy of the attribute associated with a key string
         * @param key
         *      Name of the attribute to return
         */
        Attribute getAttribute(string key);


        /**
         * Gets a (copied) collection of all attributes in this feature.
         */
        AttributeList getAttributes();

        /**
         * Gets a copy of the full attribute schema for each attribute.
         */
        AttributeSchemaList getAttributeSchemas();

        /**
         * Sets an attribute to a string value.
         */
        void setAttribute(string key, string value);

        /**
         * Sets an attribute to an integer value.
         */
        void setAttribute(string key, int value);

        /**
         * Sets an attribute to a double value.
         */
        void setAttribute(string key, double value);

        /**
         * Sets an attribute to a boolean value.
         */
        void setAttribute(string key, bool value);

        /**
         * Sets an attribute to a copy of another attribute.
         */
        void setAttribute(Attribute attr);
    }

    /**
     * Base class for any object containing an Attribute table.
     */
    public class AttributedBase : Attributed
    {

        /**
         * Gets a copy of the attribute associated with a key string
         * @param key
         *      Name of the attribute to return
         */
        public virtual Attribute getAttribute(string key)
        {
            string lkey = key.ToLowerInvariant();
            return user_attrs[key];
        }


        /**
         * Gets a (copied) collection of all attributes in this feature.
         */
        public virtual AttributeList getAttributes()
        {
            //AttributeList result;
            //foreach (AttributeTable::const_iterator i = user_attrs.begin(); i != user_attrs.end(); i++)
            //    result.push_back(i->second);
            return (AttributeList)(user_attrs.Values.ToList<Attribute>());
        }


        /**
         * Gets a copy of the full attribute schema for each attribute.
         */
        public virtual AttributeSchemaList getAttributeSchemas()
        {
            AttributeSchemaList result = new AttributeSchemaList();

            foreach (KeyValuePair<string, Attribute> i in user_attrs)
            {
                result.Add(new AttributeSchema(i.Key, i.Value.getType(), new Properties()));
            }

            return result;
        }

        /**
         * Sets an attribute to a string value.
         */
        public virtual void setAttribute(string key, string value)
        {
            string lkey = key.ToLowerInvariant();
            user_attrs[lkey] = new Attribute(lkey, value);
        }


        /**
         * Sets an attribute to an integer value.
         */
        public virtual void setAttribute(string key, int value)
        {
            string lkey = key.ToLowerInvariant();
            user_attrs[lkey] = new Attribute(lkey, value);
        }

        /**
         * Sets an attribute to a double value.
         */
        public virtual void setAttribute(string key, double value)
        {
            string lkey = key.ToLowerInvariant();
            user_attrs[lkey] = new Attribute(lkey, value);
        }


        /**
         * Sets an attribute to a boolean value.
         */
        public virtual void setAttribute(string key, bool value)
        {
            string lkey = key.ToLowerInvariant();
            user_attrs[lkey] = new Attribute(lkey, value);
        }

        /**
         * Sets an attribute to a copy of another attribute.
         */
        public virtual void setAttribute(Attribute attr)
        {
            string lkey = attr.getKey();
            user_attrs[lkey] = attr;
        }


        /**
         * Gets the set of user-created attributes.
         */
        protected AttributeTable getUserAttrs()
        {
            return user_attrs;
        }

        private AttributeTable user_attrs= new AttributeTable();
    }


}
