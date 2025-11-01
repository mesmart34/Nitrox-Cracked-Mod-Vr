using System.Reflection;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class GameInput_IsPrimaryDeviceGamepad_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => GameInput.IsPrimaryDeviceGamepad());
    
    public static bool Prefix(ref bool __result)
    {
        __result = true;
        return true;
    }
}
