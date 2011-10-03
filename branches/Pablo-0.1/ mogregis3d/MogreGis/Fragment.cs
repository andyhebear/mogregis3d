using System;
using System.Collections.Generic;


using Mogre;
namespace MogreGis
{
    /** A list of osg::Drawable references. */
    public class DrawableList : List<Renderable> { }

    /**
     * A collection of osg::Drawable instances paired with an attribute set.
     *
     * In the process of compiling features into a scene graph, the data goes
     * through two transformations: first from Features to Fragments, and then
     * from Fragments to Nodes. A "fragment" is a kind of half-way state and
     * consists of OSG drawables that may still have feature attributes, but that
     * have no scene graph structure.
     *
     * A FragmentFilter is a filter that turns Feature data into Fragment data.
     */
    public class Fragment : AttributedBase
    {

        /**
         * Creates a new, empty Fragment.
         */
        public Fragment()
        {
            //NOP
        }

        public Fragment(SceneNode node)
        {
            this.node = node;
        }

        /**
         * Creates a new fragment that holds a single osg::Drawable.
         *
         * @param drawable
         *      Drawable to add to the fragment
         */
        public Fragment(Renderable drawable)
        {
            if (drawable != null)
                getDrawables().Add(drawable);
        }


        /**
         * Sets the name of this fragment.
         *
         * If a fragment has a name set, the compiler will be forces to maintain "feature
         * integrity" -- the feature will end up residing alone under a dedicated osg::Geode
         * instead of being optimized into a larger set of geometries.
         *
         * @param name The Fragment name
         */
        public void setName(string _name)
        {
            name = _name;
        }

        /**
         * Gets the name of this Fragment. @see #setName for more information.
         *
         * @return Fragment's name. 
         */
        public string getName()
        {
            return name;
        }

        /**
         * Gets whether this fragment has a name.
         *
         * @return True if the fragment has a name; false if not.
         */
        public bool hasName()
        {
            return !string.IsNullOrEmpty(name);
        }

        /**
         * Gets access to the fragment's collection of osg::Drawable's.
         *
         * @return List of osg::Drawable instances
         */
        public DrawableList getDrawables()
        {
            return drawables;
        }

        /**
         * Adds a drawable to the fragment.
         *
         * @param d
         *      Drawable to add
         */
        public void addDrawable(Renderable d)
        {
            drawables.Add(d);
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
        private DrawableList drawables = new DrawableList();

        private SceneNode node;

        public SceneNode Node
        {
            set
            {
                this.node = value;
            }

            get
            {
                return node;
            }
        }
    }


    /** A list of reference-counted features. */
    public class FragmentList : List<Fragment> { }
}
