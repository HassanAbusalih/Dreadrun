using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SummonPlayer : MonoBehaviour
{
    [SerializeField]
    float yOffset;
    Vector3 spawnPos;
    public GameObject player;
    private GameObject gj;
    private void Start()
    {
        if (player == null)
        {
            Debug.LogWarning("Player bonked");
            return;
        }
        Invoke("SpawnPlayer", 0.01f);
    }

    void SpawnPlayer()
    {
        gj = this.gameObject;
        spawnPos = gj.transform.position;
        spawnPos.y += yOffset;
        this.player.transform.position = spawnPos;
        this.player.transform.rotation = Quaternion.Euler(0f, gj.transform.rotation.eulerAngles.y, 0f);
    }
}