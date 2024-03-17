using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Slot[] slot = new Slot[5];
    public Player owner;
    public void QuickItemUse()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            slot[0].UseItem();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            slot[1].UseItem();
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            slot[2].UseItem();
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            slot[3].UseItem();
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            slot[4].UseItem();
    }
    public void AddItem(Item iitem)
    {
        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].item == null) 
            {
                slot[i].SetItem(iitem);
                return;
            }
        }
    }
    private void Update()
    {
        QuickItemUse();
    }
}
