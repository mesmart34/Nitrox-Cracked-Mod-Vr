extern alias SteamVRRef;
extern alias SteamVRActions;
using Nitrox.Vr.Common;
using UnityEngine;

namespace Nitrox.Vr;

extern alias SteamVRActions;

public class ControllerRig : MonoBehaviour
{
    private Dictionary<Hand, Controller> Hands { get; } = new();

    private Camera camera = null!;

    private Hand mainHand = Hand.Right;

    private LaserPointer laserPointer = null!;

    public static ControllerRig Instance { get; set; } = null!;

    public void Initialize()
    {
        Instance = this;
        laserPointer = gameObject.AddComponent<LaserPointer>();
        laserPointer.Initialize();
    }

    private void Start()
    {
        camera = Camera.main!;
        
        transform.SetParent(camera.transform.parent);
        transform.Reset();
        
        CreateHands();
        
        // SetMainHand(Settings.DefaultHand);
    }

    private void Update()
    {
        if (GameInput.GetButtonDown(GameInput.Button.LeftHand))
        {
            mainHand = Hand.Left;
            // SetMainHand(mainHand);
        }
        
        if (GameInput.GetButtonDown(GameInput.Button.RightHand))
        {
            mainHand = Hand.Right;
            // SetMainHand(mainHand);
        }
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

    public Camera? GetActiveEventCamera()
    {
        if (Hands.TryGetValue(mainHand, out Controller value))
        {
           return value.GetEventCamera();
        }
        return null;
    }
}
