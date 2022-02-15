using System.Runtime.InteropServices;
using Rainmeter;

namespace SteamPlugin;

public static class SteamPlugin
{
    [DllExport]
    public static void Initialize(ref IntPtr data, IntPtr rm)
    {
        data = GCHandle.ToIntPtr(GCHandle.Alloc(new Measure()));
        var api = (API)rm;
    }

    [DllExport]
    public static void Finalize(IntPtr data)
    {
        var measure = (Measure)data;
        if (measure.Buffer != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(measure.Buffer);
        }
        GCHandle.FromIntPtr(data).Free();
    }

    [DllExport]
    public static void Reload(IntPtr data, IntPtr rm, ref double maxValue)
    {
        var measure = (Measure)data;
    }

    [DllExport]
    public static double Update(IntPtr data)
    {
        var measure = (Measure)data;
        return 0.0;
    }
}