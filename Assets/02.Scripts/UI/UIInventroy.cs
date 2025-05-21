using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventroy : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPannel;

    [Header("Select Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDes;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;

    public GameObject useBtn;
    public GameObject equipBtn;
    public GameObject unEquipBtn;
    public GameObject dropBtn;

  //  private playercon

    // Start is called before the first frame update
    void Start()
    {
        //ControllerColliderHit 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
