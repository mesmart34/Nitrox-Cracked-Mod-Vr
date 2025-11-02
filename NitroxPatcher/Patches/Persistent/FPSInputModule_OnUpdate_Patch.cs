using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine.EventSystems;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class FPSInputModule_OnUpdate_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((FPSInputModule t) => t.OnUpdate());

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        MethodInfo getter = AccessTools.DeclaredPropertyGetter(typeof(BaseInputModule), "eventSystem");
        // Snippet to print each instruction before removal
        // instructions.ForEach(ins => {
        // Mod.logger.LogInfo($"   : {ins}");
        // });
        CodeMatcher m = new CodeMatcher(instructions);
        CodeMatcher mc = m.Clone();

        CodeMatcher patched = m.MatchForward(false, new CodeMatch[] {
            /* Match and remove the following code from FPSInputModule.OnUpdate():
            if (!base.eventSystem.isFocused || TouchScreenKeyboardManager.visible)
            {
                return;
            }
            */
            // /* 0x00129138 02           */ IL_0000: ldarg.0
            // /* 0x00129139 287916000A   */ IL_0001: call      instance class [UnityEngine.UI]UnityEngine.EventSystems.EventSystem [UnityEngine.UI]UnityEngine.EventSystems.BaseInputModule::get_eventSystem()
            // /* 0x0012913E 6F7A16000A   */ IL_0006: callvirt  instance bool [UnityEngine.UI]UnityEngine.EventSystems.EventSystem::get_isFocused()
            // /* 0x00129143 2C07         */ IL_000B: brfalse.s IL_0014
            // /* 0x00129145 287B16000A   */ IL_000D: call      bool [Unity.TextMeshPro]TouchScreenKeyboardManager::get_visible()
            // /* 0x0012914A 2C01         */ IL_0012: brfalse.s IL_0015
            // /* 0x0012914C 2A           */ IL_0014: ret
            new CodeMatch(OpCodes.Ldarg_0),
            new CodeMatch(OpCodes.Call),
            new CodeMatch(OpCodes.Callvirt),
        }).ThrowIfInvalid("Could not find target").RemoveInstructions(7);

        return patched.InstructionEnumeration();
    }
}
