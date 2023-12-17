using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPurchase : MonoBehaviour
{
    [SerializeField] Text coins;
    [SerializeField] int amount;
    [SerializeField] GameObject purchaseComplete;
    [SerializeField] GameObject purchased;
    [SerializeField] GameObject startGame;
    // Start is called before the first frame update
    private void Start()
    {
        coins = GameObject.Find("moneyAmount").GetComponent<Text>();
        purchaseComplete.SetActive(false);
    }

    public void Purchase()
    {
        amount = 0;
        coins.text = "" + amount;
        purchaseComplete.SetActive(true);
        purchased.SetActive(false);
        startGame.SetActive(true);
    }

}
