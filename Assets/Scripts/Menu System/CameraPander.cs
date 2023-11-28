using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPander : MonoBehaviour
{
    [SerializeField] GameObject theCamera;
    [SerializeField] GameObject characterCustomView;
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject characterUI;

    private Vector3 startingPosition;
    private Vector3 currentPosition;

    [SerializeField] float speed;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = theCamera.gameObject.transform.position;
        currentPosition = startingPosition;
    }

    // Update is called once per frame
    void Update()
    {
        theCamera.gameObject.transform.position = Vector3.Lerp(theCamera.gameObject.transform.position, currentPosition, speed * Time.deltaTime);
    }

    public void MoveToMain()
    {
        currentPosition = startingPosition;
        mainMenuUI.SetActive(true);
        characterUI.SetActive(false);
    }

    public void MoveToCharacter()
    {
        currentPosition = characterCustomView.gameObject.transform.position;
        mainMenuUI.SetActive(false);
        characterUI.SetActive(true);
    }
}
