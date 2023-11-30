using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SentencePool", menuName = "ScriptableObjects/SentencePool", order = 1)]
public class SentencePool: ScriptableObject
{
    public List<string> sentencePool = new List<string>();
    [HideInInspector] public List<string> temporaryPool = new List<string>();
}
