﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Syroot.BinaryData;
using Syroot.Maths;
using Nintendo.Bfres.Core;

namespace Nintendo.Bfres.Switch.Core
{
    /// <summary>
    /// Loads the hierachy and data of a <see cref="Bfres.ResFile"/>.
    /// </summary>
    public class ResFileSwitchLoader : ResFileLoader
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
        internal ResFileSwitchLoader(BfresFile resFile, Stream stream, bool leaveOpen = false) : base(resFile, stream, leaveOpen)
        {
            ByteConverter = ByteConverter.Little;
            IsSwitch = true;
        }

        internal ResFileSwitchLoader(IResData resData, BfresFile resFile, Stream stream, bool leaveOpen = false)
    : base(resData, resFile, stream, leaveOpen)
        {
            ByteConverter = ByteConverter.Little;
            IsSwitch = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResFileLoader"/> class from the file with the given
        /// <paramref name="fileName"/>.
        /// </summary>
        /// <param name="resFile">The <see cref="Bfres.ResFile"/> instance to load data into.</param>
        /// <param name="fileName">The name of the file to load the data from.</param>
        internal ResFileSwitchLoader(BfresFile resFile, string fileName) : base(resFile, fileName)
        {
            ByteConverter = ByteConverter.Little;
            IsSwitch = true;
        }

        internal ResFileSwitchLoader(IResData resData, BfresFile resFile, string fileName) : base(resData, resFile, fileName)
        {
            ByteConverter = ByteConverter.Little;
            IsSwitch = true;
        }

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------


        // ---- METHODS (INTERNAL) -------------------------------------------------------------------------------------

        /// <summary>
        /// Reads and returns an instance of arbitrary type <typeparamref name="T"/> from the following offset with the
        /// given <paramref name="callback"/> or returns <c>null</c> if the read offset is 0.
        /// </summary>
        /// <typeparam name="T">The type of the data to read.</typeparam>
        /// <param name="callback">The callback to read the instance data with.</param>
        /// <param name="offset">The optional offset to use instead of reading a following one.</param>
        /// <returns>The data instance or <c>null</c>.</returns>
        /// <remarks>Offset required for ExtFile header (offset specified before size).</remarks>
        [DebuggerStepThrough]
        public override T LoadCustom<T>(Func<T> callback, uint? offset = null)
        {
            offset = offset ?? ReadOffset();
            if (offset == 0) return default(T);

            using (TemporarySeek((int)offset, SeekOrigin.Begin))
            {
                return callback.Invoke();
            }
        }

        /// <summary>
        /// Reads a BFRES offset which is relative to itself, and returns the absolute address.
        /// </summary>
        /// <returns>The absolute address of the offset.</returns>
        public override uint ReadOffset()
        {
            uint offset = (uint)ReadUInt64();
            return offset == 0 ? 0 : offset;
        }

        public override uint ReadSize() => (uint)ReadUInt64();

        internal void LoadHeaderBlock()
        {
            uint offset = ReadUInt32();
            long size = ReadInt64();
        }

        public override string ReadString(Encoding encoding = null) {
            short size = ReadInt16();
            return this.ReadString(StringCoding.ZeroTerminated, encoding);
        }
    }
}
