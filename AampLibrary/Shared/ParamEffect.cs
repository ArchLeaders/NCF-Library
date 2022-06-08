using System.Collections.Generic;

namespace Nintendo.Aamp.Shared
{
    internal static class ParamEffect
    {
        private static Dictionary<string, string> EffectTypes { get; } = new()
        {
            { "aglenv", "Environment" },
            { "glpbd", "Probe Data" },
            { "genv", "Environment" },
            { "gsdw", "Shadow" },
            { "agllmap", "Light Map" },
            { "agldof", "Depth of Field" },
            { "aglfila", "AA Filter" },
            { "aglblm", "Bloom" },
            { "aglccr", "Color Correction" },
            { "aglcube", "Cube Map" },
            { "aglatex", "Auto Dxposure" },
            { "aglflr", "Flare Filter" },
            { "aglmf", "Multi Filter" },
            { "aglsdw", "Depth Shadow" },
            { "aglshpp", "Shadow Pre Pass" },
            { "aglofx", "Occluded Effect" },
        };

        internal static string GetEffectType(this string pioType)
        {
            // Set default effect type
            string effectType = "";

            // Iterate effect types
            foreach (var effectTypeIter in EffectTypes)
                if (pioType.Contains(effectTypeIter.Key))
                    effectType = effectTypeIter.Value;

            return effectType;
        }
    }
}
