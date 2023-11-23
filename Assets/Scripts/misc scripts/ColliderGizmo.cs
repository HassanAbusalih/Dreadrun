using UnityEngine;

public class ColliderGizmo : MonoBehaviour
{
    [SerializeField]bool useColliderScale;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Vector3 _chosenScale = useColliderScale ? Vector3.Scale(boxCollider.size, transform.localScale) : transform.localScale;
        Vector3 transformedCenter = transform.TransformPoint(boxCollider.center);

        Matrix4x4 cubeTransform = Matrix4x4.TRS(transformedCenter, transform.rotation, _chosenScale);

        Gizmos.color = Color.yellow;
        Gizmos.matrix = cubeTransform;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);

        Gizmos.color = new Color(1, 0, 0, 0.15f);
        Gizmos.DrawCube(Vector3.zero, Vector3.one);

        Gizmos.matrix = Matrix4x4.identity;
    }
#endif
}