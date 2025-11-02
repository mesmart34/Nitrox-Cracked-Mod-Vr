using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine.XR;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class HandReticle_LateUpdate_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((HandReticle t) => t.LateUpdate());
    
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        foreach (CodeInstruction ins in instructions)
        {
            if (ins.Calls(AccessTools.DeclaredPropertyGetter(typeof(XRSettings), nameof(XRSettings.enabled))))
            {
                yield return new CodeInstruction(OpCodes.Ldc_I4_0);
            }
            else
            {
                yield return ins;
            }
        }
    }
}
