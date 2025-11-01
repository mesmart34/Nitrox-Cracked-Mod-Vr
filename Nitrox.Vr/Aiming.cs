using UnityEngine;

namespace Nitrox.Vr;

public class Aiming
{
    public static Camera? GetAimCamera()
    {
        if (VrCameraRig.Instance != null && VrCameraRig.Instance.laserPointer != null)
        {
            return VrCameraRig.Instance.laserPointer.eventCamera;
        }

        return null;
    }
    
    public static Transform? GetAimTransform()
    {
        if (VrCameraRig.Instance != null && VrCameraRig.Instance.laserPointer != null)
        {
            return VrCameraRig.Instance.laserPointer.transform;
        }
        
        return null;
    }
}
