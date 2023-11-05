using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseChanger : MonoBehaviour
{
    float insideTimer = 0f;
    Player[] playersInGame;
    PayloadMovement payloadMovementComponent;
    [SerializeField] float requiredTime = 10f;
    [SerializeField] float requiredDistance = 10f;
    [SerializeField] GameObject wall;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, requiredDistance);
    }

    private void Start()
    {
        playersInGame = FindObjectsOfType<Player>();
        payloadMovementComponent = gameObject.GetComponent<PayloadMovement>();
    }

    private void Update()
    {
        if (CheckIfAllPlayersAreInRange())
        {
            insideTimer += Time.deltaTime;

            if (insideTimer >= requiredTime)
            {
                ChangePhase();
            }
        }
    }

    private bool CheckIfAllPlayersAreInRange()
    {
        int playersInRange = 0;

        foreach (Player player in playersInGame)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= requiredDistance)
            {
                playersInRange++;
            }
        }

        if (playersInRange == playersInGame.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangePhase()
    {
        wall.SetActive(false);
        payloadMovementComponent.EnableMovement();
        Destroy(this);
    }
}
