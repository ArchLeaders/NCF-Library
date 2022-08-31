namespace BntxLibrary.Shared
{
    internal static class Extensions
    {
        public static int ToInt(this decimal _decimal) => Convert.ToInt32(_decimal);
        public static int ToInt(this uint _uint) => Convert.ToInt32(_uint);
        public static uint ToUInt(this decimal _decimal) => Convert.ToUInt32(_decimal);
        public static bool In(this object obj, params object[] objects) => objects.Contains(obj);

        public static uint FloorFunction(object value) => Math.Floor(Convert.ToDecimal(value)).ToUInt();
        public delegate uint FloorAction(object value);
        public static FloorAction Floor = FloorFunction;
    }
}
