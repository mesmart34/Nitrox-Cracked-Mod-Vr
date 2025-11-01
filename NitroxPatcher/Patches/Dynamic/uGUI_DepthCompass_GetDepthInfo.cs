using System.Reflection;
using HarmonyLib;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class uGUI_DepthCompass_GetDepthInfo : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = AccessTools.Method(typeof(uGUI_DepthCompass), "GetDepthInfo", [typeof(int).MakeByRefType(), typeof(int).MakeByRefType()]);
    
    public static void Postfix(ref uGUI_DepthCompass.DepthMode __result)
    {
        // if (!WristHud.isHudOn)
        // {
        //     __result = uGUI_DepthCompass.DepthMode.None;
        // }
    }
}
