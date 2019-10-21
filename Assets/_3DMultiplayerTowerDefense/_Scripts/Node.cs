using UnityEngine;
using GoomerScripts;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Node : MonoBehaviourPunCallbacks
{
    public enum NodeOwner { Player1, Player2, Player3, Player4, Player5, Player6, Player7, Player8};

    [Header("Hover Options")]
    public Color hoverColor;
    [Range(0, 1)] public float hoverRaiseHeight = 0.01f;
    [Range(0, 25)] public float hoverRaiseSpeed = 8f;

    [Header("Selection Debug")]
    public PhotonPlayer owner;
    public bool isSelectable = true;
    public bool isSelected = false;
    public bool isOccupied = false;
    public NodeOwner nodeOwner;

    [Header("Cache")]
    public Renderer rend;
    //public Image ImageColor;

    [HideInInspector]public Color startColor;
    //public Color startImageColor;
    Vector3 startPos;
    UnityAction placeTower;


    void Start()
    {
        startColor = rend.material.color;
        //startImageColor = ImageColor.color;
        startPos = transform.position;

        if(!isSelectable)
        {
            rend.material.color = Color.grey;
        }
    }

    private void Update()
    {
        SelecNode();
    }

    public void SetNodeSelectability()
    {
        if(PhotonNetwork.LocalPlayer == owner.photonView.Owner)
        {
            isSelectable = true;
            rend.material.color = startColor;
        }
        else
        {
            rend.material.color = Color.grey;
        }

        this.enabled = false;
    }

    public void SelecNode()
    {
        if (Input.touchCount > 0)
            return;

        if(!isSelectable)
        {
            return;
        }
        else
        {
            if (SelectionManager.Instance.currentlySelectedObject == gameObject)
            {
                if (isSelected)
                {
                    HighlightNode();
                    //Debug.Log("Selected " + selMan.currentlySelectedObject);
                    return;
                }
                BuildTowerUIController.Instance.TowerBuyOpenMenu();
                HighlightNodeSound();
                isSelected = true;
            }
            else
            {
                if (!isSelected)
                {
                    DehighlightNode();
                    //Debug.Log("Deselected " + selMan.currentlySelectedObject);
                    return;
                }
                BuildTowerUIController.Instance.TowerBuyCloseMenu();
                DehighlightNodeSound();
                isSelected = false;
            }
        }
    }

    public void HighlightNode()
    {
        //Debug.Log("Highlighting " + this);
        Color lerpedColor = Color.Lerp(rend.material.color, hoverColor, Time.deltaTime * 5); //color lerp cache
        rend.material.color = lerpedColor; //color change

        //if(ImageColor != null)
        //{
        //    Color _lerpedColor = Color.Lerp(ImageColor.color, hoverColor, Time.deltaTime * 5); //color lerp cache
        //    ImageColor.color = _lerpedColor; 
        //}

        transform.position = Vector3.Lerp(transform.position, startPos + new Vector3(0, hoverRaiseHeight, 0), Time.deltaTime * hoverRaiseSpeed); //node lerp
    }

    public void DehighlightNode()
    {
        //Debug.Log("Dehighlighting " + this);
        rend.material.color = Color.Lerp(rend.material.color, startColor, Time.deltaTime * 5);

        //if (ImageColor != null)
        //{
        //    Color _lerpedColor = Color.Lerp(ImageColor.color, startImageColor, Time.deltaTime * 5); //color lerp cache
        //}

        transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * hoverRaiseSpeed);
    }

    void HighlightNodeSound()
    {
        bool m_ToggleChange = true;
        if (m_ToggleChange)
        {
            AudioManager.AM.Play("HighlightNodeSound");
            m_ToggleChange = false;
        }
    }

    void DehighlightNodeSound()
    {
        bool m_ToggleChange = true;
        if (m_ToggleChange)
        {
            AudioManager.AM.Play("DehighlightNodeSound");
            m_ToggleChange = false;
        }
    }
}
