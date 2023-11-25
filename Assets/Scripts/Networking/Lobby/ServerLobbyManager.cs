using System.Collections.Generic;
using UnityEngine;

public class ServerLobbyManager : MonoBehaviour
{
    LobbyStatusPacket statusPacket;
    ScenePacket scenePacket;
    [SerializeField] string sceneName;
    public List<bool> playerStatuses = new();
    public List<string> playerIDs = new();

    void UpdateLobbyStatus(string clientID, bool isReady)
    {
        if (playerIDs.Contains(clientID))
        {
            for (int i = 0; i < playerIDs.Count; i++)
            {
                if (playerIDs[i] == clientID)
                {
                    Debug.LogError($"Updating Player {i + 1} to {isReady}");
                    playerStatuses[i] = isReady;
                }
            }
        }
        else
        {
            playerIDs.Add(clientID);
            playerStatuses.Add(isReady);
            Debug.LogError("New client in Lobby! They are player " + playerIDs.Count);
        }
        statusPacket = new LobbyStatusPacket(playerStatuses, playerIDs);
        ChangeScene();
        Server.Server.Instance.SendToAllClients(statusPacket.Serialize());
    }

    void ChangeScene()
    {
        if(playerStatuses.Count < 3) return;
        for (int i = 0; i < playerStatuses.Count; i++)
        {
            if (!playerStatuses[i])
            {
                Debug.LogError("Not all players are ready!");
                return;
            }
        }
        Debug.LogError("Run you goddamn donkey");
        scenePacket = new ScenePacket(sceneName);
        Server.Server.Instance.SendToAllClients(scenePacket.Serialize());
    }

    private void OnEnable()
    {
        Server.Server.Instance.OnServerLobbyUpdate += UpdateLobbyStatus;
    }

    private void OnDisable()
    {
        Server.Server.Instance.OnServerLobbyUpdate -= UpdateLobbyStatus;
    }
}