using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    private float _spd = 3f;
    [SerializeField]
    private int powerupID; //0 = Triple Shot, 1 = Speed, 2 = shield, 3 = railgun, 4 = ammo, 5 = health, 6 = neghealth, 7 = homing
    
    [SerializeField]
    private AudioClip _clip;
    private Player _plr;

    private void Start()
    {
        _plr = GameObject.Find("Player").GetComponent<Player>();

        if (_plr == null)
        {
            Debug.LogError("404 Player in Powerups");
        }
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.C) && _plr.gameObject.activeInHierarchy)
        {
            Vector3 targPos = _plr.transform.position;
            transform.position = (Vector3.MoveTowards(transform.position, _plr.transform.position, _spd * 2 * Time.deltaTime));
        }
        else
        {
            transform.Translate(Vector3.down * _spd * Time.deltaTime);
        }
        //transform.Translate(Vector3.down * _spd * Time.deltaTime);

        if (transform.position.y < -7.56f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ELaser")
        {
            Destroy(this.gameObject);
        }

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
                    case 5:
                        plr.AddHealth();
                        break;
                    case 6:
                        plr.Damage();
                        break;
                    case 7:
                        plr.HomingActive();
                        break;
                    case 8:
                        goto case 0;
                    case 9:
                        goto case 3;
                    case 10:
                        goto case 4;
                    case 11:
                        goto case 4;
                    case 12:
                        goto case 4;
                    case 13:
                        goto case 6;
                    case 14:
                        goto case 7;
                    default:
                        Debug.LogError("Poweup not collected.");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
