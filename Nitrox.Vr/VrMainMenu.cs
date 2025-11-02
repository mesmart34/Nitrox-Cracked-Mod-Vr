using NitroxModel.Logger;
using UnityEngine;

namespace Nitrox.Vr;

public class VrMainMenu : MonoBehaviour
{
    public static void SetupMainMenu()
    {
        if (VrCameraRig.Instance != null)
        {
            Log.Info("SetupMainMenu");
            Camera uiCamera = FindObjectsOfType<Camera>().First(c => c.name.Equals("UI Camera"));
            VrCameraRig.Instance.StealUICamera(uiCamera);
            Camera mainCamera = GameObject.FindGameObjectsWithTag("MainCamera").First(c => c.name.Equals("Main Camera")).GetComponent<Camera>();
            VrCameraRig.Instance.StealCamera(mainCamera);
        }
    }
}
