using UnityEngine;

namespace Nitrox.Vr.Common;

public static class TransformExtension
{
    public static Transform Reset(this Transform transform)
    {
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
        return transform;
    }

    public static Transform SetParentAndReset(this Transform transform, Transform parent)
    {
        transform.SetParent(parent);
        return transform.Reset();
    }
}
