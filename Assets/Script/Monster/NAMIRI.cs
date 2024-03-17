using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NAMIRI : Monster
{
    protected override void Start()
    {
        base.Start();
        this.Hp = 50;
    }
}