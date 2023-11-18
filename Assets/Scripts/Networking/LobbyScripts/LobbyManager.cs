using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Server;

public class LobbyManager : MonoBehaviour
{
    LobbyPacket lobbyPacket;
    Dictionary <string,Image> idToImage = new Dictionary<string,Image>();
    [SerializeField] Image[] images;

    private void OnEnable()
    {
        Client.Client.Instance.OnLobbyUpdate += UpdateImage;
    }

    private void OnDisable()
    {
        Client.Client.Instance.OnLobbyUpdate -= UpdateImage;
    }

    public void OnButtonClick()
    {
        lobbyPacket.isReady = !lobbyPacket.isReady;
        Client.Client.Instance.SendPacket(lobbyPacket);
    }

    private void UpdateImage( bool isReady, string id )
    {
        if(idToImage.ContainsKey(id))
        {
            idToImage[id].color = idToImage[id].color == Color.red ? Color.green : Color.red;//if red make it green and vice versa
        }
        else
        {
            idToImage.Add(id, images[idToImage.Count - 1]);
            idToImage[id].color= Color.green;
        }
    }

}