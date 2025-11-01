using System.Reflection;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class IngameMenu_Awake_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((IngameMenu t) => t.Awake());
    
    public static void Postfix(IngameMenu __instance)
    {
        uGUI_CanvasScaler scalar = __instance.GetComponent<uGUI_CanvasScaler>();
        scalar.vrMode = uGUI_CanvasScaler.Mode.Static;
    }
}
