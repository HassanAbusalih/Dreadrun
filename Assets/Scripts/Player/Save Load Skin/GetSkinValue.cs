using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetSkinValue : MonoBehaviour
{
    [SerializeField] Renderer playerModel;
   
    public void KeepSkinValue()
    {
        Material SkinToKeep = playerModel.material;
        StaticMaterial.materialToKeep = SkinToKeep;
        Debug.Log("Skin Saved");
    }
}