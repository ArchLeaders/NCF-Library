using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nintendo.Aamp;
using Nintendo.Bfres;
using Nintendo.Byml;
using Nintendo.Byml.IO;
using Nintendo.Sarc;
using Nintendo.Yaz0;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace IOTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Aamp()
        {
            AampFile aamp = new(@"..\..\..\Data\IO.aamp");
            Trace.WriteLine(aamp.ToYml());
        }
        [TestMethod]
        public void Byml()
        {
            BymlFile byml = new(@"..\..\..\Data\IO.byml");
            Trace.WriteLine(byml.ToYaml());
        }
        [TestMethod]
        public void NewAamp()
        {
            AampFile aamp = AampFile.New(2);
            File.WriteAllBytes(@"..\..\..\Data\new.aamp", aamp.ToBinary());
        }
        [TestMethod]
        public void NewByml()
        {
            BymlFile byml = new(new Dictionary<string, BymlNode>());
            File.WriteAllBytes(@"..\..\..\Data\new.byml", byml.ToBinary());
        }
        [TestMethod]
        public void WriteByml()
        {
            BymlFile byml = new(@"..\..\..\Data\IO.byml");
            File.WriteAllBytes(@"..\..\..\Data\new.byml", byml.ToBinary());
        }
        [TestMethod]
        public void ActorInfoReadWriteTest()
        {
            BymlFile byml = new(@"E:\Users\chodn\Documents\ISOs - WiiU\The Legend of Zelda Breath of the Wild (UPDATE DATA) (v208) (USA)\content\Actor\ActorInfo.product.byml");
            for (int i = 0; i < 5; i++)
            {
                File.WriteAllBytes($@"E:\Users\chodn\Documents\ISOs - WiiU\The Legend of Zelda Breath of the Wild (UPDATE DATA) (v208) (USA)\content\Actor\ActorInfo.test.{i}.product.byml", byml.ToBinary());
                byml = new($@"E:\Users\chodn\Documents\ISOs - WiiU\The Legend of Zelda Breath of the Wild (UPDATE DATA) (v208) (USA)\content\Actor\ActorInfo.test.{i}.product.byml");
            }
        }
        [TestMethod]
        public void ActorInfoYamlTest()
        {
            BymlFile byml = new(@"E:\Users\chodn\Documents\ISOs - WiiU\The Legend of Zelda Breath of the Wild (UPDATE DATA) (v208) (USA)\content\Actor\ActorInfo.test.0.product.byml");
            Trace.WriteLine(byml.ToYaml());
        }
        [TestMethod]
        public void BymlEqualityCompare()
        {
            BymlNode iFive = new BymlNode(5);
            BymlNode fFive = new BymlNode(5f);
            BymlNode list1 = new BymlNode(new List<BymlNode>() { iFive, fFive });
            BymlNode list2 = new BymlNode(new List<BymlNode>() { iFive, fFive });
            BymlNode list3 = new BymlNode(new List<BymlNode>() { iFive });
            BymlNode list4 = new BymlNode(new List<BymlNode>() { fFive });
            BymlNode hash1 = new BymlNode(new Dictionary<string, BymlNode>() { { "iFive", iFive }, { "fFive", fFive } });
            BymlNode hash2 = new BymlNode(new Dictionary<string, BymlNode>() { { "iFive", iFive }, { "fFive", fFive } });
            BymlNode hash3 = new BymlNode(new Dictionary<string, BymlNode>() { { "fFive", fFive } });
            BymlNode hash4 = new BymlNode(new Dictionary<string, BymlNode>() { { "iFive", iFive } });
            BymlNode uFive = new BymlNode(5u);
            BymlNode ulFive = new BymlNode(5ul);
            BymlNode iFive2 = new BymlNode(5);

            Trace.WriteLine($"iFive.Equals(fFive): {iFive.Equals(fFive)} false");
            Trace.WriteLine($"iFive.Equals(iFive2): {iFive.Equals(iFive2)} true");
            Trace.WriteLine($"list1.Equals(list2): {list1.Equals(list2)} true");
            Trace.WriteLine($"list2.Equals(list3): {list2.Equals(list3)} false");
            Trace.WriteLine($"list3.Equals(list4): {list3.Equals(list4)} false");
            Trace.WriteLine($"hash1.Equals(hash2): {hash1.Equals(hash2)} true");
            Trace.WriteLine($"hash2.Equals(hash3): {hash2.Equals(hash3)} false");
            Trace.WriteLine($"hash3.Equals(hash4): {hash3.Equals(hash4)} false");
            Trace.WriteLine($"uFive.Equals(iFive): {uFive.Equals(iFive)} false");
            Trace.WriteLine($"uFive.Equals(ulFive): {uFive.Equals(ulFive)} false");

            Trace.WriteLine($"iFive == fFive: {iFive == fFive} false");
            Trace.WriteLine($"iFive == iFive2: {iFive == iFive2} true");
            Trace.WriteLine($"list1 == list2: {list1 == list2} true");
            Trace.WriteLine($"list2 == list3: {list2 == list3} false");
            Trace.WriteLine($"list3 == list4: {list3 == list4} false");
            Trace.WriteLine($"hash1 == hash2: {hash1 == hash2} true");
            Trace.WriteLine($"hash2 == hash3: {hash2 == hash3} false");
            Trace.WriteLine($"hash3 == hash4: {hash3 == hash4} false");
            Trace.WriteLine($"uFive == iFive: {uFive == iFive} false");
            Trace.WriteLine($"uFive == ulFive: {uFive == ulFive} false");
        }
    }
}
