using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine.XR;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class ManagedCanvasUpdate_GetUICamera_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => ManagedCanvasUpdate.GetUICamera());
    
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions).MatchForward(false, new CodeMatch[] {
            new CodeMatch(ci => ci.Calls(typeof(XRDevice).GetMethod(nameof(XRDevice.DisableAutoXRCameraTracking))))
        }).ThrowIfNotMatch("Could not find XRDevice Deactivation").Advance(-2).RemoveInstructions(3).InstructionEnumeration();
    }
}
