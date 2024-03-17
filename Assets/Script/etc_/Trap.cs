using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

public abstract class TrapStrategy
{
    protected Trap owner;
    public TrapStrategy(Trap trap)
    {
        owner = trap;
    }
    public abstract void Active();
}

public class UpSwitchStrategy : TrapStrategy
{
    bool coroutining;
    public UpSwitchStrategy(Trap trap) : base(trap) 
    {
    }

    public override void Active()
    {
        coroutining = true;
        owner.StartCoroutine(ButtonOn());
    }
    IEnumerator ButtonOn()
    {
        while (coroutining)
        {
            Trap.switchOn = true;
            owner.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
            owner.ButtonCheck();
            owner.transform.position = new Vector2(owner.transform.position.x, owner.transform.position.y - 0.05f);
            yield return new WaitForSeconds(0.5f);
            owner.transform.position = new Vector2(owner.transform.position.x, owner.transform.position.y + 0.05f);
            Trap.switchOn = false;
            owner.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            coroutining = false;
        }
    }
}

public class DownSwitchStrategy : TrapStrategy
{
    bool coroutining;
    public DownSwitchStrategy(Trap trap) : base(trap)
    {
    }

    public override void Active()
    {
        owner.StartCoroutine(DownButtonOn());
        coroutining = true;
    }
    IEnumerator DownButtonOn()
    {
        if (coroutining)
        {
            Trap.switchOn2 = true;
            owner.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
            owner.ButtonCheck();
            owner.transform.position = new Vector2(owner.transform.position.x, owner.transform.position.y + 0.05f);
            yield return new WaitForSeconds(0.5f);
            owner.transform.position = new Vector2(owner.transform.position.x, owner.transform.position.y - 0.05f);
            Trap.switchOn2 = false;
            owner.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            coroutining = false;
        }
    }
}
public class PushDownStrategy : TrapStrategy
{
    public PushDownStrategy(Trap trap) : base(trap)
    {
    }

    public override void Active()
    {
        owner.transform.parent.transform.Translate(Vector2.down * 0.04f);
    }
}

public class PushUpStrategy : TrapStrategy
{
    public PushUpStrategy(Trap trap) : base(trap)
    {
    }

    public override void Active()
    {
        owner.transform.parent.transform.Translate(Vector2.up * 0.03f);
    }
}
public class TorchStrategy : TrapStrategy
{
    public TorchStrategy(Trap trap) : base(trap)
    {
    }

    public override void Active()
    {
        for (int i = 0; i < owner.torchList.Count; i++)
        {
            owner.torchList[i].SetActive(!owner.torchList[i].activeSelf);
        }
        if (owner.TorchPass())
            owner.CreatePipe();
    }
}
public class ResetSwitchStrategy : TrapStrategy
{
    public ResetSwitchStrategy(Trap trap) : base(trap)
    {
    }

    public override void Active()
    {
        for (int i = 0; i < owner.torchList.Count; i++)
        {
            owner.torchList[i].SetActive(false);
        }
    }
}
public enum TRAP_TYPE
{
    UPSWITCH,
    DOWNSWITCH,
    PUSHDOWN,
    PUSHUP,
    TORCH,
    RESETSWITCH,
    REVERSEGROUND,
    NONE,
}
public enum CRASH_DIR
{
    DOWN,
    LEFT,
    RIGHT,
    NONE,
}
public class Trap : MonoBehaviour, IAttackable
{
    [SerializeField] private TRAP_TYPE type;
    [SerializeField] private CRASH_DIR dir;

    public static bool switchOn;
    public static bool switchOn2;
    public bool ActiveCheck;
    public List<TrapStrategy> strategyList = null;
    public List<GameObject> torchList = new List<GameObject>();
    public GameObject destroyGameobject;
    public GameObject pipe;
    private float damage;
    private Vector2 crashDir;
    private Rigidbody2D rb;

    public float Damage 
    {
        get => damage;
        set => damage = value;
    }

    private void Start()
    {
        damage = 1;
        rb = GetComponent<Rigidbody2D>();
        strategyList = new List<TrapStrategy>();
        switch (type)
        {
            case TRAP_TYPE.UPSWITCH:
                strategyList.Add(new UpSwitchStrategy(this));
                break;
            case TRAP_TYPE.DOWNSWITCH:
                strategyList.Add(new DownSwitchStrategy(this));
                break;
            case TRAP_TYPE.PUSHDOWN:
                strategyList.Add(new PushDownStrategy(this));
                break;
            case TRAP_TYPE.PUSHUP:
                strategyList.Add(new PushUpStrategy(this));
                break;
            case TRAP_TYPE.TORCH:
                strategyList.Add(new TorchStrategy(this));
                break;
            case TRAP_TYPE.RESETSWITCH:
                strategyList.Add(new ResetSwitchStrategy(this));
                break;
            case TRAP_TYPE.NONE:
                break;
        }
        switch (dir)
        {
            case CRASH_DIR.RIGHT:
                crashDir = Vector2.right;
                break;
            case CRASH_DIR.LEFT:
                crashDir = Vector2.left;
                break;
            case CRASH_DIR.DOWN:
                crashDir = Vector2.down;
                break;
            case CRASH_DIR.NONE:
                break;
        }
    }
    public void Active()
    {
        foreach(TrapStrategy strategy in strategyList)
        {
            strategy.Active();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.transform.GetComponent<Player>();
        if (player != null && dir is CRASH_DIR.NONE)
            Active();
        if (player != null && dir is not CRASH_DIR.NONE)
            Attack(player.GetComponent<IHitable>());
    }
    private void Update()
    {
        if (type is TRAP_TYPE.NONE)
        {
            Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 1.6f), crashDir * 30, Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 1.6f), crashDir, 25, 1 << 8);
            if (hit.collider != null)
                rb.AddRelativeForce(crashDir * 30 * Time.deltaTime, ForceMode2D.Impulse);
            else
                rb.AddRelativeForce(crashDir * -30 * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
    public bool TorchPass()
    {
        foreach (GameObject torchCheck in torchList)
            ActiveCheck &= torchCheck.activeSelf;
        return ActiveCheck;
    }
    public void CreatePipe()
    {
        pipe.AddComponent<Rigidbody2D>();
        pipe.GetComponent<Rigidbody2D>().gravityScale = 10;
        for (int i = 0; i < torchList.Count; i++)
        {
            Destroy(torchList[i]);
        }
    }
    public void ButtonCheck()
    {
        if(switchOn && switchOn2)
            Destroy(destroyGameobject);
    }
    private void ResetPos()
    {
        rb.AddForce(-crashDir * 300 * Time.deltaTime, ForceMode2D.Impulse);
    }
    public void Attack(IHitable hitobj)
    {
        hitobj.Hit(damage);
    }
}
