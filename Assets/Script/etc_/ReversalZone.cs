using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class ReversalZone : MonoBehaviour, IReversalAble
{
    public void Reversal(IReversalObj reversalObj)
    {
        reversalObj.ReversalObj();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IReversalObj obj))
            Reversal(obj);
    }
}
