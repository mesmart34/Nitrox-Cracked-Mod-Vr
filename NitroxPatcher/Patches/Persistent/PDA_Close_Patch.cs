using System.Reflection;
using Nitrox.Vr;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class PDA_Close_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((PDA t) => t.Close());
    
    public static void Postfix()
    {
        if (VrHands.Instance != null)
        {
            VrHands.Instance.OnClosePDA();
        }
    }
}
