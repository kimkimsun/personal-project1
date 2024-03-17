using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player2 : Player
{ 

    private void Update()
    {
        if(transform.localScale.x < 0.4)
        {
            playerMove.distance = 1.5f;
        }
        if(transform.localScale.x > 0.5)
        {
            playerMove.distance = 3.2f;
        }
    }
}