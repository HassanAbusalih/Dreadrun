using UnityEngine;
public class LevelRotate : MonoBehaviour
{
    private float[] xScales = { -1f, 1f };
    private float[] yRotations = {45, -45, 90f, -90f, 180f, -180f };
    void Start()
    {
        Transform levelTransform = transform;
        float selectedXScale = xScales[Random.Range(0, xScales.Length)];
        float selectedYRotation = yRotations[Random.Range(0, yRotations.Length)];

        levelTransform.localScale = new Vector3(selectedXScale, levelTransform.localScale.y, levelTransform.localScale.z);
        levelTransform.localRotation = Quaternion.Euler(0f, selectedYRotation, 0f);
    }
}