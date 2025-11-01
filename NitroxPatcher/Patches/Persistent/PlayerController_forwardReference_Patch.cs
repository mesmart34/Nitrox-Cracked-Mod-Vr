using System.Reflection;
using Nitrox.Vr;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class PlayerController_forwardReference_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Property((PlayerController t) => t.forwardReference).GetGetMethod();
    
    private static Transform controllerTransform;
    
    public static bool Prefix(PlayerController __instance, ref Transform __result)
    {
        if (Settings.HandBasedTurning)
        {
            //Use the Camera's position and the laser pointer's rotation
            //Use a dummy object to hold the transform
            if (controllerTransform == null)
            {
                controllerTransform = new GameObject().transform;
            }
            controllerTransform.position = MainCamera.camera.transform.position;
            controllerTransform.rotation = Settings.LeftHandBasedTurning ? VrCameraRig.GetLeftTargetTransform()!.rotation : VrCameraRig.GetTargetTransform()!.rotation;
            __result = controllerTransform;
        }
        else
        {
            __result = MainCamera.camera.transform;
        }
        return false;
    }
}
