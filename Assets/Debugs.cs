using MatchSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugs : MonoBehaviour
{
    public float timer;

    void Update()
    {
        IncomeTimer();
    }

    private void IncomeTimer()
    {
        var startIncomeTimer = 10;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = startIncomeTimer;
            return;
        }
    }

}
