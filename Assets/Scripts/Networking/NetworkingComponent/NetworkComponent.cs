using UnityEngine;

public class NetworkComponent : MonoBehaviour
{  
    public string ClientID = "";
    public string GameObjectId;


    public void SetIDs(string _clientID, string  _gameObjectID)
    {
        ClientID = _clientID;
        GameObjectId = _gameObjectID;
    }

}