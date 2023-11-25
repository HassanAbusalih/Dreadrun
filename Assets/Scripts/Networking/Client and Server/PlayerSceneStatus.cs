using ClientLibrary;
using UnityEngine;

//Server Side
public class PlayerSceneStatus : MonoBehaviour
{
    [SerializeField] bool[] playersInMainScene = new bool[3];
    [SerializeField] string prefabName;
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
        Vector3 playerSpawnPosition = new Vector3(2, 4, 5);

        InstantiationPacket packet = new InstantiationPacket(prefabName, playerSpawnPosition, Quaternion.identity);
        Server.Server.Instance.SendToAllClients(packet.Serialize());
        Debug.LogError("Prefab is sent to all clients");
    }
}
