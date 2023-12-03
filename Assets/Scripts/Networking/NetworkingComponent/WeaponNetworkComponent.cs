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
        if (gameObject.transform.parent != null)
        {
            if (Client.Instance.isHost == false)
            {
                Destroy(gameObject);
            }
            else
            {
                Guid guid = Guid.NewGuid();
                string name = gameObject.name;
                string clone = "(Clone)";
                name = name.Replace(clone, "");

                Client.Instance.SendPacket(new InstantiationPacket(name, transform.position, transform.rotation, guid.ToString()).Serialize());
                Destroy(gameObject);
            }
        }
    }
}
