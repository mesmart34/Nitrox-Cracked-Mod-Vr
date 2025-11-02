using System.Reflection;
using Nitrox.Vr;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class uGUI_PinnedRecipes_GetMode_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((uGUI_PinnedRecipes t) => t.GetMode());
    
    public static void Postfix(ref uGUI_PinnedRecipes.Mode __result)
    {
        if (!WristHud.isHudOn)
        {
            __result = uGUI_PinnedRecipes.Mode.Off;
        }
    }
}
