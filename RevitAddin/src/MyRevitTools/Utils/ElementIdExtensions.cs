using Autodesk.Revit.DB;

namespace MyRevitTools.Utils
{
    /// <summary>
    /// Revit 2024 me ElementId 32-bit se 64-bit ho gaya.
    /// Hamesha ye helper use karo, seedha IntegerValue / Value nahi.
    /// </summary>
    public static class ElementIdExtensions
    {
        public static long GetIdValue(this ElementId id)
        {
#if REVIT2024_OR_GREATER
            return id.Value;
#else
            return id.IntegerValue;
#endif
        }

        public static ElementId ToElementId(this long value)
        {
#if REVIT2024_OR_GREATER
            return new ElementId(value);
#else
            return new ElementId((int)value);
#endif
        }
    }
}
