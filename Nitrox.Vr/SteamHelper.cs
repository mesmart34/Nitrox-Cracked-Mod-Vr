extern alias SteamVRRef;
using SteamVRRef::Valve.VR;
using UnityEngine;

namespace Nitrox.Vr;

extern alias SteamVRActions;
extern alias SteamVRRef;

public static class SteamHelper
{
    private static readonly GameInput.Button[] buttonsToIgnore =
    [
        GameInput.Button.Slot1,
        GameInput.Button.Slot2,
        GameInput.Button.Slot3,
        GameInput.Button.Slot4,
        GameInput.Button.Slot5,
        GameInput.Button.AutoMove
    ];
    
    public static bool Ready { get; set; }
    
    public static bool InputLocked = false;

    public static bool SnapTurned { get; set; } = false;

    public static bool ShouldIgnoreButton(GameInput.Button action)
    {
        return InputLocked || buttonsToIgnore.Contains(action) || action.ToString() == "45" || action.ToString() == "46";
    }

    public static void InitializeSteamVr()
    {
        SteamVR.Initialize();
        SteamVR.settings.trackingSpace = ETrackingUniverseOrigin.TrackingUniverseSeated;
       
        Ready = SteamVR.initializedState == SteamVR.InitializedStates.InitializeSuccess;
    }
}
