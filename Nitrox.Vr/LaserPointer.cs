using UnityEngine;

namespace Nitrox.Vr;

public class LaserPointer : MonoBehaviour
{
    public static LaserPointer Instance = null!;

    private const float LINE_START_WIDTH = 0.004f;
    private const float LINE_END_WIDTH = 0.005f;
    private static readonly Color startColor = new(0f, 1f, 1f, 1f);
    private static readonly Color endColor = new(0f, 1f, 1f, 1f);
    
    private LineRenderer lineRenderer = null!;
    private Vector3? endPosition;
    
    public void Initialize()
    {
        Instance = this;
        
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        Material material = new(ShaderManager.preloadedShaders.DebugDisplaySolid);
        material.SetColor(ShaderPropertyID._Color, Color.cyan);
        
        lineRenderer.material = material;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.startWidth = LINE_START_WIDTH;
        lineRenderer.endWidth = LINE_END_WIDTH;
        lineRenderer.gameObject.layer = LayerMask.NameToLayer("UI");
    }

    public void SetEnabled(bool value)
    {
        lineRenderer.enabled = value;
    }

    private void Update()
    {
        Camera? eventCamera = ControllerRig.Instance.GetActiveEventCamera();
        if (eventCamera != null && endPosition != null)
        {
            Vector3 startPosition = eventCamera.transform.position - eventCamera.transform.up * 0.05f;
            lineRenderer.SetPositions([startPosition, endPosition.Value]);
        }
    }

    public void SetPointerTarget(Vector3? pointerPosition)
    {
        endPosition = pointerPosition;
    }
}
