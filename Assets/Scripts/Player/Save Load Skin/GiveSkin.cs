using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveSkin : MonoBehaviour
{
    [SerializeField] Renderer playerSkin;
    void Start()
    {
        Material newMaterial = StaticMaterial.materialToKeep;
        playerSkin.material = newMaterial;
        Debug.Log("Skin Applied");
    }

  
}