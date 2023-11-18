using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    BoolPacket lobbyPacket;

    public void OnButtonClick()
    {
        lobbyPacket.isReady = true;
    }
}