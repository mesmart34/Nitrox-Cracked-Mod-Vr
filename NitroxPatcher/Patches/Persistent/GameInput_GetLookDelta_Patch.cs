using System.Reflection;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class GameInput_GetLookDelta_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => GameInput.GetLookDelta());
    private bool SnapTurning = false;
    
    public static void Postfix(ref Vector2 __result)
    {
        // bool isInVehicle = Player.main?.currentMountedVehicle != null;
        // if (SnapTurning && !isInVehicle) {
        //     float lookX = __result.x;
        //     float absX = NitroxModel.Helper.Mathf.Abs(lookX);
        //     float threshold = 0.5f;
        //     if (absX > threshold && !SteamVrGameInput.SnapTurned) {
        //         __result.x = Settings.SnapTurningAngle * NitroxModel.Helper.Mathf.Sign(lookX);
        //         SteamVrGameInput.SnapTurned = true;
        //     } else  {
        //         __result.x = 0;
        //         if (absX <= threshold) {
        //             SteamVrGameInput.SnapTurned = false;
        //         }
        //     }
        // }
    }
}
