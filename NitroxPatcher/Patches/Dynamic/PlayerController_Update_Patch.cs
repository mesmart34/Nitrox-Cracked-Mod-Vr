using System.Reflection;
using Nitrox.Vr;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class PlayerController_Update_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((Player t) => t.Update());
    
    public static void Postfix(PlayerController __instance)
    {
        ControllerRig.Instance.transform.position = __instance.transform.position;
        ControllerRig.Instance.transform.rotation = __instance.transform.rotation;
        
        // Log.Info($"Player position: {__instance.transform.position}");
        // Log.Info($"ControllerRig: {ControllerRig.Instance.transform.position}");
        // ControllerRig.Instance.Hands.ForEach(x => Log.Info($"Hand [{x.Key}]: {x.Value.gameObject.layer}"));
        // Log.Info($"-----------------------------------------");
    }
}
