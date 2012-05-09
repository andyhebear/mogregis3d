using System;
using System.Collections.Generic;

using Mogre;

namespace MogreGis
{
    public class StateSetList : List<Material> { }

    /**
     * Caches statesets and other objects created from resource
     * definitions. This object is intended for use in a single-threaded
     * manner.
     */
    public class ResourceCache
    {

        /**
         * Constructs a new resource cache.
         */
        public ResourceCache()
        {
            //NOP
        }

#if TODO
        /**
         * Gets the OSG state set associated with a skin resource, creating it
         * anew if necessary
         *
         * @param skin
         *      Skin for which to create the state set
         * @return
         *      An OSG state set containing a texture
         */
        public Material getStateSet(SkinResource skin)
        {
            Material result = null;
            if (skin != null)
            {
                if (!skin_state_sets.ContainsKey(skin))
                {
                    result = skin.createStateSet();
                    skin_state_sets[skin] = result;
                    //skin_state_sets[skin] = result;
                }
                else
                {
                    if (skin_state_sets[skin] == null) // null state set..
                    {
                        result = skin.createStateSet();
                        skin_state_sets[skin] = result;
                    }
                    else
                    {
                        result = skin_state_sets[skin];
                    }
                }
            }
            return result;
        }

        /**
         * Gets a node containing the scene graph loaded from the specified
         * model specification.
         *
         * @param model
         *      Model spec for which to load a model
         * @optimize
         *      Whether to run the optimizer on the model upon load (default=false)
         * @return
         *      Node containing the scene graph 
         */
        public Node getNode(ModelResource model)
        {
            return getNode(model, false);
        }
        public Node getNode(ModelResource model, bool optimize)
        {
            Node result = null;
            if (model != null)
            {
                if (!model_nodes.ContainsKey(model))
                {
                    bool simplify_extrefs = true; //TODO
                    result = model.createNode();
                    if (result != null)
                    {
                        if (optimize)
                        {
                            //GeomUtils::setDataVarianceRecursively( result, osg::Object::STATIC );
                            osgUtil.Optimizer o;
                            o.optimize(result);
                        }
                        model_nodes[model] = result;
                        //model_nodes[model.getAbsoluteURI()] = result;

                        // prevent optimization later when the object might be shared!
                        //result.setDataVariance( osg::Object::DYNAMIC ); //gw 7/8/08
                    }
                }
                else
                {
                    result = model_nodes[model];
                }
            }
            return result;
        }

        /**
         * Gets a node referening the specified model as an external reference
         *
         * @param model
         *      Model spec for which to create an external reference
         * @return
         *      Node containing the scene graph 
         */
        public Node getExternalReferenceNode(ModelResource model)
        {
            Node result = null;
            if (model != null)
            {
                if (!model_proxy_nodes.ContainsKey(model))
                {
                    result = model.createProxyNode();
                    model_proxy_nodes[model] = result;
                    //model_nodes[model.getAbsoluteURI()] = result;
                }
                else
                {
                    result = model_proxy_nodes[model];
                }
            }
            return result;
        }

        /**
         * Adds a hand-built skin resource directly to the cache.
         * For single-use (e.g. unique to the FilterEnv) skins, it is better (and
         * usually necessary for memory-usage purposes) to add them directly to the
         * local cache (versus the Resource Library) so that they don't sit around 
         * using up memory for an entire compilation.
         */
        //void addSkin( osg::Image* image );

        public SkinResource addSkin(Material state_set)
        {
            SkinResource skin = new SkinResource();
            skin_state_sets[skin] = state_set;
            return skin;
        }

        //StateSetList getSkinStateSets();

        public SkinResources getSkins()
        {
            SkinResources results = new SkinResources();
            foreach (SkinResource i in skin_state_sets.Keys)
            {
                results.Add(i);
            }
            return results;
        }

        public ModelResources getModels()
        {
            ModelResources results = new ModelResources();
            foreach (ModelResource i in model_nodes.Keys)
            {
                results.Add(i);
            }
            return results;
        }

        public ModelResources getExternalReferenceModels()
        {
            ModelResources results = new ModelResources();
            foreach (ModelResource i in model_proxy_nodes.Keys)
            {
                results.Add(i);
            }
            return results;
        }


        //typedef std::pair< osg::ref_ptr<SkinResource>, osg::ref_ptr<osg::StateSet> > SkinStateSet;
        public class SkinStateSets : Dictionary<SkinResource, Material> { }
        //typedef std::pair< osg::ref_ptr<ModelResource>, osg::ref_ptr<osg::Node> > ModelNode;
        public class ModelNodes : Dictionary<ModelResource, Node> { }
        public class ModelProxyNodes : Dictionary<ModelResource, Node> { }


        private SkinStateSets skin_state_sets;
        private ModelNodes model_nodes;
        private ModelNodes model_proxy_nodes;
#endif

    }
}
