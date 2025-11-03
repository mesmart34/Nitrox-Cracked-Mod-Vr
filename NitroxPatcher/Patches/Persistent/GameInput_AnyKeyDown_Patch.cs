extern alias SteamVRRef;
using System.Reflection;
using Nitrox.Vr;
using SteamVRRef::Valve.VR;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class GameInput_AnyKeyDown_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Property((() => GameInput.AnyKeyDown)).GetMethod;

    public static void Prefix(ref bool __result)
    {
        if (__result)
        {
            return;
        }

        foreach (SteamVR_Action_Boolean action in SteamVR_Input.actionsBoolean)
        {
            if (action.GetStateDown(SteamVR_Input_Sources.Any))
            {
                __result = true;
                break;
            }
        }
    }
}
