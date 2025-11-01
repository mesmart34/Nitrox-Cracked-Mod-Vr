using System.Reflection;
using Nitrox.Vr;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class uGUI_UpdateLevelIdentifier_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => uGUI.UpdateLevelIdentifier());
    
    public static void Postfix(uGUI __instance)
    {
        if (VrCameraRig.Instance != null)
        {
            VrCameraRig.Instance.UpdateShowControllers();
        }
    }
}
