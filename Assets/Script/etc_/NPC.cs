using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public enum NPCNAME
{
    ACTIONMASK,
    TOTEMA,
    GUNDAM,
}
public class NPC : MonoBehaviour
{
    private int id;
    [SerializeField]private NPCNAME NPCType;
    [SerializeField] private Item item;

    public int Id
    {
        get => id;
        set 
        { 
            id = value; 
        }
    }
    public Item Item
    {
        get => item;
        set => item = value;
    }
    private void Start()
    {
        switch (NPCType)
        {
            case NPCNAME.ACTIONMASK:
                id = 1;
                break;
            case NPCNAME.TOTEMA:
                id = 2;
                break;
            case NPCNAME.GUNDAM:
                id = 3;
                break;
        }
    }
}