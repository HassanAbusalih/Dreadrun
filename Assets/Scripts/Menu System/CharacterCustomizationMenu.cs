using UnityEngine;
using UnityEngine.UI;


public class CharacterCustomizationMenu : MonoBehaviour
{
    [SerializeField] Image outfitImage;
    [SerializeField] Image weaponImage;
    [SerializeField] Text categoryText;
    [SerializeField] Sprite[] outfitSprites;
    [SerializeField] Sprite[] weaponSprites;

    private int currentCategoryIndex = 0;
    private int currentOutfitIndex = 0;
    private int currentWeaponIndex = 0;

    private void Start()
    {
        UpdateCategoryText();
        UpdateOutfitImage();
        UpdateWeaponImage();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }

    public void NextCategory()
    {
        currentCategoryIndex = (currentCategoryIndex + 1) % 2;
        UpdateCategoryText();
        UpdateOutfitImage();
        UpdateWeaponImage();
    }

    public void PreviousCategory()
    {
        currentCategoryIndex = (currentCategoryIndex + 1) % 2;
        UpdateCategoryText();
        UpdateOutfitImage();
        UpdateWeaponImage();
    }

    public void NextItem()
    {
        if (currentCategoryIndex == 0)
        {
            currentOutfitIndex = (currentOutfitIndex + 1) % outfitSprites.Length;
            UpdateOutfitImage();
        }
        else
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weaponSprites.Length;
            UpdateWeaponImage();
        }
    }

    public void PreviousItem()
    {
        if (currentCategoryIndex == 0)
        {
            currentOutfitIndex = (currentOutfitIndex + outfitSprites.Length - 1) % outfitSprites.Length;
            UpdateOutfitImage();
        }
        else
        {
            currentWeaponIndex = (currentWeaponIndex + weaponSprites.Length - 1) % weaponSprites.Length;
            UpdateWeaponImage();
        }
    }

    private void UpdateCategoryText()
    {
        if (currentCategoryIndex == 0)
        {
            categoryText.text = "Outfit";
        }
        else
        {
            categoryText.text = "Weapon";
        }
    }

    private void UpdateOutfitImage()
    {
        outfitImage.sprite = outfitSprites[currentOutfitIndex];
    }

    private void UpdateWeaponImage()
    {
        weaponImage.sprite = weaponSprites[currentWeaponIndex];
    }
}
