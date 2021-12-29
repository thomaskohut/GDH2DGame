using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _spd = 4.0f;
    private float _fireRate = 3;
    private float _canFire = -1;
    private bool _isAlive = true;

    [SerializeField]
    private GameObject eLasPrefab;

    private AudioSource _audiosrc;

    private Player _plr;
    private Animator _anim;

    void Start()
    {
        _plr = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audiosrc = GetComponent<AudioSource>();

        if(_audiosrc == null)
        {
            Debug.LogError("404 Enemy Audio Source in Enemy");
        }

        if (_plr == null)
        {
            Debug.LogError("404 Player in Enemy");
        }

        if(_anim == null)
        {
            Debug.LogError("404 Animator in Enemy");
        }
    }

    // Update is called once per frame
    void Update()
    {
        EMovement();

        if (Time.time > _canFire && _isAlive)
        {
            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject eLas = Instantiate(eLasPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = eLas.GetComponentsInChildren<Laser>();
            for(int x = 0; x < lasers.Length; x++)
            {
                lasers[x].AssignELAs();
            }
        }
    }

    void EMovement()
    {
        transform.Translate(Vector3.down * _spd * Time.deltaTime);
        //Wrap();
        if (transform.position.y < -7.56f)
        {
            Destroy(this.gameObject);
        }
    }

    //void Wrap()
    //{
    //    if(transform.position.y < -7.56)
    //    {
    //        transform.position = new Vector3(Random.Range(-9.3f,9.3f), 7.56f, 0f);
    //    }
    //}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ELaser" || other.tag == "Powerups") {} else {
            _isAlive = false;
            _anim.SetTrigger("OnEnemyDeath");
            _spd = 0f;
            GetComponent<Collider2D>().enabled = false;
            _audiosrc.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }

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
