using System;
using System.Numerics;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using BntxLibrary.Core;

namespace BntxLibrary.Common.Restore
{
    /// <summary>
    /// Represents the non-generic base of a dictionary which can quickly look up <see cref="IResData"/> instances via
    /// key or index.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(TypeProxy))]
    public class NodeTable : IEnumerable, IResData
    {
        private IList<Node> nodes; // Includes root node.

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeTable"/> class.
        /// </summary>
        public NodeTable()
        {
            // Create root node.
            nodes = new List<Node> { new Node() };
        }

        // 
        // Properties

        /// <summary>
        /// Gets the number of instances stored.
        /// </summary>
        public int Count => nodes.Count - 1;

        // 
        // Methods (Public)

        /// <summary>
        /// Adds the given <paramref name="key"/> to insert in the dictionary.
        /// </summary>
        /// <exception cref="ArgumentException">Duplicated <paramref name="key"/> instances
        /// already exists.</exception>
        public void Add(string key)
        {
            if (!ContainsKey(key))
            {
                nodes.Add(new Node(key));
            }
            else throw new ArgumentException($"An item with the same key has already been added. Key: '{key}'");
        }

        /// <summary>
        /// Removes the given <paramref name="key"/> from the dictionary.
        /// </summary>
        /// <exception cref="ArgumentException">Duplicated <paramref name="key"/> instances
        /// already exists.</exception>
        public void Remove(string key)
        {
            if (ContainsKey(key))
            {
                nodes.Remove(nodes.First(n => n.Key == key));
            }
            else throw new KeyNotFoundException($"The given key '{key}' was not present in the dictionary.");
        }

        /// <summary>
        /// Removes all elements from the dictionary.
        /// </summary>
        public void Clear()
        {
            // Create new collection with root node.
            nodes.Clear();
            nodes.Add(new Node());
        }

        /// <summary>
        /// Determines whether the given <paramref name="key"/> is in the dictionary.
        /// </summary>
        /// <returns><c>true</c> if <paramref name="key"/> was found in the dictionary; otherwise <c>false</c>.
        /// </returns>
        public bool ContainsKey(string key)
        {
            return nodes.Any(p => p.Key == key);
        }

