using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class TriggerHandler : MonoBehaviour
{
    public string nextSceneName;
    public KeyCode interactKey = KeyCode.E;
    bool entered = false;
    bool completedTutorial = false;


    private void Update()
    {
        if(Input.GetKeyDown(interactKey)&& entered)
        {
            completedTutorial = true;
            SaveDataToJson();
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            entered = true;
        }
    }

    private void SaveDataToJson()
    {
        InteractData data = new InteractData();
        data.tutorialstate = completedTutorial;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText("interactionData.json", json);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

[System.Serializable]
public class InteractData
{
    public bool tutorialstate;
}
