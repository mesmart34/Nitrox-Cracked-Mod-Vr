using System.Reflection;
using HarmonyLib;
using Nitrox.Vr;
using UnityEngine;
using UnityEngine.UI;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class Unity_GraphicRaycaster_VREventCamera_Patch : NitroxPatch, IPersistentPatch
{
    // private static readonly MethodInfo TARGET_METHOD = typeof(GraphicRaycaster)
    //                                                    .GetProperty("eventCamera", BindingFlags.Public | BindingFlags.Instance)
    //                                                    ?.GetGetMethod();
    
    private static readonly MethodInfo TARGET_METHOD = AccessTools.PropertyGetter(typeof(GraphicRaycaster), "eventCamera");
    
    public static bool Prefix(GraphicRaycaster __instance, ref Camera __result)
    {
        // TODO: Clean this up
        Canvas canvas = __instance.GetComponent<Canvas>();
        if (canvas == null)
        {
            return true;
        }
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
        {
            return true;
        }
        if (VrCameraRig.Instance == null)
        {
            return true;
        }
        __result = VrCameraRig.Instance.WorldControllerCamera;
        return false;
    }
}
