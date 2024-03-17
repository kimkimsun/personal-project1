using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHead : MonoBehaviour, IHitable
{
    public void Hit(float damage)
    {
        if(TryGetComponent<Hooni>(out Hooni hooni))
            hooni.Hp -= damage;
        else if (TryGetComponent<NAMIRI>(out NAMIRI namiri))
            namiri.Hp -= damage;
    }
}