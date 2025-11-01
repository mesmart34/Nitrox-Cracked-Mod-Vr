using System.Reflection;
using Nitrox.Vr;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class FPSInputModule_GetCursorScreenPosition_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((FPSInputModule t) => t.GetCursorScreenPosition());
    
    public static void Postfix(ref Vector2 __result, FPSInputModule __instance)
    {
        if (VrCameraRig.Instance == null || VrCameraRig.Instance.UIControllerCamera == null)
        {
            return;
        }

        Camera eventCamera = VrCameraRig.Instance.UIControllerCamera;
        __result = new Vector2(eventCamera.pixelWidth * 0.5f, eventCamera.pixelHeight * 0.5f);
    }
}
