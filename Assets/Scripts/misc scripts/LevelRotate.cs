using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
public class LevelRotate : MonoBehaviour
{
    private float[] xScales = { -1f, -1f };
    private float[] yRotations = {45, -45, 90f, -90f, 180f, -180f };

    public static Action<Transform, bool> GiveLevelDirectionToPlayer;

    LookAtObject[] dialogues;
    void Start()
    {
        Transform levelTransform = transform;
        
        float selectedXScale = xScales[Random.Range(0, xScales.Length)];
        float selectedYRotation = yRotations[Random.Range(0, yRotations.Length)];

        levelTransform.localScale = new Vector3(selectedXScale, levelTransform.localScale.y, levelTransform.localScale.z);
        levelTransform.localRotation = Quaternion.Euler(0f, selectedYRotation, 0f);

        GiveLevelDirectionToPlayer?.Invoke(levelTransform,true);

        // inverting dialogues rotations if scale is -1
        if(selectedXScale== -1)
        {
            dialogues =  FindObjectsOfType<LookAtObject>();
            foreach (LookAtObject dialogue in dialogues)
            {
                dialogue.invertLookAt = true;
            }
        }
    }
}