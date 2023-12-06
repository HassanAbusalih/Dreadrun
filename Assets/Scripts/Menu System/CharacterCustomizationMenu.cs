using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class CharacterCustomizationMenu : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    [SerializeField] int currentCharacter;


    public void NextCharacter()
    {
        characters[currentCharacter].SetActive(false);
        currentCharacter = (currentCharacter + 1) % characters.Length;
        characters[currentCharacter].SetActive(true);
    }

    public void PreviousCharacter()
    {
        characters[currentCharacter].SetActive(false);
        currentCharacter--;

        if(currentCharacter < 0)
        {
            currentCharacter += characters.Length;
        }

        characters[currentCharacter].SetActive(true);
    }
}
