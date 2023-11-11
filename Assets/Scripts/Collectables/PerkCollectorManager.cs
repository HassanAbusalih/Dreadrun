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

    public Sprite blank;

    public Image[] AbilityIcon;
    public TextMeshProUGUI[] cooldownText;

    private void Start()
    {
        player = GetComponent<Player>();
    }
    public bool AcquireablePerk(Perk perk)
    {
        return (perk.unique && !acquiredPerks.Contains(perk)) || !perk.unique;
    }

    public void AcquirePerk(Perk perk)
    {
        INeedUI needUI = perk.ApplyPlayerBuffs(player);
        if (needUI != null)
        {
            for (int i = 0; i < AbilityIcon.Length && i < cooldownText.Length; i++)
            {
                if (AbilityIcon[i].sprite == blank)
                {
                    AbilityIcon[i].sprite = perk.icon;
                    float abilityCooldown = perk.FetchCooldown();
                    cooldownText[i].text = abilityCooldown.ToString();

                    if (AcquireablePerk(perk)) 
                    {
                        needUI.OnCoolDown += ()=> HandleCD(perk.FetchCooldown(), i);
                        break;
                    }
                }
            }
        } 
        acquiredPerks.Add(perk);
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
    private void HandleCD(float cooldown, int index)
    {
        StartCoroutine(StartCD(cooldown, index));
    }

    private IEnumerator StartCD(float cooldown, int index)
    {
        float timer = cooldown;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            cooldownText[index].text = timer.ToString("F1");
            //cooldownBoxes???[index].fillAmount = timer / cooldown;
            yield return null;
        }
        cooldownText[index].text = "0.0";
    }
}