using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _spd = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _spd * Time.deltaTime);
        Wrap();
    }

    void Wrap()
    {
        if(transform.position.y < -7.56)
        {
            transform.position = new Vector3(Random.Range(-9.3f,9.3f), 7.56f, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //if collides with player, destroys us and damages player
        //if collides with laser, destroy us.

        Destroy(this.gameObject);

        if (other.tag == "Player")
        {
            Player plr = other.transform.GetComponent<Player>();
            if(plr != null)
            {
                plr.Damage();
            }        
        }

        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
        }
    }
}
