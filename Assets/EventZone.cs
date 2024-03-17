using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventZone : MonoBehaviour
{
    private float X = 0.7f;
    private float Y = 0.75f;
    private int changeLayer;
    private bool stopCoroutine = true;
    private Vector2 xx;
    private Vector2 absorbDir;
    private Vector2 summoned;
    private PlayerMove.SIZE_TYPE pmType;
    [SerializeField] private ZONE_TYPE type;
    [SerializeField] private List<GameObject> bossWall = new List<GameObject>();
    public enum ZONE_TYPE
    {
        BIG,
        SMALL,
        BOSSSTART,
    }
    private void Start()
    {
        switch (type)
        {
            case ZONE_TYPE.BIG:
                xx = Vector2.one;
                absorbDir = Vector2.up;
                summoned = new Vector2(96.05f, -177.91f);
                pmType = PlayerMove.SIZE_TYPE.BIG;
                changeLayer = 14;
                break;
            case ZONE_TYPE.SMALL:
                absorbDir = Vector2.down;
                xx = new Vector2(0.3f, 0.25f);
                summoned = new Vector2(-65.32f, -178.49f);
                pmType = PlayerMove.SIZE_TYPE.DEFAULT;
                changeLayer = 8;
                X = X * -1;
                Y = Y * -1;
                break;
            case ZONE_TYPE.BOSSSTART:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player2>() != null)
        {
            StartCoroutine(EventEnter(collision.gameObject));
            collision.GetComponent<PlayerMove>().type = PlayerMove.SIZE_TYPE.BIG;
            collision.gameObject.layer = changeLayer;
        }
        else if (collision.GetComponent<Player>() != null && ZONE_TYPE.BOSSSTART == type)
            StartCoroutine(CreateWall());
    }

    IEnumerator EventEnter(GameObject player)
    {
        Debug.Log(transform.localScale.x);
        Debug.Log(X);
        if (stopCoroutine)
        for(int i =0; i < 100; i++)
        {
            player.transform.Translate(absorbDir * 0.05f);
        }
        yield return new WaitForSeconds(1);
        player.transform.localScale = xx;
        player.transform.position = summoned;
        stopCoroutine = false;
    }
    IEnumerator CreateWall()
    {
        for (int i = 0; i < bossWall.Count; i++)
        {
            bossWall[i].SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
