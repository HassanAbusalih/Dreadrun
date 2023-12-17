using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemyProgression : MonoBehaviour
{
    [SerializeField] Tutorialprogression manager;

    private void OnDestroy()
    {
        manager.ConditionUpdate();
    }
}
