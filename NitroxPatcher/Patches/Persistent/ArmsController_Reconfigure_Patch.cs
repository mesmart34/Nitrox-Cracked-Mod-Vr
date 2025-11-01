using System.Reflection;
using Nitrox.Vr;

namespace NitroxPatcher.Patches.Persistent;

public sealed partial class ArmsController_Reconfigure_Patch : NitroxPatch, IPersistentPatch
{
    public static readonly MethodInfo TARGET_METHOD = Reflect.Method((ArmsController t) => t.Reconfigure(default(PlaceTool)));
    
    public static void Postfix(PlayerTool tool)
    {
        if (VrHands.Instance != null)
        {
            VrHands.Instance.OnToolEquipped(tool);
        }
    }
}
