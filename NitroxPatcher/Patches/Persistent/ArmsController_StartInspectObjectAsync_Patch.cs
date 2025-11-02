using System.Reflection;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class ArmsController_StartInspectObjectAsync_Patch : NitroxPatch, IPersistentPatch
{
    public static readonly MethodInfo TARGET_METHOD = Reflect.Method((ArmsController t) => t.StartInspectObjectAsync(default));

    public static bool Prefix(ArmsController __instance)
    {
        return false;
    }
}
