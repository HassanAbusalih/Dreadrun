
using UnityEngine;


public class CursorSettings : MonoBehaviour
{
    [SerializeField] CursorLockMode cursorLockMode = CursorLockMode.None;
    [SerializeField] bool cursorVisible = true;

    private void Start()
    {
        Cursor.lockState = cursorLockMode;
        Cursor.visible = cursorVisible;
    }
}