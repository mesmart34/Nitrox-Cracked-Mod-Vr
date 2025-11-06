using System.Reflection;
using Nitrox.Vr;
using UnityEngine;
using static UnityEngine.Object;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class GUIHand_Send_Patch : NitroxPatch, IDynamicPatch
{
    public static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => GUIHand.Send(default, default, default));
    
    public static void Postfix(GameObject target, HandTargetEventType e, GUIHand hand)
    {
        if (e == HandTargetEventType.Click)
        {
            FindObjectsOfType<uGUI_CanvasScaler>().ForEach(cs => cs.SetDirty());
        }
        ControllerRig.Instance.SetWorldTarget(hand.activeTarget, hand.activeHitDistance);
    }
}
