extern alias SteamVRRef;
using System.Reflection;
using Nitrox.Vr;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class GameInput_GetButtonDown_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => GameInput.GetButtonDown(default(GameInput.Button)));
    
    public static bool Prefix(GameInput.Button action, ref bool __result)
    {
        string actionName = action.ToString();
        
        if (SteamVrGameInput.ShouldIgnore(action))
        {
            return false;
        }
        Log.Info(actionName);
        
        __result = SteamVRRef::Valve.VR.SteamVR_Input.GetStateDown(actionName, SteamVRRef::Valve.VR.SteamVR_Input_Sources.Any);
        return false;
    }
}
