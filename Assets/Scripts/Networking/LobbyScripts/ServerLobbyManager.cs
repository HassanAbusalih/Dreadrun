using System.Collections.Generic;
using UnityEngine;

public class ServerLobbyManager : MonoBehaviour
{
    LobbyStatusPacket statusPacket;
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
        Server.Server.Instance.SendToAllClients(statusPacket.Serialize());
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