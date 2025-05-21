using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public Button btn;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;



    public UIInventroy inventroy;

    public int idx;
    public bool equpped;
    public int quantity;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = equpped;
    }

    public void Set()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if(outline != null)
        {
            outline.enabled = equpped;
        }
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);   
        quantityText.text = string.Empty;
    }

    public void onClickBtn()
    {
        inventroy.SelectItem(idx);
    }
}
