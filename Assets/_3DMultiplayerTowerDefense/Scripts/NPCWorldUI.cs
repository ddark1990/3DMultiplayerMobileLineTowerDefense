using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class NPCWorldUI : MonoBehaviour
{
    public Image healthImg;

    public float startHealth;

    void Start()
    {
        startHealth = GetComponentInParent<Creep>().Health;
    }

    void FixedUpdate()
    {
        healthImg.fillAmount = GetComponentInParent<Creep>().Health / startHealth;
    }
}
