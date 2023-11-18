using NetworkingLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateClient : Client.Client
{
 /*   Color Color = new Color();

    protected override void Start()
    {
        base.Start();
        Color = Random.ColorHSV();
    }


    protected override void ReceivePacket(byte[] buffer, BasePacket _basePacket)
    {
        if (_basePacket.packetType == BasePacket.PacketType.Instantiation)
        {
            InstantiationPacket _instantiationPacket = new InstantiationPacket();
            _instantiationPacket = _instantiationPacket.Deserialize(buffer) as InstantiationPacket;

            GameObject _spawnedObject = Instantiate(Resources.Load("prefabs/"+_instantiationPacket.prefabName) as GameObject, _instantiationPacket.position, _instantiationPacket.rotation);
            _spawnedObject.GetComponent<MeshRenderer>().material.color = Color;

            Debug.Log(_spawnedObject.name +"Object  deserialized and Spawned successfully");
            return;
        }
        Debug.Log( "Object instantiation data was not recived");
    }*/
}
