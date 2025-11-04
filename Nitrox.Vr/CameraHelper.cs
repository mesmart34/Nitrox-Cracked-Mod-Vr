using NitroxModel.Logger;
using UnityEngine;
using static UnityEngine.Object;

namespace Nitrox.Vr;

public static class CameraHelper
{
    private static Camera? uiCamera = null;
    private static Camera? worldCamera = null;
    
    public static Camera GetUICamera
    {
        get
        {
            if(uiCamera == null)
            {
                uiCamera = FindUICamera();
            }
            return uiCamera;
        }
    }
    
    public static Camera GetWorldCamera
    {
        get
        {
            if(worldCamera == null)
            {
                worldCamera = FindWorldCamera();
            }
            return worldCamera;
        }
    }

    public static void Reset()
    {
        uiCamera = null;
        worldCamera = null;
    }

    private static Camera FindUICamera()
    {
        return FindObjectsOfType<Camera>().First(c => c.name.Equals("UI Camera"));
    }
    
    private static Camera FindWorldCamera()
    {
        return GameObject.FindGameObjectsWithTag("MainCamera").First(c => c.name.Equals("Main Camera")).GetComponent<Camera>();
    }
}
