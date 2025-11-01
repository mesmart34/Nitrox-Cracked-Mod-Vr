using System.Reflection;
using Nitrox.Vr;
using UnityEngine;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class FPSInputModule_ShouldStartDrag_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method(() => FPSInputModule.ShouldStartDrag(default(Vector2), default(Vector2), default(float), default(bool)));
    
    public static bool Prefix(ref bool __result, Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
    {
        // TODO: This has to be dependent on canvas scale, way to high for big pda, too low for small pda
        float newThreshold = 0.04f;
        __result = !useDragThreshold || (pressPos - currentPos).sqrMagnitude >= newThreshold * newThreshold;
        return false;
    }
}
