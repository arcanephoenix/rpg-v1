using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool lookAtMe = false;
    private GameObject player;

    public string playerName;
    public string calloutString;

    private void Start()
    {
        player = GameObject.Find("PlayerFunne");
    }

    private void FixedUpdate()
    {
        if(lookAtMe)
        {
            transform.LookAt(player.transform);
        }
    }
}
