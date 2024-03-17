using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    private float radius;
    private float monsterDirection;
    private bool isPlayerCheck;
    private LayerMask playerLayer = 1 << 8;
    private Rigidbody2D monsterRb;
    [SerializeField] private MONSTER_TYPE type;
    public enum MONSTER_TYPE
    {
        HOONI,
        NAMIRI,
    }
    private void Start()
    {
        monsterRb = GetComponent<Rigidbody2D>();
        Invoke("LeftMosey", 0f);
        switch (type)
        {
            case MONSTER_TYPE.HOONI:
                radius = 6f;
                monsterDirection = 2;
                break;
            case MONSTER_TYPE.NAMIRI:
                radius = 10f;
                monsterDirection = 4;
                break;
        }
    }
  
    public Vector2 TargetPos
    {
        get;
        private set;
    }
    private void Update()
    {
        Collider2D hit;
        hit = Physics2D.OverlapCircle(transform.position, radius, playerLayer);
        
        if (hit != null)
        {
            CancelInvoke();
            isPlayerCheck = true;
            //TargetPos = hit.transform.position;
            //if(transform.position.x > TargetPos.x)
            //{
            //    transform.right = TargetPos * -1;
            //}
            //else if(transform.position.x < TargetPos.x)
            //{
            //    transform.right = TargetPos;
            //}
            transform.position = Vector2.MoveTowards(transform.position, hit.transform.position, 2 * Time.deltaTime);
        }
        else
        {
            if(!isPlayerCheck)
            {
                Invoke("LeftMosey", 0f);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void LeftMosey()
    {
        isPlayerCheck = true;
        monsterDirection *= -1;
        monsterRb.velocity = new Vector2(monsterDirection, monsterRb.velocity.y);
        Invoke("RightMosey", 3f);
    }
    private void RightMosey()
    {
        monsterRb.velocity = new Vector2(monsterDirection, monsterRb.velocity.y);
        Invoke("LeftMosey", 3f);
    }
}