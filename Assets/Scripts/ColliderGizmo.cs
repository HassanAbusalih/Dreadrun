
using UnityEngine;


public class ColliderGizmo : MonoBehaviour
{
    [SerializeField]bool useColliderScale;

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Vector3 _chosenScale = useColliderScale ? GetComponent<BoxCollider>().size : transform.localScale;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GetComponent<BoxCollider>().center + transform.position, _chosenScale);

        Gizmos.color = new Color(1, 0, 0, 0.15f);
        Gizmos.DrawCube(GetComponent<BoxCollider>().center + transform.position, _chosenScale);
    }
#endif
}