        /// <summary>
        /// Returns the key given <paramref name="index"/> is within range of the dictionary.
        /// </summary>
        public string GetKey(int index)
        {
            if (index < nodes.Count && index > 0)
            {
                return nodes[index + 1].Key;
            }
            else throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Returns the key given <paramref name="index"/> is within range of the dictionary.
        /// </summary>
        public void SetKey(int index, string key)
        {
            if (index < nodes.Count && index > 0)
            {
                nodes[index + 1].Key = key;
            }
            else throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Gets or sets the <see cref="IResData"/> instance stored at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The 0-based index of the <see cref="IResData"/> instance to get or set.</param>
        /// <returns>The <see cref="IResData"/> at the specified <paramref name="index"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// The index is smaller than 0 or bigger or equal to <see cref="Count"/>.
        /// </exception>
        public string this[int index]
        {
            get => Lookup(index).Key;
            set
            {
                var node = Lookup(index);
                node.Key = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IResData"/> instance stored with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The 0-based index of the <see cref="IResData"/> instance to get or set.</param>
        /// <returns>The <see cref="IResData"/> with the specified <paramref name="key"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The index is smaller than 0 or bigger or equal to <see cref="Count"/>.
        /// </exception>
        public string this[string key]
        {
            get => Lookup(IndexOf(key)).Key;
            set
            {
                var node = Lookup(IndexOf(key));
                node.Key = value;
            }
        }

        /// <summary>
        /// Searches for the specified <paramref name="value"/> and returns the zero-based index of the first occurrence
        /// within the entire dictionary.
        /// </summary>
        /// <param name="value">The <see cref="IResData"/> instance to locate in the dictionary. The value can be
        /// <c>null</c>.</param>
        /// <returns>The zero-based index of the first occurence of <paramref name="value"/> within the entire
        /// dictionary if found; otherwise <c>-1</c>.</returns>
        public int IndexOf(string key)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Key == key)
                {
                    return i;
                }
            }
            return -1;
        }

        //
        // Methods (Internal)

        void IResData.Load(BntxFileLoader loader)
        {
            // Read the header.
            uint signature = loader.ReadUInt32(); //_DIC
            int numNodes = loader.ReadInt32(); // Excludes root node.

            int i = 0;
            // Read the nodes including the root node.
            List<Node> nodes = new List<Node>();
            for (; numNodes >= 0; numNodes--)
            {
                nodes.Add(ReadNode(loader));
                i++;
            }
            this.nodes = nodes;
        }

        void IResData.Save(BntxFileWriter writer)
        {
            // Update the Patricia trie values in the nodes.
            UpdateNodes();

            // Write header.
            writer.WriteSignature("_DIC");
            writer.Write(Count);

            // Write nodes.
            int index = -1; // Start at -1 due to root node.
            int curNode = 0;
            foreach (Node node in nodes)
            {
                writer.Write(node.Reference);
                writer.Write(node.IdxLeft);
                writer.Write(node.IdxRight);

                if (curNode == 0)
                {
                    writer.SaveRelocateEntryToSection(writer.Position, 1, (uint)nodes.Count, 1, BntxFileWriter.Section1, ""); //      <------------ Entry Set
                    writer.SaveString("");
                }   
                else
                {
                    writer.SaveString(node.Key);
                }
                curNode++;
            }
        }

        /// <summary>
        /// Returns the publically visible nodes, excluding the root node.
        /// </summary>
        /// <remarks>
        /// Should return <see cref="Node"/>, instead of <see cref="Node"/>.Key as <see cref="string"/>
        /// </remarks>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (Node node in nodes) yield return node.Key;
        }

        /// <summary>
        /// Returns the publically visible nodes, excluding the root node.
        /// </summary>
        /// <remarks>
        /// Not really sure what this is for...
        /// </remarks>
        private IEnumerator<Node> Nodes
        {
            get
            {
                foreach (Node node in nodes) yield return node;
            }
        }

        //
        // Methods (Private)

        private Node Lookup(int index)
        {
            if (index < nodes.Count && index > 0)
            {
                return nodes[index + 1];
            }
            else throw new IndexOutOfRangeException();
        }

        class Tree
        {
            public Node root;

            public Dictionary<BigInteger, Tuple<int, Node>> entries;

            public Tree()
            {
                entries = new Dictionary<BigInteger, Tuple<int, Node>>();

                root = new Node(0, -1, root);
                root.Parent = root;

                insertEntry(0, root);
            }

            int GetCompactBitIdx()
            {
                return -1;
            }

            public void insertEntry(BigInteger data, Node node)
            {
                entries[data] = (Tuple.Create(entries.Count, node));
            }

            Node Search(BigInteger data, bool prev)
            {
                if (root.Child[0] == root)
                    return root;

                Node node = root.Child[0];
                Node prevNode = node;
                while (true)
                {
                    prevNode = node;
                    node = node.Child[data.GetBit(node.bitInx)];
                    if (node.bitInx <= prevNode.bitInx)
                        break;
                }
                if (prev)
                    return prevNode;
                else
                    return node;
            }

