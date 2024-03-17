using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCheck : MonoBehaviour
{
    public List<GameObject> torchList = new List<GameObject>();
    public List<GameObject> switchList = new List<GameObject>();
    public GameObject Pipe;
    void Update()
    {
        bool isCheck = true;
        foreach (GameObject go in torchList)
            isCheck &= go.activeSelf;

        if (isCheck)
        {
            Pipe.AddComponent<Rigidbody2D>();
            Pipe.GetComponent<Rigidbody2D>().gravityScale = 10;
            for (int i = 0; i < switchList.Count; i++)
            {
                switchList[i].SetActive(false);
            }
        }
    }
}
