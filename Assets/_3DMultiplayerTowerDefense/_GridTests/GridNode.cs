using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridNode : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Image NodeImage; 


    public void OnSelect(BaseEventData eventData)
    {
        NodeImage.color = Color.Lerp(NodeImage.color, Color.magenta, Time.deltaTime * 5);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        NodeImage.color = Color.white;
    }
}
