using System.Reflection;
using Nitrox.Vr;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class PDA_Open_Patch : NitroxPatch, IPersistentPatch
{
    private static readonly MethodInfo TARGET_METHOD = Reflect.Method((PDA t) => t.Open(default, default, default));
    
    public static void Postfix()
    {
        if (VrHands.Instance != null)
        {
            VrHands.Instance.OnOpenPDA();
        }
    }
}
