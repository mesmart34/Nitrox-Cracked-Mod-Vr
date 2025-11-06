using System.Reflection;
using Nitrox.Vr;
using UnityEngine;
using UnityEngine.UI;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class Unity_GraphicRaycaster_VREventCamera_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = typeof(GraphicRaycaster)
                                                       .GetProperty("eventCamera", BindingFlags.Public | BindingFlags.Instance)
                                                       ?.GetGetMethod();
    
    public static bool Prefix(GraphicRaycaster __instance, ref Camera __result)
    {
        Canvas canvas = __instance.GetComponent<Canvas>();
        if (canvas == null)
        {
            return true;
        }
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera == null))
        {
            return true;
        }
        if (ControllerRig.Instance == null)
        {
            return true;
        }
        __result = ControllerRig.Instance.EventCamera;
        return false;
    }
}
