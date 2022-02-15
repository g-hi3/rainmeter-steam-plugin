using System.Runtime.InteropServices;

namespace Rainmeter;

public class Measure
{
    public static implicit operator Measure(IntPtr data)
    {
        return (Measure)GCHandle.FromIntPtr(data).Target!;
    }
    
    public IntPtr Buffer;
}