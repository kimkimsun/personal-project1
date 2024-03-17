using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorGround : MonoBehaviour
{
    public GameObject mirrorGround;

    void Update()
    {
        transform.position = new Vector2(mirrorGround.transform.position.x, mirrorGround.transform.position.y + 12.05f);
    }
}