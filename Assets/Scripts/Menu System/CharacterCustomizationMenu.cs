using UnityEngine;

public class CharacterCustomizationMenu : MonoBehaviour
{
    [SerializeField] MeshRenderer character;
    [SerializeField] Material[] characterSkins;
    [SerializeField] int currentSkin;
    [SerializeField] GameObject purchaseOption;
    [SerializeField] GameObject startGameButton;

    public void NextSkin()
    {
        currentSkin = (currentSkin + 1) % characterSkins.Length;
        character.material = characterSkins[currentSkin];
        purchaseOption.SetActive(true);
        startGameButton.SetActive(false);

        if (currentSkin == 0)
        {
            purchaseOption.SetActive(false);
            startGameButton.SetActive(true);
        }
    }

    public void PreviousSkin()
    {
        currentSkin--;
        purchaseOption.SetActive(true);
        startGameButton.SetActive(false);

        if (currentSkin < 0)
        {
            currentSkin += characterSkins.Length;
        }

        if(currentSkin == 0)
        {
            purchaseOption.SetActive(false);
            startGameButton.SetActive(true);
        }
        character.material = characterSkins[currentSkin];
    }
}