using System.Reflection;
using HarmonyLib;
using Nitrox.Vr;
using UnityEngine;
using UnityEngine.UI;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class uGUI_GraphicRaycaster_VREventCamera_Patch : NitroxPatch, IPersistentPatch
{
    // private static readonly MethodInfo TARGET_METHOD = typeof(uGUI_GraphicRaycaster)
    //                                                    .GetProperty("eventCamera", BindingFlags.Public | BindingFlags.Instance)
    //                                                    ?.GetGetMethod();

    private static readonly MethodInfo TARGET_METHOD = AccessTools.PropertyGetter(typeof(uGUI_GraphicRaycaster), "eventCamera");

    
    public static bool Prefix(uGUI_GraphicRaycaster __instance, ref Camera __result)
    {
        if (VrCameraRig.Instance == null)
        {
            return true;
        }
        
        if (!(SNCameraRoot.main != null) || __instance.guiCameraSpace)
        {
            __result = VrCameraRig.Instance.UIControllerCamera;
        }
        else
        {
            __result = VrCameraRig.Instance.WorldControllerCamera;
        }
        
        return false;
    }
}
