// using System.Collections.Generic;
// using System.Reflection;
// using System.Reflection.Emit;
// using HarmonyLib;
// using UnityEngine.XR;
//
// namespace NitroxPatcher.Patches.Dynamic;
//
// public sealed partial class HandReticle_LateUpdate_Patch : NitroxPatch, IDynamicPatch
// {
//     private static readonly MethodInfo TARGET_METHOD = Reflect.Method((HandReticle t) => t.LateUpdate());
//
//     public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//     {
//         FieldInfo desiredIconField = AccessTools.Field(typeof(HandReticle), nameof(HandReticle.desiredIconType));
//         CodeMatcher matcher = new(instructions);
//
//         matcher.MatchForward(false, new[] {
//             new CodeMatch(OpCodes.Ldc_I4_1),
//             new CodeMatch(opc => opc.StoresField(desiredIconField))
//         }).SetOpcodeAndAdvance(OpCodes.Ldc_I4_0);
//
//         foreach (CodeInstruction ins in matcher.InstructionEnumeration())
//         {
//             if (ins.Calls(AccessTools.DeclaredPropertyGetter(typeof(XRSettings), nameof(XRSettings.enabled))))
//             {
//                 yield return new CodeInstruction(OpCodes.Ldc_I4_0);
//             }
//             else
//             {
//                 yield return ins;
//             }
//         }
//     }
//     
//     // public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//     // {
//     //     foreach (CodeInstruction ins in instructions)
//     //     {
//     //         if (ins.Calls(AccessTools.DeclaredPropertyGetter(typeof(XRSettings), nameof(XRSettings.enabled))))
//     //         {
//     //             yield return new CodeInstruction(OpCodes.Ldc_I4_0);
//     //         }
//     //         else
//     //         {
//     //             yield return ins;
//     //         }
//     //     }
//     // }
//     //
//     // public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//     // {
//     //     FieldInfo desiredIconField = AccessTools.Field(typeof(HandReticle), nameof(HandReticle.desiredIconType));
//     //     return new CodeMatcher(instructions).MatchForward(false, new CodeMatch[] {
//     //         // /* 0x0012B387 17           */ IL_015F: ldc.i4.1
//     //         // /* 0x0012B388 7DB5320004   */ IL_0160: stfld     valuetype HandReticle/IconType HandReticle::desiredIconType
//     //         new CodeMatch(OpCodes.Ldc_I4_1), // Store 1
//     //         new CodeMatch(opc => opc.StoresField(desiredIconField)),
//     //     }).SetOpcodeAndAdvance(OpCodes.Ldc_I4_0).InstructionEnumeration(); // Replace by 0
//     // }
// }
