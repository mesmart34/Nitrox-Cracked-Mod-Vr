using UnityEngine;

namespace Nitrox.Vr;

extern alias SteamVRActions;
extern alias SteamVRRef;

public static class SteamVrGameInput
{
    public static bool IsSteamVrReady;
    public static bool InputLocked = false;
    
    public static bool ShouldIgnore(GameInput.Button action)
    {
        return !IsSteamVrReady || InputLocked
                               || action == GameInput.Button.Slot1
                               || action == GameInput.Button.Slot2
                               || action == GameInput.Button.Slot3
                               || action == GameInput.Button.Slot4
                               || action == GameInput.Button.Slot5
                               || action == GameInput.Button.AutoMove
                               || action.ToString() == "45" || action.ToString() == "46";
    }
}
