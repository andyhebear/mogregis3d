using System;
using System.Collections.Generic;


using Mogre;
namespace MogreGis
{
    /**
     * A filter that processes feature or fragment data into osg::Node scene graphs.
     *
     * A NodeFilter can take as input Feautres, Fragments or osg::Nodes. It generates
     * a list of osg::Node instances. To implement a NodeFilter you create a subclass and
     * implement the appropriate process(...) method(s).
     */
    public class NodeFilter : Filter
    {

        public override string getFilterType() { return getStaticFilterType(); }
        public override Filter clone() { return new NodeFilter(this); }
        public static string getStaticFilterType() { return "NodeFilter"; }
        public static FilterFactory getFilterFactory() { return new FilterFactoryImpl<NodeFilter>(); }

        /**
         * Processes a single feature into a collection of nodes.
         *
         * @param input
         *      Individual feature to process
         * @param env
         *      Contextual compilation environment
         * @return
         *      Resulting node list
         */
        public virtual AttributedNodeList process(Feature input, FilterEnv env)
        {
            AttributedNodeList output = new AttributedNodeList();
            //NOP - never called
            return output;
        }

        /**
         * Processes a collection of features into a collection of nodes.
         *
         * @param input
         *      Batch of features to process
         * @param env
         *      Contextual compilation environment
         * @return
         *      Resulting node list
         */
        public virtual AttributedNodeList process(FeatureList input, FilterEnv env)
        {
            AttributedNodeList output = new AttributedNodeList();
            foreach (Feature i in input)
            {
                AttributedNodeList interim = process(i, env);
                output.InsertRange(output.Count, interim);
            }
            return output;
        }

        /**
         * Processes a single fragment into a collection of nodes.
         *
         * @param input
         *      Individual fragment to process
         * @param env
         *      Contextual compilation environment
         * @return
         *      Resulting node list
         */
        public virtual AttributedNodeList process(Fragment input, FilterEnv env)
        {
#if TODO
            AttributedNodeList output;
            osg.Geode geode = new osg.Geode();
            foreach (Drawable i in input.getDrawables())
                geode.addDrawable(i);
            output.Add(new AttributedNode(geode));
            return output;
#endif
            throw new NotImplementedException();
        }

        /**
         * Processes a collection of fragments into a collection of nodes.
         *
         * @param input
         *      Batch of fragments to process
         * @param env
         *      Contextual compilation environment
         * @return
         *      Resulting node list
         */
        public virtual AttributedNodeList process(FragmentList input, FilterEnv env)
        {
            AttributedNodeList output = new AttributedNodeList();
            foreach (Fragment i in input)
            {
                AttributedNodeList interim = process(i, env);
                output.InsertRange(output.Count, interim);
            }
            return output;
        }

        /**
         * Processes a single node into a collection of nodes.
         *
         * @param input
         *      Individual node to process
         * @param env
         *      Contextual compilation environment
         * @return
         *      Resulting node list
         */
        public virtual AttributedNodeList process(AttributedNode input, FilterEnv env)
        {
            AttributedNodeList output = new AttributedNodeList();
            output.Add(input);
            return output;
        }

        /**
         * Processes a collection of nodes into a collection of nodes.
         *
         * @param input
         *      Batch of nodes to process
         * @param env
         *      Contextual compilation environment
         * @return
         *      Resulting node list
         */
        public virtual AttributedNodeList process(AttributedNodeList input, FilterEnv env)
        {
            AttributedNodeList output = new AttributedNodeList();
            foreach (AttributedNode i in input)
            {
                AttributedNodeList interim = process(i, env);
                output.InsertRange(output.Count, interim);
            }
            return output;
        }

        public override FilterState newState()
        {
            return new NodeFilterState((NodeFilter)clone());
        }


        public NodeFilter()
        {
            //NOP
        }
        protected NodeFilter(NodeFilter rhs)
            : base(rhs)
        {
            //NOP
        }

        protected void embedAttributes(Node node, AttributeList attrs)
        {
#if TODO
            osgSim.ShapeAttributeList to_embed = new osgSim.ShapeAttributeList();

            foreach (Attribute a in attrs)
            {
                switch (a.getType())
                {
                    case Attribute.AttrType.TYPE_INT:
                    case Attribute.AttrType.TYPE_BOOL:
                        to_embed.push_back(osgSim.ShapeAttribute(a.getKey(), a.asInt()));
                        break;
                    case Attribute.AttrType.TYPE_DOUBLE:
                        to_embed.push_back(osgSim.ShapeAttribute(a.getKey(), a.asDouble()));
                        break;
                    case Attribute.AttrType.TYPE_STRING:
                        to_embed.push_back(osgSim.ShapeAttribute(a.getKey(), a.asString()));
                }
            }

            node.setUserData(to_embed);
        
#endif
            throw new NotImplementedException();
        }
    }
}
