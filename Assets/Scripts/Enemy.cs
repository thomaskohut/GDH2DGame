using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _spd = 4.0f;
    private Player _plr;
    private Animator _anim;

    private AudioSource _audiosrc;

    // Start is called before the first frame update
    void Start()
    {
        _plr = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audiosrc = GetComponent<AudioSource>();

        if(_audiosrc == null)
        {
            Debug.LogError("404 Enemy Audio Source");
        }

        if (_plr == null)
        {
            Debug.LogError("404 Player");
        }

        if(_anim == null)
        {
            Debug.LogError("404 Animator");
        }
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

    void OnTriggerEnter2D(Collider2D other)
    {
        _anim.SetTrigger("OnEnemyDeath");
        _spd = 0f;
        GetComponent<Collider2D>().enabled = false;
        _audiosrc.Play();
        Destroy(this.gameObject, 2.8f);

        if (other.tag == "Player")
        {
            //Player plr = other.transform.GetComponent<Player>();
            if (_plr != null)
            {
                _plr.Damage();
            }        
        }

        if (other.tag == "Laser")
        {
            if (_plr != null)
            {
                _plr.AddScore();
            }
            Destroy(other.gameObject);
        }
    }
}
