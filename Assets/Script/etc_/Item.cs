using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using static UnityEditor.Progress;


public abstract class ItemStrategy
{
    protected Item item;
    protected string explanation;
    public ItemStrategy(Item item)
    {
        this.item = item;
    }
    public abstract void Use();
}
public class ActionBeam : ItemStrategy
{
    bool isCool = true;
    float coolTime = 2;
    float damage = 15;
    public ActionBeam(Item item) : base(item)
    {
        item.coolTime = coolTime;
        item.Damage = damage;
        item.explanation = "이름 : 액션빔" + System.Environment.NewLine + "대미지 : 프레임당 1"
                      + System.Environment.NewLine + "지속시간 : 2초, 쿨타임 : 5초";
    }
    public override void Use()
    {
        if (isCool)
        {
            item.gameObject.SetActive(true);
            isCool = false;
            item.StartCoroutine(ActionBeamCoroutine(item.gameObject));
            item.owenrSlotImage.type = Image.Type.Filled;
            item.owenrSlotImage.fillAmount = 0;
        }
    }
    IEnumerator ActionBeamCoroutine(GameObject actionBeam)
    {
        item.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        item.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(2f);
        item.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        item.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        while (coolTime <= 5)
        {
            item.owenrSlotImage.fillAmount = coolTime / 5;
            yield return new WaitForSeconds(1f);
            coolTime++;
            item.coolTime = coolTime;
        }
        isCool = true;
        coolTime = 0;
        item.coolTime = coolTime;
        actionBeam.transform.position = actionBeam.transform.parent.position;
        actionBeam.SetActive(false);
    }
}
public class GundamPunch : ItemStrategy
{
    bool isCool = true;
    float coolTime = 2;
    float damage = 20;

    public GundamPunch(Item item) : base(item)
    {
        item.coolTime = coolTime;
        item.Damage = damage;
        item.explanation = "이름 : 건담 펀치" + System.Environment.NewLine + "대미지 : 10"
                      + System.Environment.NewLine + "투척 무기 , 쿨타임 : 5초";
    }

    public override void Use()
    {
        if (isCool)
        {
            item.gameObject.SetActive(true);
            item.rb.bodyType = RigidbodyType2D.Dynamic;
            item.rb.AddRelativeForce(item.player.transform.right * 30f, ForceMode2D.Impulse);
            item.gameObject.transform.SetParent(null);
            isCool = false;
            item.StartCoroutine(DestroyGundamPunch(item.gameObject));
            item.owenrSlotImage.type = Image.Type.Filled;
            item.owenrSlotImage.fillAmount = 0;
        }
    }
    IEnumerator DestroyGundamPunch(GameObject gundamPunch)
    {
       while (coolTime <= 5)
       {
           item.owenrSlotImage.fillAmount = coolTime / 5;
           yield return new WaitForSeconds(1);
           coolTime++;
           item.coolTime = coolTime;
       }
       isCool = true;
       coolTime = 0;
       item.coolTime = coolTime;
       gundamPunch.transform.SetParent(item.player.transform);
       gundamPunch.transform.position = gundamPunch.transform.parent.position;
       gundamPunch.SetActive(false);
    }
}
public class TopemaBoots : ItemStrategy
{
    Player player;
    bool isCool = true;
    float coolTime = 0;
    public TopemaBoots(Item item) : base(item) 
    {
        player = GameManager.Instance.player1;
        item.explanation = "이름 : 토페마의 부츠" + System.Environment.NewLine + "점프력 2배 증가"
                      + System.Environment.NewLine + "사용 아이템"
                      + System.Environment.NewLine + "지속시간 : 2초, 쿨타임 : 50초";
    }
    public override void Use()
    {
        if (isCool)
        {
            item.gameObject.SetActive(true);
            item.gameObject.transform.position = new Vector2(1000, 1000);
            player.GetComponent<PlayerMove>().PlayerJump += 5f;
            item.StartCoroutine(JumpReset());
            item.owenrSlotImage.type = Image.Type.Filled;
            item.owenrSlotImage.fillAmount = 0;
        }
    }
    IEnumerator JumpReset()
    {
        isCool = false;
        Debug.Log("들어옴");
        yield return new WaitForSeconds(5f);
        player.GetComponent<PlayerMove>().PlayerJump -= 5f;
        while (coolTime <= 10)
        {
            item.owenrSlotImage.fillAmount = coolTime / 10;
            yield return new WaitForSeconds(1);
            coolTime++;
            item.coolTime = coolTime;
        }
        isCool = true;
    }
}

public class Potion : ItemStrategy
{
    public Potion(Item item) : base(item) 
    {

    }

    public override void Use()
    {
        item.player.Hp += 1;
    }
}
public enum ITEM_TYPE
{
    ACTIONBEAM,
    GUNDAMPUNCH,
    TOPEMABOOTS,
    POTION,
}
public class Item : MonoBehaviour, IAttackable
{
    public string explanation;
    public Sprite sprite;
    public Image owenrSlotImage;
    public float coolTime;
    public Rigidbody2D rb;
    public Player player;

    private float damage;
    [SerializeField] private ItemStrategy itemStrategy;
    [SerializeField] private ITEM_TYPE type;

    public float Damage 
    {
        get => damage; 
        set => damage = value;
    }
    public ITEM_TYPE Type
    {
        get => type;
        set => type = value;
    }
    private void Start()
    {
        player = GameManager.Instance.player1;
        rb = GetComponent<Rigidbody2D>();
        switch (type)
        {
            case ITEM_TYPE.ACTIONBEAM:
                itemStrategy = new ActionBeam(this);
                break;
            case ITEM_TYPE.GUNDAMPUNCH:
                itemStrategy = new GundamPunch(this);
                break;
            case ITEM_TYPE.TOPEMABOOTS:
                itemStrategy = new TopemaBoots(this);
                break;
            case ITEM_TYPE.POTION:
                itemStrategy = new Potion(this);
                break;
        }
    }
    public void Use()
    {
        itemStrategy.Use();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null)
            if (collision.TryGetComponent<IHitable>(out IHitable target))
                    Attack(target);
    }
    public void Attack(IHitable hitobj)
    {
        hitobj.Hit(Damage);
    }
}
