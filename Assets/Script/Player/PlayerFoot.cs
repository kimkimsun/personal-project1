using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour, IAttackable
{
    float damage = 10;
    public float Damage
    {
        get => damage;
        set
        {
            damage = value;
        }
    }
    private void Start()
    {
        Damage = this.Damage;
    }
    public void Attack(IHitable hitobj)
    {
        hitobj.Hit(damage);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IHitable>() != null && collision.GetComponent<Player>() == null)
        {
            Attack(collision.gameObject.GetComponent<IHitable>());
            transform.parent.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * 4 * Time.deltaTime, ForceMode2D.Impulse);
            transform.parent.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * 5 * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}