using UnityEngine;

namespace Nitrox.Vr.Common;

public readonly struct TransformOffset(Vector3 pos, Vector3 angles)
{
    public TransformOffset(Transform transform) : this(transform.localPosition, transform.localEulerAngles)
    {
    }

    public void Apply(Transform tf)
    {
        tf.localPosition = pos;
        tf.localEulerAngles = angles;
    }
}
