using System.Reflection;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class GamepadInputModule_IsInputAllowed_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((GamepadInputModule t) => t.IsInputAllowed());

    public static bool Prefix(ref bool __result)
    {
        __result = !WaitScreen.IsWaiting; // && Application.isFocused;
        // __result = true; // && Application.isFocused;
        return false;
    }
}
