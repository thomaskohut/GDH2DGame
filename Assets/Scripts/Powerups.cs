using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    private float _spd = 3f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.down * _spd * Time.deltaTime);

        if (transform.position.y < -7.56f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player") {
            Player plr = other.transform.GetComponent<Player>();
            if(plr != null)
            {
                plr.TShotActive();
            } else
            {
                Debug.LogError("Player not found.");
            }

            Destroy(this.gameObject);
        }
    }
}
