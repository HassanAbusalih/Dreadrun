using ClientLibrary;
using UnityEngine;

//Server Side
public class PlayerSceneStatus : MonoBehaviour
{
    [SerializeField] bool[] playersInMainScene = new bool[3];
    [SerializeField] string realPlayer;
    [SerializeField] string fakePlayer;
    int index;

    private void OnEnable()
    {
        Server.Server.Instance.UpdatePlayerSceneStatus += UpdateSceneStatus;
    }

    private void OnDisable()
    {
        Server.Server.Instance.UpdatePlayerSceneStatus -= UpdateSceneStatus;

    }
    void UpdateSceneStatus(bool status)
    {
        playersInMainScene[index] = status;
        index++;
        if (index == playersInMainScene.Length)
        {
            SpawnPlayers();
            Debug.LogError("All players are now in the main scene");
        }
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < playersInMainScene.Length; i++)
        {
            Server.Server.Instance.SpawnPlayerObjects(realPlayer, fakePlayer, i);
        }
    }
}
