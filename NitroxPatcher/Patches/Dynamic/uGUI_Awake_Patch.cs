// using System.Reflection;
// using Nitrox.Vr;
//
// namespace NitroxPatcher.Patches.Dynamic;
//
// public sealed partial class uGUI_Awake_Patch : NitroxPatch, IDynamicPatch
// {
//     public static readonly MethodInfo TARGET_METHOD = Reflect.Method((uGUI t) => t.Awake());
//     
//     public static void Prefix(uGUI __instance)
//     {
//         __instance.gameObject.AddComponent<DebugPanel>();
//     }
// }
