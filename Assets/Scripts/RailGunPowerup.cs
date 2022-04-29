using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGunPowerup : MonoBehaviour
{
    Player plr;
    void Start()
    {
        plr = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        transform.Translate(plr.transform.position.x, plr.transform.position.y + 1, plr.transform.position.z);
    }
}