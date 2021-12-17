using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    private float _spd = 3f;
    [SerializeField]
    private int powerupID; //0 = Triple Shot, 1 = Speed, 2 = shield
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
                    default:
                        Debug.LogError("Poweup not collected.");
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
