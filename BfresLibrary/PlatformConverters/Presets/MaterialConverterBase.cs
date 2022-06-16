namespace Nintendo.Bfres.PlatformConverters
{
    internal class MaterialConverterBase
    {
        internal virtual void ConvertToWiiUMaterial(Material material) { }
        internal virtual void ConvertToSwitchMaterial(Material material) { }
        internal string RenderInfoBoolString(bool value) => value ? "true" : "false";
    }
}
