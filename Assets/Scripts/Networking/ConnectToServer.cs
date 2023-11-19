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
        connectButton.onClick.AddListener(()=> ClientLibrary.Client.Instance.ConnectToServer(ipAddressInput.text));
        ClientLibrary.Client.Instance.ConnectedToServerEvent += LoadIntoGame;
    }

    private void OnDestroy()
    {
        ClientLibrary.Client.Instance.ConnectedToServerEvent -= LoadIntoGame;
    }

    void LoadIntoGame()
    {
       StartCoroutine(DelayToLoadScene());
    }

    IEnumerator DelayToLoadScene()
    {
        while (ClientLibrary.Client.Instance.networkComponent.ClientID == "" || ClientLibrary.Client.Instance.networkComponent.ClientID == "0")
        {
            yield return null;
        }
        SceneManager.LoadScene(1);
        while (ClientLibrary.Client.Instance.OnLobbyUpdate == null)
        {
            yield return null;
        }
    }
}