using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkComponent : MonoBehaviour
{
    
    public string ClientID = "";
    public string GameObjectId;


    private void Start()
    {
        GameObjectId = gameObject.GetInstanceID().ToString();
    }
}
