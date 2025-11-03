extern alias SteamVRRef;
using System.Reflection;
using Nitrox.Vr;


namespace NitroxPatcher.Patches.Persistent;

public sealed partial class GameInput_GetButtonUp_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => GameInput.GetButtonUp(default(GameInput.Button)));
    
    public static bool Prefix(GameInput.Button action, ref bool __result)
    {
        string actionName = action.ToString();
        
        if (SteamHelper.ShouldIgnoreButton(action))
        {
            return false;
        }
        
        __result = SteamVRRef::Valve.VR.SteamVR_Input.GetStateUp(actionName, SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
        return false;
    }
}
