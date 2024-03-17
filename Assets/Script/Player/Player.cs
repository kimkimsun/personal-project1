using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IReversalObj, IHitable
{
    [SerializeField] protected PlayerMove playerMove;
    [SerializeField] protected Image[] hpImage = new Image[3];
    [SerializeField] protected Rigidbody2D playerRb;
    protected SpriteRenderer playerSprite;
    protected BoxCollider2D playerCollider;
    protected const int maxHp = 3;
    protected float hp = 3;
    protected Color originalColor = new Color(1, 1, 1, 1);
    protected Color hitColor = new Color(1, 1, 1, 0.4f);

    public PlayerMove PlayerMove
    {
        get => playerMove;
    }

    public float Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp > maxHp)
                hp = maxHp;
            else if (hp <= 0)
                Destroy(gameObject);
            for (int i = 0; i < hpImage.Length; i++)
                hpImage[i].gameObject.SetActive(false);
            for (int i = 0; i < hp; i++)
                hpImage[i].gameObject.SetActive(true);
        }
    }
    protected virtual void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        playerRb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
    }
    public void ReversalObj()
    {
        playerMove.IsGravity = !playerMove.IsGravity;
        transform.localScale = new Vector2(transform.localScale.x, (transform.localScale.y * -1));
        playerRb.gravityScale *= -1;
    }
    public virtual void Hit(float damage)
    {
        playerRb.AddRelativeForce(Vector2.right * 5 * Time.deltaTime, ForceMode2D.Impulse);
    }
}