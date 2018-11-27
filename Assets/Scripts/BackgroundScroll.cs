using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    Transform player;
    MeshRenderer mr;
    Vector2 playerOffset;

    void Start()
    {
        player = GameObject.Find("Player Ship").GetComponent<Transform>();
    }
    void Update ()
    {
        if(player)
        {
            Scroll();
        }

    }

    // Scroll background based on player movement
    void Scroll()
    {
        mr = GetComponent<MeshRenderer>();
        Material mat = mr.material;
        Vector2 offset = mat.mainTextureOffset;

        //offset.y = player.transform.position.y / transform.localScale.y;
        offset.x = player.transform.position.x / transform.localScale.x;
        playerOffset = offset;
        mat.mainTextureOffset = offset;

    }
}
