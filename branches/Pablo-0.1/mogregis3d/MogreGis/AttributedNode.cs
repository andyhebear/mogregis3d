using System.Collections.Generic;

using Mogre;

namespace MogreGis
{
    /**
    * An osg::Node coupled with an Attribute set.
    *
    * In the process of compiling features into a scene graph, the data goes
    * through two transformations: first from Features to Fragments, and then
    * from Fragments to AttributedNodes.
    *
    * A NodeFilter is a filter that turns Fragment data into AttributedNode data.
    */
    public class AttributedNode : AttributedBase
    {

        /**
         * Creates a new, empty AttributedNode.
         */
        public AttributedNode()
        {
            //NOP
        }

        /**
         * Creates a new AttributedNode that holds an osg::Node.
         *
         * @param node
         *      Node to encapsulate
         */
        public AttributedNode(Node _node)
        {
            node = _node;
        }

        /**
         * Creates a new AttributedNode that holds an osg::Node.
         *
         * @param node
         *      Node to encapsulate
         * @param attrs
         *      Attributes to include
         */
        public AttributedNode(Node _node, AttributeList attrs)
        {
            node = _node;
            addAttributes(attrs);
        }



        /**
         * Sets the name of this node.
         *
         * @param name
         *      Name of the node.
         */
        public void setName(string _name)
        {
            name = _name;
        }

        /**
         * Gets the name of this node. @see #setName for more information.
         *
         * @return AttributedNode's name. 
         */
        public string getName()
        {
            return name;
        }

        /**
         * Gets whether this node has a name.
         *
         * @return True if the node has a name; false if not.
         */
        public bool hasName()
        {
            return !string.IsNullOrEmpty(name);
        }

        /**
         * Gets access to the object's inner osg::Node.
         *
         * @return an osg::Node refernece
         */
        public Node getNode()
        {
            return node;
        }

        /**
         * Sets the node in this AttributedNode.
         *
         * @param node Node to set
         */
        public void setNode(Node _node)
        {
            node = _node;
        }

        /**
         * Adds a collection of attributes to this fragment. The compiler will embed the
         * attributes into the resulting scene graph as a osgSim::ShapeAttributeList.
         *
         * If a fragment has attributes, the compiler will be forces to maintain "feature
         * integrity" -- the feature will end up residing alone under a dedicated osg::Geode
         * instead of being optimized into a larger set of geometries.
         *
         * @param attrs
         *      Attributes to embed
         */
        public void addAttributes(AttributeList attrs)
        {
            foreach (Attribute i in attrs)
            {
                this.setAttribute(i);
            }
        }



        private string name;
        private Node node;
    }


    /** A list of reference-counted attributed node objects. */
    public class AttributedNodeList : List<AttributedNode> { }
}
