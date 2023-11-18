using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateServer : Server.Server
{
   /* [SerializeField] GameObject prefabToSpawn;
    string prefabName;


    protected override void Start()
    {
        base.Start();
        prefabName = prefabToSpawn.name;
    }
    protected override void SendPositionPacket(int _clientIndex)
    {
        Vector3 _spawnPosition = new Vector3(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 5));
        Quaternion _spawnRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        byte[] serializedPosition = new InstantiationPacket(prefabName,_spawnPosition,_spawnRotation).SerializeInstantiationData();
        clients[_clientIndex].socket.Send(serializedPosition);
    }*/
}
