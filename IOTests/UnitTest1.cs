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
            File.WriteAllBytes(@"E:\Users\chodn\Documents\ISOs - WiiU\The Legend of Zelda Breath of the Wild (UPDATE DATA) (v208) (USA)\content\Actor\ActorInfo.test.product.byml", byml.ToBinary());
        }
        [TestMethod]
        public void ActorInfoYamlTest()
        {
            BymlFile byml = new(@"E:\Users\chodn\Documents\ISOs - WiiU\The Legend of Zelda Breath of the Wild (UPDATE DATA) (v208) (USA)\content\Actor\ActorInfo.product.byml");
            Trace.WriteLine(byml.ToYaml());
        }
    }
}
