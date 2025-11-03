using NitroxModel.Logger;
using UnityEngine;
using static UnityEngine.Object;

namespace Nitrox.Vr;

public static class CameraStealer
{
    private static Camera? _uiCamera = null;
    
    public static Camera GetUICamera
    {
        get
        {
            if(_uiCamera == null)
            {
                return FindUICamera();
            }
            return _uiCamera;
        }
    }

    private static Camera FindUICamera()
    {
        return FindObjectsOfType<Camera>().First(c => c.name.Equals("UI Camera"));
    }
}
