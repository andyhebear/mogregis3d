using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MogreGis
{
    /* Unique identifier for an rtree node. */
    public struct RtNodeId
    {
        private int Id;
        public RtNodeId(int id)
        {
            Id = id;
        }
        public static implicit operator RtNodeId(int id)
        {
            return new RtNodeId(id);
        }
        public static implicit operator int(RtNodeId node)
        {
            return node.Id;
        }

    }

    /* Single entry in either a leaf or non-leaf node */
    public class RtEntry<DATA>
    {

        public RtEntry() { }
        public GeoExtent extent;

        public RtNodeId child_id = 0;
        public DATA data;

    }

    /* Ordered collection of node entries */
    public class RtEntryList<DATA> : List<RtEntry<DATA>> { };

    /* An R-tree node */
    public class RtNode<DATA>
    {

        public RtNode() { }
        public bool isLeaf() { return is_leaf; }
        public bool isRoot() { return parent_id == 0; }

        public RtNodeId id = 0;
        public RtNodeId parent_id = 0;
        public bool is_leaf = true;
        public RtEntryList<DATA> entries;
    };

    /* table that maps node IDs to nodes (for caching/in-memory storage) */
    public class RtNodeMap<DATA> : Dictionary<RtNodeId, RtNode<DATA>> { };

    /* An R-tree spatial data structure */
    public class RTree<DATA>
    {

        public RTree()
        {
            node_id_gen = 0;
            RtNode<DATA> root = new RtNode<DATA>();
            root.is_leaf = true;
            root_id = ++node_id_gen;
            root.id = root_id;
            root.parent_id = 0;
            node_table[root.id] = root;
        }

        public void insert(GeoExtent extent, DATA data)
        {
            // adjust zero-area extents
            if (extent.getArea() == 0.0)
            {
                GeoExtent extent2 = extent;
                extent2.expand(0.0001, 0.0001);
                insert(root_id, extent2, data);
            }
            else
            {
                insert(root_id, extent, data);
            }
        }

        public List<DATA> find(GeoExtent extent)
        {
            List<DATA> list = new List<DATA>();
            find(root_id, extent, list);
            return list;
        }

#if TODO
        public  bool writeTo( std::ostream& out, const GeoExtent& extent );
        public  bool readFrom( std::istream& in, SpatialReference* srs, GeoExtent& out_extent );
#endif

        GeoExtent
        computeExtent(RtEntryList<DATA> list)
        {
            GeoExtent extent = new GeoExtent();
            foreach (RtEntry<DATA> i in list)
            {
                extent.expandToInclude(i.extent);
            }
            return extent;
        }

        private void insert(RtNodeId node_id, GeoExtent extent, DATA data)
        {
            // I1
            RtNode<DATA> L = chooseLeaf(node_id, extent);
            RtNode<DATA> LL = null;

            // I2
            RtEntry<DATA> entry = new RtEntry<DATA>();
            entry.extent = extent;
            entry.data = data;
            L.entries.Add(entry);

            if (L.entries.Count >= CONST_M)
            {
                LL = splitNodeQ(L);
            }

            // I3, I4
            adjustTree(L, LL);
        }

        private RtNode<DATA> chooseLeaf(RtNodeId node_id, GeoExtent extent)
        {
            RtNode<DATA> node = node_table[node_id];
            if (!node.isLeaf())
            {
                double extent_area = extent.getArea();
                double smallest_area_diff = Double.MaxValue;
                RtEntry<DATA> best_i = node.entries[0];

                foreach (RtEntry<DATA> i in node.entries)
                {
                    RtEntry<DATA> entry = i;
                    GeoExtent future_extent = entry.extent;
                    future_extent.expandToInclude(extent);
                    double area_diff = future_extent.getArea() - extent_area;
                    if (area_diff < smallest_area_diff)
                    {
                        smallest_area_diff = area_diff;
                        best_i = i;
                    }
                }

                return chooseLeaf(best_i.child_id, extent);
            }
            else
            {
                return node;
            }
        }

        private void adjustTree(RtNode<DATA> N, RtNode<DATA> NN)
        {
            if (N.isRoot() && NN != null) // I4
            {
                // split the root node
                RtNode<DATA> P = new RtNode<DATA>();
                P.is_leaf = false;
                P.id = ++node_id_gen;
                P.parent_id = N.parent_id;
                root_id = P.id;
                node_table[P.id] = P;

                N.parent_id = P.id;
                RtEntry<DATA> E_N = new RtEntry<DATA>();
                E_N.child_id = N.id;
                E_N.extent = computeExtent(N.entries);
                P.entries.Add(E_N);

                NN.parent_id = P.id;
                RtEntry<DATA> E_NN = new RtEntry<DATA>();
                E_NN.child_id = NN.id;
                E_NN.extent = computeExtent(NN.entries);
                P.entries.Add(E_NN);
            }
            else if (!N.isRoot())
            {
                // AT1, AT2
                RtNode<DATA> P = node_table[N.parent_id];
                RtNode<DATA> PP = null;

                // AT3
                foreach (RtEntry<DATA> i in P.entries)
                {
                    if (i.child_id == N.id)
                    {
                        i.extent = computeExtent(N.entries);
                        break;
                    }
                }

                // AT4
                if (NN != null)
                {
                    NN.parent_id = P.id;

                    RtEntry<DATA> E_NN = new RtEntry<DATA>();
                    E_NN.child_id = NN.id;
                    E_NN.extent = computeExtent(NN.entries);
                    P.entries.Add(E_NN);

                    if (P.entries.Count >= CONST_M)
                    {
                        PP = splitNodeQ(P);
                    }
                }

                adjustTree(P, PP);
            }
        }

        private RtNode<DATA> splitNodeQ(RtNode<DATA> L)
        {
            RtNode<DATA> LL = new RtNode<DATA>();
            LL.is_leaf = L.is_leaf;
            LL.parent_id = L.parent_id;
            LL.id = ++node_id_gen;
            node_table[LL.id] = LL;

            RtEntryList<DATA> first_group = new RtEntryList<DATA>();
            RtEntryList<DATA> second_group = new RtEntryList<DATA>();

            int first_index = 0;
            int second_index = 1;

            pickSeedsQ(L.entries, out first_index, out second_index);
            RtEntry<DATA> first = L.entries[first_index];
            RtEntry<DATA> second = L.entries[second_index];

            first_group.Add(first);
            second_group.Add(second);

            GeoExtent first_ext = computeExtent(first_group);
            GeoExtent second_ext = computeExtent(second_group);

            foreach (RtEntry<DATA> i in L.entries)
            {
                if (i != first && i != second)
                {
                    if (first_group.Count >= CONST_M / 2)
                    {
                        second_group.Add(i);
                        second_ext = computeExtent(second_group);
                    }
                    else if (second_group.Count >= CONST_M / 2)
                    {
                        first_group.Add(i);
                        first_ext = computeExtent(first_group);
                    }
                    else
                    {
                        GeoExtent e_first = first_ext;
                        e_first.expandToInclude(i.extent);
                        double d_first = e_first.getArea() - first_ext.getArea();

                        GeoExtent e_second = second_ext;
                        e_second.expandToInclude(i.extent);
                        double d_second = e_second.getArea() - second_ext.getArea();

                        if (d_first < d_second)
                        {
                            first_group.Add(i);
                            first_ext = computeExtent(first_group);
                        }
                        else
                        {
                            second_group.Add(i);
                            second_ext = computeExtent(second_group);
                        }
                    }
                }
            }

            L.entries = first_group;
            LL.entries = second_group;

            // if LL has subnodes, inform them of their new parent.
            if (!LL.isLeaf())
            {
                foreach (RtEntry<DATA> i in LL.entries)
                {
                    RtNode<DATA> child_node = node_table[i.child_id];
                    child_node.parent_id = LL.id;
                }
            }

            return LL;
        }

        private void find(RtNodeId node_id, GeoExtent extent, List<DATA> results)
        {
            RtNode<DATA> node = node_table[node_id];
            foreach (RtEntry<DATA> i in node.entries)
            {
                if (i.extent.intersects(extent))
                {
                    if (node.isLeaf())
                    {
                        results.Add(i.data);
                    }
                    else
                    {
                        find(i.child_id, extent, results);
                    }
                }
            }
        }

        private void pickSeedsQ(RtEntryList<DATA> list, out int out_first_index, out int out_second_index)
        {
            double largest_d = -1.0; //0.0;
            out_first_index = 0;
            out_second_index = 0;

            for (int i = 0; i < list.Count; i++)
            {
                RtEntry<DATA> one = list[i];
                for (int j = i + 1; i < list.Count; j++)
                {
                    RtEntry<DATA> two = list[i];
                    GeoExtent combined = one.extent;
                    combined.expandToInclude(two.extent);
                    double d = combined.getArea() - one.extent.getArea() - two.extent.getArea();
                    if (d > largest_d)
                    {
                        largest_d = d;
                        out_first_index = i;
                        out_second_index = j;
                    }
                }
            }
        }


        private RtNodeMap<DATA> node_table;
        private RtNodeId root_id;
        private RtNodeId node_id_gen;

        // constants from Guttman84
        private const int CONST_M = 64;
        private const int CONST_m = (CONST_M / 2);

    }
}
