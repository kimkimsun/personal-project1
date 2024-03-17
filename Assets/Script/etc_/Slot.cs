using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Inventory slotOwner;
    public TextMeshProUGUI itemExplanation;
    public Image image;
    public Image explanationImage;
    public Item item;
    public void SetItem(Item iitem)
    {
        item = iitem;      
        if(item == null)
            image.sprite = null;
        else
        {
            image.sprite = item.sprite;
            itemExplanation.text = item.explanation;
            item.owenrSlotImage = image;
        }
    }

    public void UseItem()
    {
        if (item != null)
            item.Use();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image.sprite != null)
            explanationImage.gameObject.SetActive(true);
        else
            return;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        explanationImage.gameObject.SetActive(false);
    }
}