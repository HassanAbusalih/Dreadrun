using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Server;

public class LobbyManager : MonoBehaviour
{
    LobbyPacket lobbyPacket;
    Dictionary<string,Image> idToImage = new Dictionary<string,Image>();
    [SerializeField] Image[] images;

    private void OnEnable()
    {
        ClientLibrary.Client.Instance.OnLobbyUpdate += UpdateImage;
    }

    private void OnDisable()
    {
        ClientLibrary.Client.Instance.OnLobbyUpdate -= UpdateImage;
    }

    private void Start()
    {
        string clientID = ClientLibrary.Client.Instance.networkComponent.ClientID;
        lobbyPacket = new LobbyPacket(false, "", clientID);
        Debug.LogError($"My client ID is {clientID}");
        ClientLibrary.Client.Instance.SendPacket(lobbyPacket.Serialize());
        Debug.LogError("I'm joining the lobby!");
    }


    public void OnButtonClick()
    {
        lobbyPacket.isReady = !lobbyPacket.isReady;
        //UpdateImage(lobbyPacket.isReady, index);
        ClientLibrary.Client.Instance.SendPacket(lobbyPacket.Serialize());
        Debug.LogError($"My ready status is now {lobbyPacket.isReady}!");
    }

    private void UpdateImage(List<string> playerIDs, List<bool> playerStatuses)
    {
        for (int i = 0; i < playerIDs.Count; i++)
        {
            if (idToImage.ContainsKey(playerIDs[i]))
            {
                Debug.LogError($"Player {i + 1} is now {playerStatuses[i]}!");
                idToImage[playerIDs[i]].color = playerStatuses[i] ? Color.green : Color.red; //if red make it green and vice versa
            }
            else
            {
                idToImage.Add(playerIDs[i], images[i]);
               
            }
        }
    }

    void UpdateImage(bool isReady, int index)
    {
        images[index].color = isReady ? Color.green : Color.red;
    }
}