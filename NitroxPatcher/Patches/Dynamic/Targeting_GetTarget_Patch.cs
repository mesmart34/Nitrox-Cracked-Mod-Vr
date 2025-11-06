// using System.Collections.Generic;
// using System.Reflection;
// using System.Reflection.Emit;
// using HarmonyLib;
// using Nitrox.Vr;
// using UnityEngine;
// using UnityEngine.XR;
//
// namespace NitroxPatcher.Patches.Dynamic;
//
// public sealed partial class Targeting_GetTarget_Patch : NitroxPatch, IDynamicPatch
// {
//     // private static readonly MethodInfo TARGET_METHOD =
//     //     Reflect.Method(() => Targeting.GetTarget(default, default, out Reflect.Ref<GameObject>.Field, out Reflect.Ref<float>.Field));
//     
//     private static readonly MethodInfo TARGET_METHOD = AccessTools.Method(
//         typeof(Targeting), 
//         nameof(Targeting.GetTarget),
//         [typeof(GameObject), typeof(float), typeof(GameObject).MakeByRefType(), typeof(float).MakeByRefType()]
//     );
//     
//     public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//     {
//         MethodInfo cameraPropInfo = typeof(MainCamera).GetProperty(nameof(MainCamera.camera))!.GetGetMethod();
//         MethodInfo targetTransformGetter = typeof(ControllerRig).GetProperty(nameof(ControllerRig.Instance.EventCamera))!.GetGetMethod();
//         Log.Info($"try to get method info {cameraPropInfo}");
//         foreach (CodeInstruction ins in instructions)
//         {
//             if (ins.Calls(cameraPropInfo))
//             {
//                 Log.Info($"Try to set the event camera {targetTransformGetter}");
//                 yield return new CodeInstruction(OpCodes.Call, targetTransformGetter);
//             }
//             else
//             {
//                 yield return ins;
//             }
//         }
//         // foreach (CodeInstruction ins in instructions)
//         // {
//         //     bool skipNext = false;
//         //     // Look for `Transform transform = MainCamera.camera.transform;` in the first line of the method and replace it with our own
//         //     if (ins.Calls(typeof(MainCamera).GetProperty(nameof(MainCamera.camera)).GetGetMethod()))
//         //     {
//         //         // Skip next instruction which called/got transform from the main camera
//         //         skipNext = true;
//         //         MethodInfo targetTransformGetter = typeof(ControllerRig).GetProperty(nameof(ControllerRig.EventCamera))?.GetGetMethod();
//         //         yield return new CodeInstruction(OpCodes.Call, targetTransformGetter);
//         //     }
//         //     else if (skipNext)
//         //     {
//         //     }
//         //     else
//         //         yield return ins;
//         // }
//     }
// }
