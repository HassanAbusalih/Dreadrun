using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;
    [SerializeField]
    private List<GameObject> ObjectPool = new List<GameObject>();
    [SerializeField]
    private int amountToPool = 30;
    [SerializeField]
    private GameObject mango;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            ObjectPool.Add(Instantiate(mango, transform.position, transform.rotation));
            ObjectPool[i].SetActive(false);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < ObjectPool.Count; i++)
        {
           if (!ObjectPool[i].activeInHierarchy)
            {
                return ObjectPool[i];
            }
        }
        return null;
    }
}
