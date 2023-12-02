using ClientLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponNetworkComponent : NetworkComponent
{
    public bool assigned { get; private set; }
    private void Start()
    {
        if (Client.Instance.isHost == false)
        {
            Destroy(gameObject);
        }
        else
        {
            Guid guid = Guid.NewGuid();
            Client.Instance.SendPacket(new InstantiationPacket(gameObject.name, transform.position, transform.rotation, guid.ToString()).Serialize());
            Destroy(gameObject);
        }
        
    }
}
