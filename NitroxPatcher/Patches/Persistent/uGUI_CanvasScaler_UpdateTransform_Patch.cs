using System.Reflection;
using Nitrox.Vr;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class uGUI_CanvasScaler_UpdateTransform_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((uGUI_CanvasScaler t) => t.UpdateTransform(default(Camera)));
    
    public static void Postfix(uGUI_CanvasScaler __instance)
    {
        // TODO: There gotta be a better way to attach this only to the PDA, maybe custom behaviour, disabling the Scalar?
        if (__instance.gameObject.GetComponent<uGUI_PDA>() == null)
        {
            return;
        }
        if (VrCameraRig.Instance == null)
        {
            return;
        }
        
        Transform rigWorldPos = SNCameraRoot.main.transform;

        Vector3 worldPos = __instance._anchor.transform.position;
        Quaternion worldRot = __instance._anchor.transform.rotation;
        Vector3 uiSpacePos = worldPos - rigWorldPos.position;
        Quaternion uiSpaceRotation = worldRot;
        
        __instance.rectTransform.position = uiSpacePos;
        __instance.rectTransform.rotation = uiSpaceRotation;
    }
}
