using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nintendo.Aamp;
using Nintendo.Bfres;
using Nintendo.Byml;
using Nintendo.Sarc;
using Nintendo.Yaz0;
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
            BymlFile byml = new(NodeType.Dictionary);
            File.WriteAllBytes(@"..\..\..\Data\new.byml", byml.ToBinary());
        }
    }
}
