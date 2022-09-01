﻿using System;
using System.Linq;
using System.Diagnostics;
using System.Text;
using BntxLibrary.Core;

namespace BntxLibrary.Common
{
    /// <summary>
    /// Represents custom user variables which can be attached to many sections and subfiles of a <see cref="ResFile"/>.
    /// </summary>
    [DebuggerDisplay(nameof(UserData) + " {" + nameof(Name) + "}")]
    public class UserData : IResData
    {
        public UserData()
        {
            Name = "";
            SetValue(new int[] { 0 });
        }

        // ---- FIELDS -------------------------------------------------------------------------------------------------

        private object _value;

        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        /// <summary>
        /// Gets or sets the name with which the instance can be referenced uniquely in <see cref="ResDict{UserData}"/>
        /// instances.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The data type of the stored values.
        /// </summary>
        public UserDataType Type { get; private set; }

        // ---- METHODS (PUBLIC) ---------------------------------------------------------------------------------------

        /// <summary>
        /// Returns the stored value as an array of <see cref="Int32"/> instances when the <see cref="Type"/> is
        /// <see cref="UserDataType.Int32"/>.
        /// </summary>
        /// <returns>The typed value.</returns>
        public int[] GetValueInt32Array()
        {
            return (int[])_value;
        }

        /// <summary>
        /// Returns the stored value as an array of <see cref="Single"/> instances when the <see cref="Type"/> is
        /// <see cref="UserDataType.Single"/>.
        /// </summary>
        /// <returns>The typed value.</returns>
        public float[] GetValueSingleArray()
        {
            return (float[])_value;
        }

        /// <summary>
        /// Returns the stored value as an array of <see cref="String"/> instances when the <see cref="Type"/> is
        /// <see cref="UserDataType.String"/> or <see cref="UserDataType.WString"/>.
        /// </summary>
        /// <returns>The typed value.</returns>
        public string[] GetValueStringArray()
        {
            if (_value == null || Type != UserDataType.String && Type != UserDataType.WString)
                return new string[0];

            return (string[])_value;
        }

        /// <summary>
        /// Returns the stored value as an array of <see cref="Byte"/> instances when the <see cref="Type"/> is
        /// <see cref="UserDataType.Byte"/>.
        /// </summary>
        /// <returns>The typed value.</returns>
        public byte[] GetValueByteArray()
        {
            return (byte[])_value;
        }

        /// <summary>
        /// Sets the stored <paramref name="value"/> as an <see cref="Int32"/> array and the <see cref="Type"/> to
        /// <see cref="UserDataType.Int32"/>
        /// </summary>
        /// <param name="value">The value to store.</param>
        public void SetValue(int[] value)
        {
            Type = UserDataType.Int32;
            _value = value;
        }

        /// <summary>
        /// Sets the stored <paramref name="value"/> as a <see cref="Single"/> array and the <see cref="Type"/> to
        /// <see cref="UserDataType.Single"/>
        /// </summary>
        /// <param name="value">The value to store.</param>
        public void SetValue(float[] value)
        {
            Type = UserDataType.Single;
            _value = value;
        }

        /// <summary>
        /// Sets the stored <paramref name="value"/> as a <see cref="String"/> array and the <see cref="Type"/> to
        /// <see cref="UserDataType.String"/> or <see cref="UserDataType.WString"/> depending on
        /// <paramref name="asUnicode"/>.
        /// </summary>
        /// <param name="asUnicode"><c>true</c> to store data as UTF-16 encoded strings, or <c>false</c> to store it
        /// as ASCII encoded strings.</param>
        /// <param name="value">The value to store.</param>
        public void SetValue(string[] value, bool asUnicode = false)
        {
            Type = asUnicode ? UserDataType.WString : UserDataType.String;
            _value = value;
        }

        /// <summary>
        /// Sets the stored <paramref name="value"/> as a <see cref="Byte"/> array and the <see cref="Type"/> to
        /// <see cref="UserDataType.Byte"/>
        /// </summary>
        /// <param name="value">The value to store.</param>
        public void SetValue(byte[] value)
        {
            Type = UserDataType.Byte;
            _value = value;
        }


        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IResData.Load(BntxFileLoader loader)
        {
            Name = loader.LoadString();
            long DataOffset = loader.ReadOffset(true);
            uint count = loader.ReadUInt32();
            Type = loader.ReadEnum<UserDataType>(true);

            char[] Reserved = System.Text.Encoding.ASCII.GetString(loader.ReadBytes(43)).ToCharArray();
            switch (Type)
            {
                case UserDataType.Byte:
                    _value = loader.LoadCustom(() => loader.ReadSBytes((int)count), DataOffset);
                    break;
                case UserDataType.Int32:
                    _value = loader.LoadCustom(() => loader.ReadInt32s((int)count), DataOffset);
                    break;
                case UserDataType.Single:
                    _value = loader.LoadCustom(() => loader.ReadSingles((int)count), DataOffset);
                    break;
                case UserDataType.String:
                    _value = loader.LoadCustom(() => loader.LoadStrings((int)count, Encoding.UTF8), DataOffset);
                    break;
                case UserDataType.WString:
                    _value = loader.LoadCustom(() => loader.LoadStrings((int)count, Encoding.Unicode), DataOffset);
                    break;
            }
        }
        bool ContainsUnicodeCharacter(object input)
        {
            const int MaxAnsiCode = 255;

            if (input is string[])
            {
                foreach (string inp in (string[])input)
                {
                    bool IsUni = inp.Any(c => c > MaxAnsiCode);

                    if (IsUni == true)
                        return true;
                }
            }

            return false;
        }

        internal void SaveData(BntxFileSaver saver)
        {
            switch (Type)
            {
                case UserDataType.Int32:
                    saver.Write((int[])_value);
                    break;
                case UserDataType.Single:
                    saver.Write((float[])_value);
                    break;
                case UserDataType.String:
                    saver.SaveStrings((string[])_value, Encoding.UTF8);
                    break;
                case UserDataType.WString:
                    saver.SaveStrings((string[])_value, Encoding.Unicode);
                    break;
                case UserDataType.Byte:
                    saver.Write((byte[])_value);
                    break;
            }
        }

        internal long DataOffset;

        void IResData.Save(BntxFileSaver saver)
        {
            saver.SaveString(Name);
            DataOffset = saver.SaveOffset();
            saver.Write(((Array)_value).Length); // Unsafe cast, but _value should always be Array.
            saver.WriteEnum(Type, true);
            saver.Seek(43);
        }
    }

    /// <summary>
    /// Represents the possible data types of values stored in <see cref="UserData"/> instances.
    /// </summary>
    public enum UserDataType : byte
    {
        /// <summary>
        /// The values is an <see cref="Int32"/> array.
        /// </summary>
        Int32,

        /// <summary>
        /// The values is a <see cref="Single"/> array.
        /// </summary>
        Single,

        /// <summary>
        /// The values is a <see cref="String"/> array encoded in ASCII.
        /// </summary>
        String,

        /// <summary>
        /// The values is a <see cref="Byte"/> array.
        /// </summary>
        Byte,

        /// <summary>
        /// The values is a <see cref="String"/> array encoded in UTF-16.
        /// </summary>
        WString,
    }
}
