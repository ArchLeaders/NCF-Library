using Aamp.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Nintendo.Aamp
{
    public class Hashes
    {
        // From https://github.com/zeldamods/aamp
        // Copyright 2018 leoetlino <leo@leolam.fr>
        // Licensed under GPLv2+
        // https://github.com/zeldamods/aamp/blob/master/LICENSE

        private static Dictionary<uint, string> hashName = new();
        public static Dictionary<uint, string> HashName { get => hashName; set => hashName = value; }

        public static bool HasString(string name) => hashName.ContainsKey(Crc32.Compute(name));
        private static void CheckHash(string hashStr)
        {
            uint hash = Crc32.Compute(hashStr);
            hashName.TryAdd(hash, hashStr);
        }

        private static void GenerateHashes()
        {
            foreach (string hashStr in new Resource("Data.AampHashedNames").ToString().Split('\n'))
            {
                SetNumberedString(hashStr, 6);
                CheckHash(hashStr);
            }
            foreach (string hashStr in new Resource("Data.AampHashedNamesNumbered").ToString().Split('\n'))
            {
                string[] strArray = GeneratedNumberHashes(hashStr);
                if (strArray.Length > 0) {
                    foreach (string strHash in strArray)
                        CheckHash(strHash);
                }
                else {
                    CheckHash(hashStr);
                }
            }
            
            SetNumberedString("PointLightRig", 50);
            SetNumberedString("SpotLightRig", 50);
            SetNumberedString("AI_", 1000);
            SetNumberedString("Action_", 1000);
            SetNumberedString("HemisphereLight", 30);
            SetNumberedString("Fog", 30);
            SetNumberedString("DirectionalLight", 30);
            SetNumberedString("BloomObj", 30);
            SetNumberedString("OfxLargeLensFlareRig", 30);
            SetNumberedString("name", 50);
            SetNumberedString("intensity", 50);
            SetNumberedString("connection_curve_", 100);
            SetNumberedString("bone_", 100);
            SetNumberedString("output_single_", 100);
            SetNumberedString("support_bone_", 100);
        }

        private static void SetNumberedString(string hashStr, int Amount)
        {
            for (int i = 0; i < Amount; i++)
                CheckHash($"{hashStr}{i}");
        }

        public static string[] GeneratedNumberHashes(string hash)
        {
            if (!hash.Contains('%'))
                return Array.Empty<string>();
            string[] strArray = new string[6];
            Regex r = new(@"%(\d*)d");
            string match = r.Match(hash).Groups[1].Value;
            int count = match == "" ? 0 : int.Parse(match);
            for (int index = 0; index < 6; ++index)
                strArray[index] = r.Replace(hash, new MatchEvaluator(m => index.ToString().PadLeft(count, '0')));
            return strArray;
        }

        public static uint SetName(string Name) => Crc32.Compute(Name);

        public static string GetName(uint hash)
        {
            if (hashName.Count == 0)
                GenerateHashes();

            if (!hashName.TryGetValue(hash, out string? name))
                return hash.ToString();

            return name;
        }
    }
}
