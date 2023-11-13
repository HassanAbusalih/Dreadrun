using System.Net;
using UnityEngine;

public class PhaseChanger : MonoBehaviour
{
    float insideTimer = 0f;
    Player[] playersInGame;
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
        if (GameManager.Instance != null)
            GameManager.Instance.onPhaseChange.AddListener(ChangePhase);
    }

    private void Update()
    {
        //if (CheckIfAllPlayersAreInRange())
        {
            insideTimer += Time.deltaTime;

            if (insideTimer >= requiredTime)
            {
                GameManager.Instance.ChangePhase();
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
        if (wall != null)
            wall.SetActive(false);

        Destroy(this);
    }
    void OnDestroy()
    {
        GameManager.Instance.onPhaseChange.RemoveListener(ChangePhase);
    }
}
