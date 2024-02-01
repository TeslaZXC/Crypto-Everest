using UnityEngine;
using System.Runtime.InteropServices;

public class RBDeviceType
{
#if UNITY_EDITOR
    public static bool isMobile()
    {
        return false;
    }
#else
    [DllImport("__Internal")]
    public static extern bool isMobile();
#endif
}
