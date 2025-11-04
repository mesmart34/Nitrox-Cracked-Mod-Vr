using System.Reflection;
using Nitrox.Vr;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class ArmsController_Start_Patch : NitroxPatch, IDynamicPatch
{
    public static readonly MethodInfo TARGET_METHOD = Reflect.Method((ArmsController t) => t.Start());

    public static void Postfix(ArmsController __instance)
    {
        __instance.Reconfigure(null);

        ControllerRig rig = ControllerRig.Instance;
        if (rig == null)
        {
            return;
        }
        
        rig.transform.position = __instance.transform.position;
        rig.transform.rotation = __instance.transform.rotation;
    }
}
