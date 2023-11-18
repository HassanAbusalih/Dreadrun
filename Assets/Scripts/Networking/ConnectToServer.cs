using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ConnectToServer : MonoBehaviour
{
    [SerializeField] Button connectButton;
    [SerializeField] TMP_InputField ipAddressInput;

    private void OnEnable()
    {
        connectButton.onClick.AddListener(()=> Client.Client.Instance.ConnectToServer(ipAddressInput.text));
        Client.Client.Instance.ConnectedToServerEvent += LoadIntoGame;
    }

    private void OnDestroy()
    {
        Client.Client.Instance.ConnectedToServerEvent -= LoadIntoGame;
    }

    void LoadIntoGame()
    {
       StartCoroutine(DelayToLoadScene());
    }

    IEnumerator DelayToLoadScene()
    {
        yield return new WaitUntil(() => Client.Client.Instance.networkComponent.ClientID != null);
        SceneManager.LoadScene(1);
        LobbyPacket lobbyPacket = new LobbyPacket(false, "", Client.Client.Instance.networkComponent.ClientID);

        Client.Client.Instance.SendPacket(lobbyPacket);
        Debug.LogError("I'm joining the lobby!");
    }
}
