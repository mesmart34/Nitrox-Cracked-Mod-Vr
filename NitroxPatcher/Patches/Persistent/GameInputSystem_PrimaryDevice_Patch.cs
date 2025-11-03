using System.Reflection;
using Nitrox.Vr;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class GameInputSystem_PrimaryDevice_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Property((GameInputSystem t) => t.PrimaryDevice).GetGetMethod();
    
    public static bool Prefix(ref GameInput.Device __result)
    {
        if (Settings.IsVrEnabled)
        {
            __result = GameInput.Device.Controller;
        }
        return false;
    }
}
