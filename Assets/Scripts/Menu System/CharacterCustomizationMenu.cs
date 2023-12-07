using UnityEngine;

public class CharacterCustomizationMenu : MonoBehaviour
{
    [SerializeField] MeshRenderer character;
    [SerializeField] Material[] characterSkins;
    [SerializeField] int currentSkin;

    public void NextSkin()
    {
        currentSkin = (currentSkin + 1) % characterSkins.Length;
        character.material = characterSkins[currentSkin];
    }

    public void PreviousSkin()
    {
        currentSkin--;

        if (currentSkin < 0)
        {
            currentSkin += characterSkins.Length;
        }

        character.material = characterSkins[currentSkin];
    }
}