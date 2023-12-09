using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    PerkCollectorManager perkCollector;
    int rowIndex = 0;
    int columnIndex = 0;

    private void OnEnable()
    {
        perkCollector = FindObjectOfType<PerkCollectorManager>();
        if(perkCollector != null)
        perkCollector.UpdateEquippedPerkUi += SpawnAndSetItemUI;
    }

    private void OnDisable()
    {
        if(perkCollector != null)
        perkCollector.UpdateEquippedPerkUi -= SpawnAndSetItemUI;
    }

    void SpawnAndSetItemUI(Perk _perkUIToSpawn, Color borderColor)
    {
        if(perkCollector == null ||perkUIPrefab == null || spawnStartPosition ==null ) return;
        if (TotalPerksUiMade.Contains(_perkUIToSpawn)) return;
        if (rowIndex >= maxCellsPerRow) { rowIndex = 0; columnIndex++; }

        Vector3 cellOffset = new Vector3(XOffset * rowIndex, YOffset * columnIndex, 0);
        Vector3 cellSpawnPosition = spawnStartPosition.position + cellOffset;

        PerkUI _newUIPerk = Instantiate(perkUIPrefab, cellSpawnPosition, Quaternion.identity);
        _newUIPerk.transform.SetParent(spawnStartPosition.transform);
        _newUIPerk.GetComponent<RectTransform>().localScale = cellSize;
        if (borderColor != Color.white)
        {
            _newUIPerk.GetComponent<Image>().color = new(borderColor.r, borderColor.g, borderColor.b, 1);
        }
        _newUIPerk.SetPerk(_perkUIToSpawn, perkCollector);
        TotalPerksUiMade.Add(_perkUIToSpawn);
        rowIndex++;
    }
}