            public void Insert(string name)
            {
                string bits = name.ToBinaryString(Encoding.UTF8);
                BigInteger data = bits.Aggregate(new BigInteger(), (b, c) => b * 2 + c - '0');
                Node current = Search(data, true);
                int bitIdx = Bit.Mismatch(current.Data, data);

                while (bitIdx < current.Parent.bitInx)
                    current = current.Parent;

                if (bitIdx < current.bitInx)
                {
                    Node newNode = new Node(data, bitIdx, current.Parent);
                    newNode.Child[data.GetBit(bitIdx) ^ 1] = current;
                    current.Parent.Child[data.GetBit(current.Parent.bitInx)] = newNode;
                    current.Parent = newNode;

                    insertEntry(data, newNode);
                }
                else if (bitIdx > current.bitInx)
                {
                    Node newNode = new Node(data, bitIdx, current);
                    if (current.Data.GetBit(bitIdx) == (data.GetBit(bitIdx) ^ 1))
                        newNode.Child[data.GetBit(bitIdx) ^ 1] = current;
                    else
                        newNode.Child[data.GetBit(bitIdx) ^ 1] = root;


                    current.Child[data.GetBit(current.bitInx)] = newNode;
                    insertEntry(data, newNode);
                }
                else
                {

                    int NewBitIdx = data.GetFirst1Bit();

                    if (current.Child[data.GetBit(bitIdx)] != root)
                        NewBitIdx = Bit.Mismatch(current.Child[data.GetBit(bitIdx)].Data, data);
                    Node newNode = new Node(data, NewBitIdx, current);

                    newNode.Child[data.GetBit(NewBitIdx) ^ 1] = current.Child[data.GetBit(bitIdx)];
                    current.Child[data.GetBit(bitIdx)] = newNode;
                    insertEntry(data, newNode);
                }
            }
        }


        private void UpdateNodes()
        {
            Tree tree = new Tree();

            // Create a new root node with empty key so the length can be retrieved throughout the process.
            nodes[0] = new Node() { Key = String.Empty, bitInx = -1, Parent = nodes[0] };

            // Update the data-referencing nodes.
            for (ushort i = 1; i < nodes.Count; i++)
                tree.Insert(nodes[i].Key);

            int CurEntry = 0;
            foreach (var entry in tree.entries.Values)
            {
                Node node = entry.Item2;

                node.Reference = (uint)(node.GetCompactBitIdx() & 0xffffffff);
                node.IdxLeft = (ushort)tree.entries[node.Child[0].Data].Item1;
                node.IdxRight = (ushort)tree.entries[node.Child[1].Data].Item1;
                node.Key = node.GetName();
                nodes[CurEntry] = node;

                CurEntry++;
            }

            // Remove the dummy empty key in the root again.
            nodes[0].Key = null;
        }

        private Node ReadNode(BntxFileLoader loader)
        {
            return new Node()
            {
                Reference = loader.ReadUInt32(),
                IdxLeft = loader.ReadUInt16(),
                IdxRight = loader.ReadUInt16(),
                Key = loader.LoadString(),
            };
        }

        // ---- CLASSES ------------------------------------------------------------------------------------------------

        /// <summary>
        /// Represents a node forming the Patricia trie of the dictionary.
        /// </summary>
        [DebuggerDisplay(nameof(Node) + " {" + nameof(Key) + "}")]
        protected class Node
        {
            internal const int SizeInBytes = 16;

            internal List<Node> Child = new List<Node>();
            internal Node Parent;
            internal int bitInx;
            internal BigInteger Data;
            internal uint Reference;
            internal ushort IdxLeft;
            internal ushort IdxRight;
            internal string Key;

            internal Node()
            {
                Child.Add(this);
                Child.Add(this);
                Reference = UInt32.MaxValue;
            }
            internal string GetName()
            {
                BigInteger data = Data.GetBitLength() + 7 / 8;
                byte[] stringBytes = Data.ToByteArray();
                Array.Reverse(stringBytes, 0, stringBytes.Length); //Convert to big endian
                return Encoding.UTF8.GetString(stringBytes); //Decode byte[] to string
            }
            internal int GetCompactBitIdx()
            {
                int byteIndx = bitInx / 8;
                return (byteIndx << 3) | bitInx - 8 * byteIndx;
            }
            internal Node(BigInteger data, int bitidx, Node parent) : this()
            {
                Data = data;
                bitInx = bitidx;
                Parent = parent;
            }
            internal Node(string key) : this()
            {
                Key = key;
            }
        }

        private class TypeProxy
        {
            private NodeTable _dict;

            internal TypeProxy(NodeTable dict)
            {
                _dict = dict;
            }
        }
    }
}
