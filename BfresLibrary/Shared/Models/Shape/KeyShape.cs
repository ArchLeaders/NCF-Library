using Nintendo.Bfres.Core;

namespace Nintendo.Bfres
{
    public class KeyShape : IResData
    {
        // ---- PROPERTIES ---------------------------------------------------------------------------------------------

        public byte[] TargetAttribIndices { get; set; }

        public byte[] TargetAttribIndexOffsets { get; set; }

        // ---- METHODS ------------------------------------------------------------------------------------------------

        void IResData.Load(ResFileLoader loader)
        {
            TargetAttribIndices = loader.ReadBytes(20);
            TargetAttribIndexOffsets = loader.ReadBytes(4);
        }
        
        void IResData.Save(ResFileSaver saver)
        {
            saver.SaveCustom(TargetAttribIndices, () => saver.Write(TargetAttribIndices));
            saver.SaveCustom(TargetAttribIndexOffsets, () => saver.Write(TargetAttribIndexOffsets));
        }
    }
}