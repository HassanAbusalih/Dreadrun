using System.Collections;
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
        while (Client.Client.Instance.networkComponent.ClientID == "" || Client.Client.Instance.networkComponent.ClientID == "0")
        {
            yield return null;
        }
        SceneManager.LoadScene(1);
        string clientID = Client.Client.Instance.networkComponent.ClientID;
        LobbyPacket lobbyPacket = new LobbyPacket(false, "", clientID);
        Debug.LogError($"My client ID is {clientID}");
        Client.Client.Instance.SendPacket(lobbyPacket.Serialize());
        Debug.LogError("I'm joining the lobby!");
    }
}