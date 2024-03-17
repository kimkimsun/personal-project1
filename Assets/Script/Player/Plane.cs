using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    bool isDrive;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Player1>() != null)
        {
            collision.transform.SetParent(transform, true);
            transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            transform.GetChild(0).gameObject.transform.position = new Vector2(95.38f, -195.76f);
            transform.GetChild(0).gameObject.GetComponent<PlayerMove>().IsMove = false;
            transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = false;
            isDrive = true;
        }
    }

    private void Update()
    {
        if(isDrive)
        {
            if(Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector2.up * 8f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector2.left * 8f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector2.down * 8f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector2.right * 8f * Time.deltaTime);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TEST");
        if (collision.gameObject.tag == "EventEndZone")
        {
            Debug.Log("TEST22");
            isDrive = false;
            transform.GetChild(0).gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            transform.GetChild(0).gameObject.GetComponent<PlayerMove>().IsMove = true;
            transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().enabled = true;
            transform.GetChild(0).gameObject.transform.SetParent(null);
            Destroy(gameObject);
        }
    }
}
