using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponIDs", menuName = "ScriptableObjects/WeaponIDs")]
public class WeaponIDs : ScriptableObject
{
    public Dictionary<int, GameObject> WeaponIDsDictionary = new Dictionary<int, GameObject>();
    [SerializeField] List<GameObject> weaponPrefabs = new List<GameObject>();

    public void InitializeWeaponIDsDictionary()
    {
        WeaponIDsDictionary.Clear();
        for (int i = 1; i <= weaponPrefabs.Count; i++)
        {
            WeaponIDsDictionary.Add(i, weaponPrefabs[i-1]);
        }
    }

    public GameObject GetAndSpawnWeaponBasedOnWeaponID(int _ID, Vector3 _positionToSpawn)
    {
        GameObject _weaponToSpawn = WeaponIDsDictionary[_ID];
        GameObject spawnedWeapon = Instantiate(_weaponToSpawn, _positionToSpawn, Quaternion.identity);
        return _weaponToSpawn;

    }

}
