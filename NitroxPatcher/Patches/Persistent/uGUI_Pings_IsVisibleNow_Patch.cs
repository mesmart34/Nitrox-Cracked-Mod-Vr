using System.Reflection;
using Nitrox.Vr;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class uGUI_Pings_IsVisibleNow_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((uGUI_Pings t) => t.IsVisibleNow());
    
    public static void Postfix(ref bool __result)
    {
        __result &= WristHud.isHudOn;
    }
}
