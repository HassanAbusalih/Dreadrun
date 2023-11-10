using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PerkCollectorManager : MonoBehaviour
{
    [SerializeField]
    List<Perk> acquiredPerks = new List<Perk>();
    private Player player;
    private PerkSelector perkSelector;

    public Sprite blank;

    public Image[] AbilityIcon;
    public TextMeshProUGUI[] cooldownText;

    private void Start()
    {
        player = GetComponent<Player>();
        perkSelector = FindObjectOfType<PerkSelector>();
    }
    public bool AcquireablePerk(Perk perk)
    {
        return (perk.unique && !acquiredPerks.Contains(perk)) || !perk.unique;
    }

    public bool RequiresUI(Perk perk)
    {
        return (perk.needsUI);
    }
    public void AcquirePerk(Perk perk)
    {
        acquiredPerks.Add(perk);
        if (RequiresUI(perk))
        {
            for (int i = 0; i < AbilityIcon.Length && i < cooldownText.Length; i++)
            {
                if (AbilityIcon[i].sprite == blank)
                {
                    AbilityIcon[i].sprite = perk.icon;
                    float abilityCooldown = perk.FetchCooldown();
                    cooldownText[i].text = abilityCooldown.ToString();
                    break;
                }
            }
        }
      

    }
    private void OnTriggerEnter(Collider other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();
        if (collectable != null)
        {
            Perk perk = collectable.Collect() as Perk;
           

            if (perk != null && AcquireablePerk(perk)) 
            {
                AcquirePerk(perk);
                perk.ApplyPlayerBuffs(player);
                Destroy(other.gameObject);
            }
        }
    }
}