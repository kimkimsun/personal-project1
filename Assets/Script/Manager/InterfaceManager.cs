using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IAttackable
{
    void Attack(IHitable hitobj);
    float Damage
    {
        get;
        set;
    }
}
public interface IHitable
{
    void Hit(float damage);
}
public interface IReversalAble
{
    void Reversal(IReversalObj reversalObj);
}
public interface IReversalObj
{
    void ReversalObj();
}
public class InterfaceManager
{
    
}