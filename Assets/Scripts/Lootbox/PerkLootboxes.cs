using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkLootboxes : MonoBehaviour
{
    [SerializeField] List<Perk> unlockablePerkPool;
    [SerializeField] bool opened;
    [SerializeField] bool entered;
    [SerializeField] UnlockedPerks unlocked;
    // Start is called before the first frame update
    void Start()
    {
        unlocked = FindObjectOfType<UnlockedPerks>();
        RemoveCommonPerks(unlockablePerkPool, unlocked.unlockedPerkpool);
    }

    void Update()
    {
        if (entered)
        {
            OpenLootBox();
        }
    }
    void RemoveCommonPerks(List<Perk> unlockable, List<Perk> unlocked)
    {
        unlockable.RemoveAll(poolA => unlocked.Contains(poolA));
        if (unlockablePerkPool.Count == 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<UnlockedPerks>())
        {
            entered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        entered = false;
    }

    void OpenLootBox()
    {
        if(Input.GetKeyDown(KeyCode.E)&& !opened)
        {
            int randomPerk = Random.Range(0, unlockablePerkPool.Count);
            Perk selectedPerk = unlockablePerkPool[randomPerk];
            unlocked.AddNewPerk(selectedPerk);
            opened = true;
        }
    }
}