using System.Reflection;
using Nitrox.Vr;
using Nitrox.Vr.Common;
using NitroxClient.MonoBehaviours;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class PlayerController_Start_Patch : NitroxPatch, IDynamicPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((Player t) => t.Start());
    
    public static void Postfix(PlayerController __instance)
    {
        ControllerRig rig = new GameObject(nameof(ControllerRig)).AddComponent<ControllerRig>();
        rig.Initialize();
        rig.SetLayer(LayerID.Default);
    }
}
