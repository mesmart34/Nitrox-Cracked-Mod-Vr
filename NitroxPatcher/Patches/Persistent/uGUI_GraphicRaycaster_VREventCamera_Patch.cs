using System.Reflection;
using Nitrox.Vr;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class uGUI_GraphicRaycaster_VREventCamera_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = typeof(uGUI_GraphicRaycaster)
                                                       .GetProperty("eventCamera", BindingFlags.Public | BindingFlags.Instance)
                                                       ?.GetGetMethod();
    
    public static bool Prefix(uGUI_GraphicRaycaster __instance, ref Camera __result)
    {
        if (ControllerRig.Instance == null)
        {
            return true;
        }
        
        if (!(SNCameraRoot.main != null) || __instance.guiCameraSpace)
        {
            __result = ControllerRig.Instance.EventCamera;
        }
        else
        {
            __result = ControllerRig.Instance.EventCamera;//world
        }
        
        return false;
    }
}
