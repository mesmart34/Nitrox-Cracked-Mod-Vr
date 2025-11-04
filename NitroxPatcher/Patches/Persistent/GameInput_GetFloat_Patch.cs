extern alias SteamVRActions;
using System.Reflection;
using Nitrox.Vr;
using SteamVRActions::Valve.VR;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

extern alias SteamVRRef;

public sealed partial class GameInput_GetFloat_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => GameInput.GetFloat(default(GameInput.Button)));

    public static bool Prefix(GameInput.Button action, ref float __result)
    {
        if (SteamHelper.InputLocked || !SteamHelper.Ready)
        {
            __result = 0.0f;
            return false;
        }
        
        Vector2 vec;
        bool isPressed = false;
        float value = 0.0f;
        switch (action)
        {
            case GameInput.Button.MoveForward:
                vec = SteamVR_Actions.subnautica.Move.GetAxis(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
                value = vec.y > 0.0f ? vec.y : 0.0f;
                break;
            case GameInput.Button.MoveBackward:
                vec = SteamVR_Actions.subnautica.Move.GetAxis(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
                value = vec.y < 0.0f ? -vec.y : 0.0f;
                break;
            case GameInput.Button.MoveRight:
                vec = SteamVR_Actions.subnautica.Move.GetAxis(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
                value = vec.x > 0.0f ? vec.x : 0.0f;
                break;
            case GameInput.Button.MoveLeft:
                vec = SteamVR_Actions.subnautica.Move.GetAxis(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
                value = vec.x < 0.0f ? -vec.x : 0.0f;
                break;
            case GameInput.Button.MoveUp:
                isPressed = SteamVR_Actions.subnautica.MoveUp.GetState(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
                value = isPressed ? 1.0f : 0.0f;
                break;
            case GameInput.Button.MoveDown:
                isPressed = SteamVR_Actions.subnautica.MoveDown.GetState(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
                value = isPressed ? 1.0f : 0.0f;
                break;
            case GameInput.Button.LookUp:
                vec = SteamVR_Actions.subnautica.Look.GetAxis(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
                value = vec.y > 0.0f ? vec.y : 0.0f;
                break;
            case GameInput.Button.LookDown:
                vec = SteamVR_Actions.subnautica.Look.GetAxis(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
                value = vec.y < 0.0f ? -vec.y : 0.0f;
                break;
            case GameInput.Button.LookRight:
                vec = SteamVR_Actions.subnautica.Look.GetAxis(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
                value = vec.x > 0.0f ? vec.x : 0.0f;
                break;
            case GameInput.Button.LookLeft:
                vec = SteamVR_Actions.subnautica.Look.GetAxis(SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
                value = vec.x < 0.0f ? -vec.x : 0.0f;
                break;
        }

        __result = NitroxModel.Helper.Mathf.Clamp(value, -1.0f, 1.0f);

        return false;
    }
}
