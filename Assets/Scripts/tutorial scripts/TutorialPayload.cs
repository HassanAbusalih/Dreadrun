using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPayload : MonoBehaviour
{
    [SerializeField] float InteractionRange;
    [SerializeField] Payload payload;
    [SerializeField] GameObject wall;

    // Start is called before the first frame update
    void Start()
    {
        payload= FindObjectOfType<Payload>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PayloadInRange())
        {
            wall.SetActive(false);
        }
    }

    bool PayloadInRange()
    {
        if (Vector3.Distance(transform.position, payload.transform.position) < InteractionRange)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, InteractionRange);
    }
}
