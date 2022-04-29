using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _spd = 4.0f;
    private float _fireRate = 3;
    private float _canFire = -1;
    private float _eshieldactivate;
    private int _eShieldHealth;
    private bool _eShieldActive = false;
    private bool _isAlive = true;

    [SerializeField]
    private GameObject eLasPrefab;
    [SerializeField]
    private GameObject _eshield;
    private AudioSource _audiosrc;
    [SerializeField]
    GameObject _expl;

    private Player _plr;
    private Animator _anim;

    void Start()
    {
        _plr = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audiosrc = GetComponent<AudioSource>();

        if (_audiosrc == null)
        {
            Debug.LogError("404 Enemy Audio Source in Enemy");
        }

        if (_plr == null)
        {
            Debug.LogError("404 Player in Enemy");
        }

        if (_anim == null)
        {
            Debug.LogError("404 Animator in Enemy");
        }

        _eshieldactivate = Random.Range(0f, 1f);
        if (_eshieldactivate <= .4f)
        {
            EShield();
        }
    }

    void Update()
    {
        EMovement();
        EFire();
    }

    void EFire()
    {
        if (Time.time > _canFire && _isAlive)
        {

            _fireRate = Random.Range(3.0f, 7.0f);
            _canFire = Time.time + _fireRate;
            GameObject _eLas = Instantiate(eLasPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = _eLas.GetComponentsInChildren<Laser>();
            for (int x = 0; x < lasers.Length; x++)
            {
                if (_plr.transform.position.y > transform.position.y && _plr != null)
                {
                    lasers[x].AssignRELas();
                }
                else
                {
                    lasers[x].AssignELAs();
                }
            }
        }
    }

    void EMovement()
    {
        transform.Translate(Vector3.down * _spd * Time.deltaTime);

        if (transform.position.y < -7.56f)
        {
            Destroy(this.gameObject);
        }
    }

    void EShield()
    {
        if (!_eShieldActive)
        {
            _eShieldHealth = 1;
            _eshield.SetActive(true);
            _eShieldActive = true;
        }
    }

    void EDamage(int damagetype)
    {
        if (_eShieldActive && damagetype >= 1)
        {
            if (_eShieldHealth > 1)
            {
                _eShieldHealth--;
            }
            else
            {
                _eshield.SetActive(false);
                _eShieldActive = false;
            }
            return;
        }
        if (damagetype == 0)
        {
            _eshield.SetActive(false);
            _eShieldActive = false;
        }
        _isAlive = false;
        //_anim.SetTrigger("OnEnemyDeath");
        _spd = 0f;
        GetComponent<Collider2D>().enabled = false;
        _audiosrc.Play();
        GameObject explosion = Instantiate(_expl, transform.position, Quaternion.identity);
        explosion.transform.localScale = transform.localScale;
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ELaser" || other.tag == "Powerups" || other.tag == "EShield") {}

        if (other.tag == "Player")
        {
            if (_plr != null)
            {
                _plr.Damage();
                EDamage(1);
            }
        }

        if (other.tag == "Laser" || other.tag == "HomingLaser")
        {
            if (_plr != null && !_eShieldActive)
            {
                _plr.AddScore(10);
            }           
            Destroy(other.gameObject);
            EDamage(1);
        }

        if (other.tag == "RailLaser")
        {
            if (_plr != null)
            {
                _plr.AddScore(10);
            }
            EDamage(0);
        }
    }
}
