using UnityEngine.XR;

namespace Nitrox.Vr;

public static class Settings
{
    public static readonly bool IsVrEnabled = XRSettings.loadedDeviceName == "OpenVR";

    public static readonly Common.Hand DefaultHand = Common.Hand.Right;

    public static float SnapTurningAngle { get; set; } = 45.0f;
}
