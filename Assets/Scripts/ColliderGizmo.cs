
using UnityEngine;


public class ColliderGizmo : MonoBehaviour
{
    private void OnValidate()
    {

    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GetComponent<BoxCollider>().center + transform.position, GetComponent<BoxCollider>().size + transform.localScale);

        Gizmos.color = new Color(1, 0, 0, 0.15f);
        Gizmos.DrawCube(GetComponent<BoxCollider>().center + transform.position, GetComponent<BoxCollider>().size + transform.localScale);
    }
#endif
}


