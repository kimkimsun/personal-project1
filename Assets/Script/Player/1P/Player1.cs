using System.Collections;
using UnityEngine;

public class Player1 : Player, IHitable
{
    private Item curItem;
    private Animator shinAnimator;
    private float radius = 1f;
    [SerializeField] private Inventory inven;
   
    protected void Update()
    {
        Collider2D hit;
        hit = Physics2D.OverlapCircle(transform.position, radius, 1 << 7);
       if (hit != null)
       {
           if (Input.GetKeyDown(KeyCode.E))
           {
               playerMove.IsMove = false;
               GameManager.Instance.TalkPanel.gameObject.SetActive(true);
               if (GameManager.Instance.TalkEnd(hit.gameObject.GetComponent<NPC>().Id, hit.gameObject.GetComponent<NPC>()))
                    Destroy(hit.gameObject);
           }
           if (Input.GetKeyDown(KeyCode.Escape))
           {
               GameManager.Instance.TalkPanel.gameObject.SetActive(false);
               playerMove.IsMove = true;
               GameManager.Instance.talkIndex = 0;
            }
       }

        if(Input.GetKey(playerMove.rightKey))
        {
            shinAnimator.SetBool("Run", true);
            if(Input.GetKey(playerMove.jumpKey))
            {
                shinAnimator.SetBool("Run", false);
                shinAnimator.SetBool("Jump", true);
            }
        }
        if (Input.GetKey(playerMove.leftKey))
        {
            shinAnimator.SetBool("Run", true);
            if (Input.GetKey(playerMove.jumpKey))
            {
                shinAnimator.SetBool("Run", false);
                shinAnimator.SetBool("Jump", true);
            }
        }
        if (Input.GetKeyUp(playerMove.rightKey))
        {
            shinAnimator.SetBool("Run", false);
        }
        if (Input.GetKeyUp(playerMove.leftKey))
        {
            shinAnimator.SetBool("Run", false);
        }
        else if (playerRb.velocity.y != 0)
            shinAnimator.SetBool("Jump", true);
        else if (playerRb.velocity.y == 0)
            shinAnimator.SetBool("Jump", false);

        if (curItem != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                inven.AddItem(curItem);
                curItem.transform.SetParent(transform);
                curItem.transform.position = curItem.transform.parent.position;
                curItem.gameObject.SetActive(false);
            }
        }
    }
    protected override void Start()
    {
        base.Start();
        shinAnimator = GetComponent<Animator>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Item>() != null)
            curItem = collision.GetComponent<Item>();
        if (collision.GetComponent<Item>() && collision.GetComponent<Item>().Type == ITEM_TYPE.POTION)
        {
            collision.GetComponent<Item>().Use();
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Item>() != null)
            curItem = null;
    }
    public void Hit(IAttackable atkObj)
    {
        Hp -= atkObj.Damage;
        gameObject.layer = 9;
        HitFunction();
    }
    protected void HitFunction()
    {
        playerSprite.color = hitColor;
        playerRb.AddRelativeForce(Vector2.right * 4, ForceMode2D.Impulse);
        playerRb.AddRelativeForce(Vector2.up * 2, ForceMode2D.Impulse);
        StartCoroutine(ChangeColor());
    }
    IEnumerator ChangeColor()
    {
        yield return new WaitForSeconds(1.5f);
        playerSprite.color = originalColor;
        gameObject.layer = 8;
    }

    public override void Hit(float damage)
    {
        Hp -= damage;
        playerRb.AddRelativeForce(Vector2.right * 5, ForceMode2D.Impulse);
    }
}