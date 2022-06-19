namespace Nintendo.Byml
{
    /// <summary>
    /// Represents the type of which a dynamic BYML node can be.
    /// </summary>
    public enum NodeType : byte
	{
        None,
        StringIndex = 0xA0,
		PathIndex = 0xA1,
		Array = 0xC0,
		Dictionary = 0xC1,
		StringArray = 0xC2,
		PathArray = 0xC3,
		Boolean = 0xD0,
		Integer = 0xD1,
		Float = 0xD2,
		Uinteger = 0xD3,
		Long = 0xD4,
		ULong = 0xD5,
		Double = 0xD6,
		Null = 0xFF
	}
}
