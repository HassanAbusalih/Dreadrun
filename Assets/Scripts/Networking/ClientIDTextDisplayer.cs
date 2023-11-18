using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientIDTextDisplayer : MonoBehaviour
{
    TextMeshPro clientIDText;
    NetworkComponent networkComponent;

    private void Awake()
    {
        clientIDText = GetComponent<TextMeshPro>();
        networkComponent = GetComponentInParent<NetworkComponent>();
    }

    private void Update()
    {
       // if(networkComponent.ClientID != 0)
        clientIDText.text = "Client ID: " + networkComponent.ClientID.ToString();
    }
}
