using System.Reflection;
using Nitrox.Vr;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class GUIHand_Send_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => GUIHand.Send(default, default, default));

    private static void DirtyAllCanvases()
    {
        // TODO: Should cache this maybe, not sure when those could change though   
        Object.FindObjectsOfType<uGUI_CanvasScaler>().ForEach(cs => cs.SetDirty());
    }

    public static void Postfix(GameObject target, HandTargetEventType e, GUIHand hand)
    {
        if (e == HandTargetEventType.Click)
        {
            DirtyAllCanvases();
        }
        if (VrCameraRig.Instance != null)
        {
            VrCameraRig.Instance.SetWorldTarget(hand.activeTarget, hand.activeHitDistance);
        }
    }
}
