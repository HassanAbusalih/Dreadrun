using TMPro;
using UnityEngine;

public class RandomDialogue : MonoBehaviour
{
    public TextMeshPro textMeshPro;
    public SentencePool sentencePoolManager;

    private void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        DisplayRandomSentence();
    }

    private void DisplayRandomSentence()
    {
        if (sentencePoolManager.temporaryPool.Count == 0)
        {
            sentencePoolManager.temporaryPool.AddRange(sentencePoolManager.sentencePool);
        }
        int randomIndex = Random.Range(0, sentencePoolManager.temporaryPool.Count);
        string randomSentence = sentencePoolManager.temporaryPool[randomIndex];
        textMeshPro.text = randomSentence;
        sentencePoolManager.temporaryPool.RemoveAt(randomIndex);
    }
}
