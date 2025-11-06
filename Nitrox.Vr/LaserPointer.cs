using NitroxModel.Logger;
using UnityEngine;

namespace Nitrox.Vr;

public class LaserPointer : MonoBehaviour
{
    private const float LINE_START_WIDTH = 0.004f;
    private const float LINE_END_WIDTH = 0.005f;
    private static readonly Color startColor = new(0f, 1f, 1f, 1f);
    private static readonly Color endColor = new(0f, 1f, 1f, 1f);
    
    private LineRenderer lineRenderer = null!;
    private Vector3? startPosition;
    private Vector3? endPosition;
    
    public void Initialize()
    {
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
        Log.Info($"Laser Update [origin = {startPosition}, target = {endPosition}]");
        if (endPosition == null || endPosition == null)
        {
            lineRenderer.enabled = false;
            return;
        }

        lineRenderer.enabled = true;
        lineRenderer.SetPositions([startPosition!.Value, endPosition.Value]);
    }

    public void SetPointerOriginTransform(Vector3 originPosition)
    {
        startPosition = originPosition;
    }

    public void SetPointerTarget(Vector3? pointerPosition)
    {
        endPosition = pointerPosition;
    }
}
