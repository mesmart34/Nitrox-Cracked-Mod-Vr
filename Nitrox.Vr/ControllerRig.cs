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
    
    private LineRenderer lineRenderer = null!;
    private Vector3? endPosition;

    public static ControllerRig Instance { get; set; } = null!;
    
    private const float LINE_START_WIDTH = 0.004f;
    private const float LINE_END_WIDTH = 0.005f;
    private const float LENGTH = 5.0f;
    private static readonly Color startColor = new(0f, 1f, 1f, 1f);
    private static readonly Color endColor = new(0f, 1f, 1f, 1f);

    private void Start()
    {
        camera = Camera.main!;
        
        transform.SetParent(camera.transform.parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        CreateHands();
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        
        Material material = new(ShaderManager.preloadedShaders.DebugDisplaySolid);
        material.SetColor(ShaderPropertyID._Color, Color.cyan);
        
        lineRenderer.material = material;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.startWidth = LINE_START_WIDTH;
        lineRenderer.endWidth = LINE_END_WIDTH;
        lineRenderer.gameObject.layer = LayerMask.NameToLayer("UI");
        
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

        Camera? eventCamera = GetActiveEventCamera();
        if (eventCamera != null && endPosition != null)
        {
            Vector3 startPosition = eventCamera.transform.position - eventCamera.transform.up * 0.05f;
            lineRenderer.SetPositions([startPosition, endPosition.Value]);
        }
    }

    // private void SetMainHand(Hand hand)
    // {
    //     Hands.ForEach(x =>
    //     {
    //         x.Value.SetPointerEnabled(hand == x.Key);
    //     });
    // }

    // public void SetModelsEnabled(bool renderEnabled)
    // {
    //     Hands.ForEach(x => x.Value.SetModelEnabled(renderEnabled));
    // }

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

    public void SetPointerTarget(Vector3 pointerPosition)
    {
        endPosition = pointerPosition;
    }
}
