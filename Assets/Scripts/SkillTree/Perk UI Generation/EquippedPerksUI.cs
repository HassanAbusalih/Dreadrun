
using System.Collections.Generic;
using UnityEngine;

public class EquippedPerksUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PerkUI perkUIPrefab;
    [SerializeField] RectTransform spawnStartPosition;

    [Header("perk UI Spawn Settings")]
    [SerializeField] Vector2 cellSize;
    [SerializeField] int maxCellsPerRow;
    [SerializeField] float XOffset;
    [SerializeField] float YOffset;

    List<Perk> TotalPerksUiMade = new List<Perk>();
    PerkSelector perkSelector;
    int rowIndex = 0;
    int columnIndex = 0;

    private void OnEnable()
    {
        perkSelector = FindObjectOfType<PerkSelector>();
        if(perkSelector != null)
        perkSelector.UpdateEquippedPerkUi += SpawnAndSetItemUI;
    }

    private void OnDisable()
    {
        if(perkSelector != null)
        perkSelector.UpdateEquippedPerkUi -= SpawnAndSetItemUI;
    }

    void SpawnAndSetItemUI(Perk _perkUIToSpawn)
    {
        if(perkSelector == null ||perkUIPrefab == null || spawnStartPosition ==null ) return;
        if (TotalPerksUiMade.Contains(_perkUIToSpawn)) return;
        if (rowIndex >= maxCellsPerRow) { rowIndex = 0; columnIndex++; }

        Vector3 cellOffset = new Vector3(XOffset * rowIndex, YOffset * columnIndex, 0);
        Vector3 cellSpawnPosition = spawnStartPosition.position + cellOffset;

        PerkUI _newUIPerk = Instantiate(perkUIPrefab, cellSpawnPosition, Quaternion.identity);
        _newUIPerk.transform.SetParent(spawnStartPosition.transform);
        _newUIPerk.GetComponent<RectTransform>().localScale = cellSize;
        _newUIPerk.SetPerk(_perkUIToSpawn,perkSelector);

        TotalPerksUiMade.Add(_perkUIToSpawn);
        rowIndex++;
    }
}
