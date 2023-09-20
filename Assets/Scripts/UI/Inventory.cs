using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    public void Awake()
    {
        /*if(instance != null)
        {
            Destroy(gameObject);
            return;
        }*/
        instance = this;
    }
    #endregion

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;
    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;
    private int slotCnt;
    public int SlotCnt
    {
        get => slotCnt;
        set{
            slotCnt =value;
            onSlotCountChange.Invoke(slotCnt); 
        }
    }
    public List<Item> items = new List<Item>();

    void Start()
    {
        SlotCnt = 10;
    }

    public bool Additem(Item _item)
    {
        if(items.Count < SlotCnt )
        {
            items.Add(_item);
            if(onChangeItem != null)
                onChangeItem.Invoke();
            return true;
        }
        return false;
    }
}

