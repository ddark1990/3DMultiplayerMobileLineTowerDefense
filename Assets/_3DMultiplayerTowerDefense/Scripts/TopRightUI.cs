using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopRightUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI incomeText;
    public TextMeshProUGUI goldText;

    void Start()
    {
        
    }

    void Update()
    {
        incomeText.text = GameManager.instance.playerIncome.ToString();
        timerText.text = GameManager.instance.incomeTimer.ToString("#");
        goldText.text = GameManager.instance.playerGold.ToString();
    }
}
