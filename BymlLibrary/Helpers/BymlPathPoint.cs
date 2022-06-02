using Syroot.Maths;
using System.ComponentModel;

namespace Nintendo.Byml
{
    /// <summary>
    /// Represents a point in a BYML path.
    /// </summary>
    public class BymlPathPoint
    {
        /// <summary>
        /// The size of a single point in bytes when serialized as BYAML data.
        /// </summary>
        internal const int SizeInBytes = 28;

        /// <summary>
        /// Initializes a new instance of the <see cref="BymlPathPoint"/> class.
        /// </summary>
        public BymlPathPoint() => Normal = new Vector3F(0, 1, 0);

		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		public Vector3F Position { get; set; }

		/// <summary>
		/// Gets or sets the normal.
		/// </summary>
		public Vector3F Normal { get; set; }

        /// <summary>
        /// Gets or sets an unknown value.
        /// </summary>
        public uint Unknown { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(BymlPathPoint other) => Position == other.Position && Normal == other.Normal && Unknown == other.Unknown;
        public override string ToString() => $"ByamlPathPoint Pos:{Position} Norm:{Normal} Unk:{Unknown}";
	}
}
