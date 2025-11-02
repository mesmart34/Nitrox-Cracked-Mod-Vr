using System.Reflection;
using Nitrox.Vr;
using NitroxClient.GameLogic;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class Vehicle_OnPilotModeBegin_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((Vehicle t) => t.OnPilotModeBegin());

    public static void Prefix(Vehicle __instance)
    {
        Resolve<Vehicles>().BroadcastOnPilotModeChanged(__instance.gameObject, true);
        
        if (__instance is SeaMoth || __instance is Exosuit)
        {
            VrHud.OnEnterVehicle();
        }
    }
}
