using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUISwitcher : MonoBehaviour
{
    [Header("Weapon Sprite Settings")]
    Image weaponSlot;
    Image weaponImageHolder;
    [SerializeField] Sprite blankSprite;
    [SerializeField] Color emptyWeaponColor;
    Color defaultColor;

    [Header("Description Panel Settings")]
    GameObject descriptionPanel;
    TextMeshProUGUI weaponDescriptionText;
    string currentWeaponDescription;

    [SerializeField] Color commonColor;
    [SerializeField] Color rareColor;
    [SerializeField] Color legendaryColor;

    [SerializeField] Dictionary<ItemRarityTypes, Color> itemRarityToColor = new Dictionary<ItemRarityTypes, Color>();


    private void OnEnable()
    {
        PlayerWeapon.WeaponPickedUpOrDropped += ChangeWeaponSpriteOnPickUpOrDrop;
    }

    private void Awake()
    {
        InitializeReferences();
        descriptionPanel.SetActive(false);
        weaponImageHolder.sprite = blankSprite;
        SetWeaponSlotColor(emptyWeaponColor);
    }

    void InitializeReferences()
    {
        weaponSlot = transform.GetComponent<Image>();
        weaponImageHolder = transform.GetChild(0).GetComponent<Image>();
        descriptionPanel = transform.GetChild(2).gameObject;
        weaponDescriptionText = descriptionPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        currentWeaponDescription = weaponDescriptionText.text;
        defaultColor = weaponSlot.color;
        SetWeaponSlotColor(emptyWeaponColor);

        itemRarityToColor.Add(ItemRarityTypes.empty,emptyWeaponColor);
        itemRarityToColor.Add(ItemRarityTypes.Common, commonColor);
        itemRarityToColor.Add(ItemRarityTypes.Rare, rareColor);
        itemRarityToColor.Add(ItemRarityTypes.Legendary, legendaryColor);
    }

    private void OnDisable()
    {
        PlayerWeapon.WeaponPickedUpOrDropped -= ChangeWeaponSpriteOnPickUpOrDrop;
    }

    void ChangeWeaponSpriteOnPickUpOrDrop(PlayerWeapon _weapon)
    {
        if (!_weapon.pickedUp)
        {
            weaponImageHolder.sprite = blankSprite;
            SetWeaponSlotColor(emptyWeaponColor);
            currentWeaponDescription = "No Weapon Currently Equipped";
            return;
        }
        weaponImageHolder.sprite = _weapon.weaponIcon;
        currentWeaponDescription = _weapon.weaponDescription;
        SetWeaponSlotColor(itemRarityToColor[_weapon.rarityType]);
    }

    public void ShowWeaponDescription()
    {
        descriptionPanel.SetActive(true);
        weaponDescriptionText.text = currentWeaponDescription;
    }

    public void HideWeaponDescription()
    {
        descriptionPanel.SetActive(false);
    }

    void SetWeaponSlotColor(Color _newColor)
    {
        weaponSlot.color = _newColor;
    }

}
