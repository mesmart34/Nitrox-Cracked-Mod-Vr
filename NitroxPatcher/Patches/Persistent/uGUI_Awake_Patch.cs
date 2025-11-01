using System.Reflection;
using Nitrox.Vr;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class uGUI_Awake_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((uGUI t) => t.Awake());

    public static void Postfix()
    {
        Log.Info("camera rig");
        VrCameraRig rig = new GameObject(nameof(VrCameraRig)).AddComponent<VrCameraRig>();
        VrCameraRig.Instance = rig;
        Object.DontDestroyOnLoad(rig);
    }
}
