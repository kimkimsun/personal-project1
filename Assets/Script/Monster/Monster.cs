using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Monster : MonoBehaviour, IAttackable, IHitable
{
    protected readonly int hitLayer = 9;
    protected readonly int originLayer = 3;
    protected Color originalColor;
    protected Color hitColor;
    protected float hp;
    protected float damage;
    protected Rigidbody2D monsterRb;

    [SerializeField] protected float customHpAmount;
    [SerializeField] protected GameObject entranceZone;
    [SerializeField] protected MonsterMove move;
    [SerializeField] protected Image hpBar;
    [SerializeField] protected List<GameObject> DestroyObj = new List<GameObject>();
    [SerializeField] protected List<GameObject> DropItem = new List<GameObject>();

    public float Damage
    {
        get => damage;
        set 
        { 
            damage = value; 
        }
    }
    public float Hp
    {
        get => hp;
        set
        {
            hp = value;
            hpBar.fillAmount = hp;
            if (hp <= 0)
            {
                for (int i = 0; i < DropItem.Count; i++)
                {
                    Instantiate(DropItem[i], transform.position, transform.rotation);
                }
                for (int i = 0; i < DestroyObj.Count; i++)
                {
                    Destroy(DestroyObj[i]);
                }
                Destroy(this.gameObject);
            }
        }
    }
    protected virtual void Start()
    {
        damage = 1f;
        hpBar.fillAmount = hp/customHpAmount;
        monsterRb = GetComponent<Rigidbody2D>();
        originalColor = new Color(1, 1, 1, 1);
        hitColor = new Color(1, 1, 1, 0.4f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IHitable target = collision.gameObject.GetComponent<IHitable>();
        if (target != null && collision.gameObject.GetComponent<Player2>() == null)
            Attack(target);
    }
    public void Hit(float damage)
    {
        Hp -= damage;
        gameObject.layer = hitLayer;
        gameObject.transform.GetChild(0).gameObject.layer = hitLayer;
        monsterRb.AddRelativeForce(Vector2.up * 3, ForceMode2D.Impulse);
        monsterRb.AddRelativeForce(Vector2.left * 3, ForceMode2D.Impulse);
        StartCoroutine(ChangeColor());
    }
    public IEnumerator ChangeColor()
    {
        GetComponent<SpriteRenderer>().color = hitColor;
        gameObject.layer = hitLayer;
        yield return new WaitForSeconds(1.5f);
        GetComponent<SpriteRenderer>().color = originalColor;
        gameObject.layer = originLayer;
        gameObject.transform.GetChild(0).gameObject.layer = originLayer;
        gameObject.transform.GetChild(0).gameObject.layer = originLayer;
    }
    public void Attack(IHitable hitobj)
    {
        hitobj.Hit(damage);
    }
}