using System.IO;
using Syroot.BinaryData;
using Nintendo.Bfres.Core;

namespace Nintendo.Bfres.WiiU.Core
{
    /// <summary>
    /// Loads the hierachy and data of a <see cref="Bfres.ResFile"/>.
    /// </summary>
    public class ResFileWiiULoader : ResFileLoader
    {
        // ---- FIELDS -------------------------------------------------------------------------------------------------



        // ---- CONSTRUCTORS & DESTRUCTOR ------------------------------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="ResFileLoader"/> class loading data into the given
        /// <paramref name="resFile"/> from the specified <paramref name="stream"/> which is optionally left open.
        /// </summary>
        /// <param name="resFile">The <see cref="Bfres.ResFile"/> instance to load data into.</param>
        /// <param name="stream">The <see cref="Stream"/> to read data from.</param>
        /// <param name="leaveOpen"><c>true</c> to leave the stream open after reading, otherwise <c>false</c>.</param>
        internal ResFileWiiULoader(BfresFile resFile, Stream stream, bool leaveOpen = false) : base(resFile, stream, leaveOpen)
        {
            ByteConverter = ByteConverter.Big;
        }

        internal ResFileWiiULoader(IResData resData, BfresFile resFile, Stream stream, bool leaveOpen = false) : base(resData, resFile, stream, leaveOpen)
        {
            ByteConverter = ByteConverter.Big;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResFileLoader"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="resFile">The <see cref="Bfres.ResFile"/> instance to load data into.</param>
        /// <param name="fileName">The name of the file to load the data from.</param>
        internal ResFileWiiULoader(BfresFile resFile, string fileName) : base(resFile, fileName)
        {
            ByteConverter = ByteConverter.Big;
        }

        internal ResFileWiiULoader(IResData resData, BfresFile resFile, string fileName) : base(resData, resFile, fileName)
        {
            ByteConverter = ByteConverter.Big;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------
    }
}
