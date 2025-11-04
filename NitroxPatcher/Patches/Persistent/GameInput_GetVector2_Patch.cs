extern alias SteamVRActions;
extern alias SteamVRRef;
using System.Reflection;
using Nitrox.Vr;
using SteamVRActions::Valve.VR;
using SteamVRRef::Valve.VR;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

extern alias SteamVRRef;

public sealed partial class GameInput_GetVector2_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => GameInput.GetVector2(default));

    public static bool Prefix(GameInput.Button action, ref Vector2 __result)
    {
        Vector2 vec = Vector2.zero;
        if (SteamHelper.InputLocked || !SteamHelper.Ready)
        {
            return false;
        }

        switch (action)
        {
            case GameInput.Button.Look:
                vec = SteamVR_Actions.subnautica.Look.GetAxis(SteamVR_Input_Sources.Any);
                
                Vector2 sensitivity = new(0.405f, 0.405f);

                float mag = vec.magnitude;
                Vector2 normVec = ((mag > 0f) ? (vec / mag) : Vector2.zero);
                mag = UnityEngine.Mathf.Pow(mag, 2f) * 500f;
                vec = normVec * mag;
                vec *= sensitivity * Time.deltaTime;
                break;
            case GameInput.Button.Move:
                vec = SteamVR_Actions.subnautica.Move.GetAxis(SteamVR_Input_Sources.Any);
                break;
        }

        __result = vec;

        return false;
    }
}
