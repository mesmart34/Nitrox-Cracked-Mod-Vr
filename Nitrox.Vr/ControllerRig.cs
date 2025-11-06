extern alias SteamVRRef;
extern alias SteamVRActions;
using Nitrox.Vr.Common;
using NitroxModel.Logger;
using UnityEngine;

namespace Nitrox.Vr;

extern alias SteamVRActions;

public class ControllerRig : MonoBehaviour
{
    private Dictionary<Hand, Controller> Hands { get; } = new();

    private Camera camera = null!;

    private Hand mainHand = Hand.Right;

    private LaserPointer laserPointer = null!;

    public Controller ActiveHand => Hands[mainHand];

    public Camera EventCamera => ActiveHand.EventCamera;

    public static ControllerRig Instance { get; set; } = null!;

    public void Initialize()
    {
        Instance = this;
        laserPointer = gameObject.AddComponent<LaserPointer>();
        laserPointer.Initialize();

        camera = Camera.main!;

        transform.SetParent(camera.transform.parent);
        transform.Reset();

        CreateHands();

        // SetupHandReticleOnHand(SNCameraRoot.main.guiCamera, ActiveHand.transform);
    }

    private void Update()
    {
        if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
        {
            mainHand = Hand.Left;
        }

        if (GameInput.GetButtonDown(GameInput.Button.RightHand))
        {
            mainHand = Hand.Right;
        }
        Log.Info($"Event camera: {EventCamera.transform.position}");
        
        laserPointer.SetPointerOriginTransform(ActiveHand.transform.position);
    }

    public void CreateHands()
    {
        Hands.Add(Hand.Left, CreateController(Hand.Left, camera.transform.parent));
        Hands.Add(Hand.Right, CreateController(Hand.Right, camera.transform.parent));
    }

    private static Controller CreateController(Hand hand, Transform parent)
    {
        Controller? controller = new GameObject($"{hand}").AddComponent<Controller>();
        controller.Initialize(hand, parent);
        return controller;
    }

    public void SetPointerTargetPosition(Vector3? targetPosition)
    {
        laserPointer.SetPointerTarget(targetPosition);
    }
    
    public void SetWorldTarget(GameObject target, float distance)
    {
        Log.Info($"target name: {target.gameObject.name}");
        DebugPanel.Show(target.gameObject.name);
    }

    public void SetLayer(int layerID)
    {
        Hands.ForEach(x => x.Value.SetLayer(layerID));
    }
    
    public static void SetupHandReticleOnHand(Camera uiCamera, Transform rightControllerUI)
    {
        GameObject handReticle = HandReticle.main.gameObject;
        handReticle.transform.SetParent(rightControllerUI.transform, true);
        
        Canvas canvas = handReticle.GetComponent<Canvas>();
        if(canvas == null)
        {
            canvas = handReticle.AddComponent<Canvas>();
            
        }
        canvas.worldCamera = uiCamera;
        
        handReticle.transform.localEulerAngles = new Vector3(90, 0, 0);
        handReticle.transform.localPosition = new Vector3(0, 0, 0.05f);
        handReticle.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
    }
}
