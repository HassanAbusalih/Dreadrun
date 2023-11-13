using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory/Speed Potion")]
public class SpeedPotion : ItemBase
{
    [SerializeField] int speedIncrease;
    float defaultSpeed;

    float timer = 0;
    [SerializeField] float duration;

    [SerializeField] GameObject timerPrefab;
    Player playerRef;

    public override void UseOnSelf(Player player)
    {
        hasBuffedItem = true;
        defaultSpeed = player.playerStats.speed;
        player.playerStats.speed = speedIncrease;
        Debug.Log("Speed increased");
        GetTimer();
        playerRef = player;
    }

    private void GetTimer()
    {
        Timer timerScript = Instantiate(timerPrefab).GetComponent<Timer>();
        timerScript.SetTimerAndDuration(timer, duration);
        timerScript.onTimerMet += ResetSpeed;
    }

    private void ResetSpeed()
    {
        playerRef.playerStats.speed = defaultSpeed;
        hasBuffedItem = false;
        Debug.Log("Speed back to normal");
    }

}
