using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridNode : MonoBehaviour
{
    public Image NodeImage;


    private void OnMouseOver()
    {
        NodeImage.color = Color.Lerp(NodeImage.color, Color.magenta, Time.deltaTime * 5);
    }

    private void OnMouseExit()
    {
        NodeImage.color = Color.white;
    }
}
