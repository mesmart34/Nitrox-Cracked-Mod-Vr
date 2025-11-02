using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Nitrox.Vr;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class Targeting_GetTarget_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => Targeting.GetTarget(default, default, out Reflect.Ref<GameObject>.Field, out Reflect.Ref<float>.Field));
    
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (CodeInstruction ins in instructions)
        {
            bool skipNext = false;
            PropertyInfo cameraPropInfo = typeof(MainCamera).GetProperty(nameof(MainCamera.camera));
            if (ins.Calls(cameraPropInfo?.GetGetMethod()))
            {
                skipNext = true;
                MethodInfo targetTransformGetter = typeof(VrCameraRig).GetMethod(nameof(VrCameraRig.GetTargetTransform));
                yield return new CodeInstruction(OpCodes.Call, targetTransformGetter);
            }
            else if (!skipNext)
            {
                yield return ins;
            }
        }
    }
}
