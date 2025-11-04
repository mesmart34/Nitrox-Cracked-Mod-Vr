using System;
using System.Collections.Generic;
using System.Reflection;
using Nitrox.Vr;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NitroxPatcher.Patches.Persistent
{
    public sealed partial class FPSInputModule_UpdateMouseState_Patch : NitroxPatch, IPersistentPatch
    {
        private static readonly MethodInfo TARGET_METHOD = Reflect.Method((FPSInputModule t) => t.UpdateMouseState(default));
        
        public static void Prefix(FPSInputModule __instance, PointerEventData leftData)
        {
            Vector3? position = null;
            
            if (leftData != null && __instance.lastRaycastResult.isValid)
            {
                leftData.position = __instance.lastRaycastResult.worldPosition;
                position = __instance.lastRaycastResult.worldPosition;
            }

            if (LaserPointer.Instance != null)
            {
                LaserPointer.Instance.SetPointerTarget(position);
            }
        }
    }
}
