using System.Reflection;
using Nitrox.Vr;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class GameInput_GetLookDelta_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => GameInput.GetLookDelta());
    private bool SnapTurning = false;
    
    public static void Postfix(ref Vector2 __result)
    {
        bool isInVehicle = Player.main.currentMountedVehicle != null;
        if (Settings.IsSnapTurningEnabled && !isInVehicle) {
            float lookX = __result.x;
            float absX = UnityEngine.Mathf.Abs(lookX);
            const float THRESHOLD = 0.5f;
            if (absX > THRESHOLD && !SteamVrGameInput.SnapTurned) {
                __result.x = Settings.SnapTurningAngle * UnityEngine.Mathf.Sign(lookX);
                SteamVrGameInput.SnapTurned = true;
            } else  {
                __result.x = 0;
                if (absX <= THRESHOLD) {
                    SteamVrGameInput.SnapTurned = false;
                }
            }
        }
    }
}
