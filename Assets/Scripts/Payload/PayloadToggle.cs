using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadToggle : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Payload payload;
    [SerializeField] float range = 10f;
    [SerializeField] GameObject gateWall;
    [SerializeField] GameObject musicStart;

    void Start()
    {
        player = FindObjectOfType<Player>();
        payload = FindObjectOfType<Payload>();
    }

    void Update()
    {
        if (player == null) return;
        if (Vector3.Distance(transform.position, player.transform.position) < range)
        {
            if (Input.GetKey(KeyCode.F))
            {
                payload.StartFollowingPath();
                gameObject.SetActive(false);
                Destroy(gateWall);
            }
        }
    }
    private void OnDisable()
    {
        if(musicStart != null)
        {
            musicStart.SetActive(true);
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

}
