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


    private void OnEnable()
    {
        PlayerWeapon.weaponPickedUpOrDropped += ChangeWeaponSpriteOnPickUpOrDrop;
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
    }

    private void OnDisable()
    {
        PlayerWeapon.weaponPickedUpOrDropped -= ChangeWeaponSpriteOnPickUpOrDrop;
    }

    void ChangeWeaponSpriteOnPickUpOrDrop(PlayerWeapon _weapon)
    {
        if (_weapon == null)
        {
            weaponImageHolder.sprite = blankSprite;
            SetWeaponSlotColor(emptyWeaponColor);
            currentWeaponDescription = "No Weapon Currently Equipped";
            return;
        }
        weaponImageHolder.sprite = _weapon.weaponIcon;
        currentWeaponDescription = _weapon.weaponDescription;
        SetWeaponSlotColor(defaultColor);
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
