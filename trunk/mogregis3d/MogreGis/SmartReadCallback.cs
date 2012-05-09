using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mogre;
using Sharp3D.Math.Core;

namespace MogreGis
{
    /* (internal class)
     *
     * Read callback for the IntersectionVisitor that traverses PagedLODs, maintains a
     * cache, and tracks the MRU node. The MRU dramatically speeds up multiple localized
     * intersection tests.
     */
    public class SmartReadCallback : osgUtil.IntersectionVisitor.ReadCallback
    {

        public SmartReadCallback()
            : this(100)
        {
        }
        public SmartReadCallback(int max_lru);

        public void setMinRange(float min_range);
        public float getMinRange();

        public Node getMruNode();

        public void setMruNode(Node node);

        public Node getMruNodeIfContains(Vector3D p, Node fallback);

        public Node getMruNodeIfContains(Vector3D p1, Vector3D p2, Node fallback);

        public float getMruHitRatio();

        // ReadCallback

        public virtual Node readNodeFile(string filename);


        public class FileNameSceneMap : Dictionary<string, Node> { }

        private struct NodeRef
        {
            NodeRef(string _name, Node _node)
            {
                name = _name;
                node = _node;
            }
            string name;
            Node node;
            DateTime timestamp;
        }

        private struct NodeRefComp
        {
            //         bool operator() ( NodeRef  lhs, NodeRef  rhs )   {
            //             return lhs.timestamp < rhs.timestamp;
            //         }
        }

        private class MRUSet : HashSet<NodeRef> { } //TODO What about NodeRefComp??
        //typedef std::queue<osg::ref_ptr<NodeRef> > MRUQueue;
        private class NodeMap : Dictionary<string, NodeRef> { }

        private MRUSet mru;
        //MRUQueue mru_queue;
        private NodeMap node_map;

        private uint max_cache_size;
        //OpenThreads::Mutex  _mutex;
        private FileNameSceneMap cache;
        private Node mru_node;
        private Mogre.BoundingSphere mru_world_bs;
        private int mru_tries, mru_hits;

        private float min_isect_range;
    }
}
