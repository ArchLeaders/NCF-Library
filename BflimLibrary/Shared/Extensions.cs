namespace BntxLibrary.Shared
{
    internal static class Extensions
    {
        public static int ToInt(this decimal _decimal) => Convert.ToInt32(_decimal);
        public static int ToInt(this uint _uint) => Convert.ToInt32(_uint);
        public static uint ToUInt(this decimal _decimal) => Convert.ToUInt32(_decimal);

        public static int FloorFunction(dynamic value) => decimal.Floor(value).ToInt();
        public delegate int FloorAction(dynamic value);
        public static FloorAction Floor = FloorFunction;
    }
}
