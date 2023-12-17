using UnityEngine;

public class CharacterCustomizationMenu : MonoBehaviour
{
    [SerializeField] MeshRenderer character;
    [SerializeField] Material[] characterSkins;
    [SerializeField] int currentSkin;
    [SerializeField] GameObject purchaseOption;

    public void NextSkin()
    {
        currentSkin = (currentSkin + 1) % characterSkins.Length;
        character.material = characterSkins[currentSkin];
        purchaseOption.SetActive(true);

        if (currentSkin == 0)
        {
            purchaseOption.SetActive(false);
        }
    }

    public void PreviousSkin()
    {
        currentSkin--;
        purchaseOption.SetActive(true);

        if (currentSkin < 0)
        {
            currentSkin += characterSkins.Length;
        }

        if(currentSkin == 0)
        {
            purchaseOption.SetActive(false);
        }
        character.material = characterSkins[currentSkin];
    }
}