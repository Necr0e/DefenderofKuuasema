using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    public Transform Player;

    void LateUpdate()
    {
        //Follow the player on the x-axis alone, stop if player is destroyed.
        if(Player)
        {
            transform.position = new Vector3(Player.transform.position.x,0,-15);
        }
        else
        {
            return;
        }


    }
}
