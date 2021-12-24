using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    private float _spd = 3f;
    [SerializeField]
    private int powerupID; //0 = Triple Shot, 1 = Speed, 2 = shield, 3 = railgun

    [SerializeField]
    private AudioClip _clip;

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

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if(plr != null)
            {
                switch(powerupID)
                {
                    case 0:
                        plr.TShotActive();
                        break;
                    case 1:
                        plr.SpeedBoostActive();
                        break;
                    case 2:
                        plr.ShieldActive();
                        break;
                    case 3:
                        plr.RailgunActive();
                        break;
                    case 4:
                        plr.AmmoResupply();
                        break;
                    default:
                        Debug.LogError("Poweup not collected.");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
