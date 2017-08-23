using UnityEngine;

public class UltimateSeeThroughEventManager : MonoBehaviour
{
    public delegate void TransformBoolToVoid(Transform arg, bool arg2);
    public static event TransformBoolToVoid SetTransformObjectToTransparentOrNonTransparentEvent;


    public void SetTransformObjectToTransparentOrNonTransparentMethod(Transform transform, bool transparentOrNonTransparent)
    {
        SetTransformObjectToTransparentOrNonTransparentEvent(transform, transparentOrNonTransparent);
    }
}
