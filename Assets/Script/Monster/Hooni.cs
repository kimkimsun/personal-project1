using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooni : Monster
{ 
    protected override void Start()
    {
        base.Start();
        this.hp = 30;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            hpBar.gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(hp);
        }
    }
}