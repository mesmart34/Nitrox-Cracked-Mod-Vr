using System.Reflection;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class VROptions_GetUseGazeBasedCursor_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => VROptions.GetUseGazeBasedCursor());
    
    public static bool Prefix(ref bool __result)
    {
        __result = true;
        return false;
    }
}
