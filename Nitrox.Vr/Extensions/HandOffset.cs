using Nitrox.Vr.Common;
using UnityEngine;

namespace Nitrox.Vr.Extensions;

public static class HandOffset
{
    public static TransformOffset RightHand = new TransformOffset(new Vector3(0.05f, 0.06f, -0.17f), new Vector3(40.0f, 175.0f, 270.0f));
    public static TransformOffset LeftHand = new TransformOffset(new Vector3(-0.05f, 0.06f, -0.17f), new Vector3(-40.0f, 0.0f, 90.0f));
    public static TransformOffset PDA = new TransformOffset(new Vector3(-0.05f, 0.05f, -0.14f), new Vector3(305.0f, 355.0f, 100.0f));

    internal static TransformOffset GetHandOffset(this PlayerTool tool)
    {
        switch (tool)
        {
            case FireExtinguisher _: return new TransformOffset(new Vector3(0.062f, 0.077f, -0.148f), new Vector3(40.736f, 139.849f, 249.888f));
            case Seaglide _: return new TransformOffset(new Vector3(0.055f, 0.101f, -0.125f), new Vector3(24.986f, 153.649f, 265.740f));
            case Gravsphere _: return new TransformOffset(new Vector3(-0.010f, 0.114f, -0.125f), new Vector3(10.485f, 158.948f, 244.422f));
            case DeployableStorage _: return new TransformOffset(new Vector3(0.017f, 0.099f, -0.135f), new Vector3(27.633f, 159.160f, 251.929f));
            case Constructor _: return new TransformOffset(new Vector3(0.042f, 0.076f, -0.166f), new Vector3(53.635f, 151.667f, 249.508f));
            case LEDLight _: return new TransformOffset(new Vector3(0.051f, 0.113f, -0.122f), new Vector3(20.287f, 157.143f, 262.503f));
            case Knife _: return new TransformOffset(new Vector3(0.008f, 0.095f, -0.115f), new Vector3(17.193f, 162.033f, 250.308f));
            case FlashLight _: return new TransformOffset(new Vector3(0.011f, 0.125f, -0.123f), new Vector3(18.573f, 162.636f, 247.017f));
            case Beacon _: return new TransformOffset(new Vector3(0.006f, 0.137f, -0.165f), new Vector3(31.791f, 151.351f, 242.064f));
            case StasisRifle _: return new TransformOffset(new Vector3(0.013f, 0.091f, -0.155f), new Vector3(32.203f, 147.266f, 237.102f));
            case PropulsionCannonWeapon _: return new TransformOffset(new Vector3(0.001f, 0.078f, -0.169f), new Vector3(36.510f, 148.234f, 231.454f));
            case BuilderTool _: return new TransformOffset(new Vector3(0.042f, 0.090f, -0.129f), new Vector3(30.567f, 155.533f, 258.169f));
            case AirBladder _: return new TransformOffset(new Vector3(-0.032f, 0.090f, -0.133f), new Vector3(7.689f, 145.798f, 224.260f));
            case DiveReel _: return new TransformOffset(new Vector3(-0.019f, 0.096f, -0.119f), new Vector3(14.196f, 148.834f, 238.635f));
            case Welder _: return new TransformOffset(new Vector3(0.002f, 0.110f, -0.140f), new Vector3(23.999f, 153.807f, 241.427f));
            case ScannerTool _: return new TransformOffset(new Vector3(0.006f, 0.109f, -0.149f), new Vector3(25.775f, 145.925f, 236.092f));
            case LaserCutter _: return new TransformOffset(new Vector3(-0.017f, 0.123f, -0.133f), new Vector3(17.726f, 151.261f, 233.612f));
            case Flare _: return new TransformOffset(new Vector3(0.019f, 0.088f, -0.134f), new Vector3(31.170f, 153.374f, 244.228f));
            case RepulsionCannon _: return new TransformOffset(new Vector3(-0.002f, 0.088f, -0.166f), new Vector3(33.777f, 149.093f, 232.610f));
            default: return RightHand;
        }
        ;
    }

    internal static TransformOffset GetAimOffset(this PlayerTool tool)
    {
        switch (tool)
        {
            case BuilderTool _: return new TransformOffset(new Vector3(-0.021f, -0.040f, 0.030f), new Vector3(73.312f, 342.306f, 323.270f));
            case ScannerTool _: return new TransformOffset(new Vector3(0.008f, -0.106f, -0.016f), new Vector3(73.923f, 62.973f, 42.123f));
            case FireExtinguisher _: return new TransformOffset(new Vector3(0.003f, -0.101f, -0.054f), new Vector3(64.181f, 10.062f, 346.755f));
            case Seaglide _: return new TransformOffset(new Vector3(0.003f, -0.101f, -0.054f), new Vector3(64.181f, 10.062f, 346.755f));
            case RepulsionCannon _: return new TransformOffset(new Vector3(-0.014f, -0.083f, 0.024f), new Vector3(13.596f, 352.247f, 315.238f));
            case StasisRifle _: return new TransformOffset(new Vector3(-0.037f, -0.073f, 0.039f), new Vector3(44.380f, 349.016f, 309.364f));
            case FlashLight _: return new TransformOffset(new Vector3(-0.029f, -0.024f, 0.074f), new Vector3(74.684f, 27.358f, 341.475f));
            case DiveReel _: return new TransformOffset(new Vector3(0.020f, -0.105f, -0.084f), new Vector3(80.062f, 36.462f, 346.389f));
            case Welder _: return new TransformOffset(new Vector3(-0.028f, -0.017f, 0.056f), new Vector3(59.356f, 350.101f, 303.392f));
            case LaserCutter _: return new TransformOffset(new Vector3(-0.014f, -0.042f, 0.068f), new Vector3(60.953f, 4.267f, 316.030f));
            case HeatBlade _: return new TransformOffset(new Vector3(-0.016f, 0.033f, 0.004f), new Vector3(65.781f, 19.633f, 267.986f));
            case LEDLight _: return new TransformOffset(new Vector3(-0.021f, 0.005f, -0.008f), new Vector3(63.596f, 40.279f, 283.758f));
            
            //TODO: here was a variable from original VrCameraRig "DefaultTargetTransform"
            default: return new TransformOffset(Vector3.zero, new Vector3(45, 0, 0));
        }
        ;
    }
}
