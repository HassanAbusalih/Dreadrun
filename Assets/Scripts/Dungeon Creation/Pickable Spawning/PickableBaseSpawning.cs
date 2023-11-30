using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableBaseSpawning : MonoBehaviour
{
    public void AddChildrenToSpawnPointsList(Transform _spawnPointsParent,List<Transform>_spawnPoints)
    {
        if (_spawnPointsParent == null) { return; }
        foreach (Transform child in _spawnPointsParent)
        {
            if (_spawnPoints.Contains(child)) { continue; }
            _spawnPoints.Add(child);
        }
        _spawnPoints.RemoveAll(item => item == null);
    }
    protected bool SpawnObjectAtRandomSpawnPoint(GameObject _pickableToSpawn,List<Transform> _spawnPoints)
    {
        // if there are no spawn points left, then return (this is to avoid index out of range error)
        if (_spawnPoints.Count == 0)
        { Debug.LogWarning("There is no more pickable spawn points,so cannot spawn more, try changing spawn settings "); return false; }

        int _randomPickableSpawnPointIndex = Random.Range(0, _spawnPoints.Count);
        Vector3 _randomSpawnPosition = _spawnPoints[_randomPickableSpawnPointIndex].position;

        GameObject _pickableSpawned = Instantiate(_pickableToSpawn, _randomSpawnPosition, Quaternion.identity);
        _pickableSpawned.transform.parent = this.transform;
         Transform _pickableTransform = _spawnPoints[_randomPickableSpawnPointIndex];
        _spawnPoints.RemoveAt(_randomPickableSpawnPointIndex);
        Destroy(_pickableTransform.gameObject);
        return true;
    }
}

