using System.Collections;

namespace Nintendo.Byml.IO
{
    /// <summary>
    /// BymlNodeType extension methods
    /// </summary>
    internal static class NodeTypeExtension
	{
		internal static bool IsEnumerable(this NodeType nodeType) => nodeType >= NodeType.Array && nodeType <= NodeType.PathArray;

		/// <summary>
		/// Gets the corresponding, instantiatable <see cref="Type"/> for the given <paramref name="nodeType"/>.
		/// </summary>
		/// <param name="nodeType">The <see cref="NodeTypeExtension"/> which should be instantiated.</param>
		/// <returns>The <see cref="Type"/> to instantiate for the node.</returns>
		internal static Type GetInstanceType(this NodeType nodeType)
		{
            return nodeType switch
            {
                NodeType.StringIndex => typeof(string),
                NodeType.PathIndex => typeof(List<BymlPathPoint>),
				// TODO: Check if this could be loaded as an object array.
				NodeType.Array => throw new BymlException("Cannot instantiate an array of unknown element type."),
				// TODO: Check if this could be loaded as a string-object dictionary.
                NodeType.Dictionary => throw new BymlException("Cannot instantiate an object of unknown type."),
                NodeType.Boolean => typeof(bool),
                NodeType.Integer => typeof(int),
                NodeType.Float => typeof(float),
                NodeType.Uinteger => typeof(uint),
                NodeType.Long => typeof(long),
                NodeType.ULong => typeof(ulong),
                NodeType.Double => typeof(double),
                NodeType.Null => typeof(object),
                _ => throw new BymlException($"Unknown node type {nodeType}."),
            };
        }

        /// <summary>
        /// Gets the BymlNodeType of the given <paramref name="node"/> object.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="isInternalNode"></param>
        /// <returns></returns>
        /// <exception cref="BymlException"></exception>
        static internal NodeType GetNodeType(dynamic node, bool isInternalNode = false)
        {
            if (isInternalNode)
            {
                return node switch
                {
                    IEnumerable<string> => NodeType.StringArray,
                    IEnumerable<List<NodeType>> => NodeType.PathArray,
                    _ => throw new BymlException($"Type '{node.GetType()}' is not supported as a main BYAML node."),
                };
            }
            else
            {
                return node switch
                {
                    string => NodeType.StringIndex,
                    List<BymlPathPoint> => NodeType.PathIndex,
                    IDictionary<string, dynamic> => NodeType.Dictionary,
                    IEnumerable => NodeType.Array,
                    bool => NodeType.Boolean,
                    int => NodeType.Integer,
                    float => NodeType.Float, // TODO decimal is float or double?
                    uint => NodeType.Uinteger,
                    long => NodeType.Long,
                    ulong => NodeType.ULong,
                    double => NodeType.Double,
                    null => NodeType.Null,
                    _ => throw new BymlException($"Type '{node.GetType()}' is not supported as a BYAML node."),
                };
            }
        }
    }
}
