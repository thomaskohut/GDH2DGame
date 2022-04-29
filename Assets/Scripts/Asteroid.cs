using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private bool _split;
    [SerializeField]
    private float _rotspd;
    [SerializeField]
    private float _spd = 5.0f;
    private int _wavenum;
    [SerializeField]
    private GameObject _expl;
    private GameObject _p;

    [SerializeField]
    private GameObject _eContainer;

    private SpawnManager _sm;
    private Player _plr;
    
    // Start is called before the first frame update
    void Start()
    {
        _eContainer = GameObject.Find("SpawnContainer");
        _p = GameObject.Find("Player");
        _sm = GameObject.Find("SpawnContainer").GetComponent<SpawnManager>();
        _plr = _p.GetComponent<Player>();
        _eContainer = GameObject.Find("SpawnContainer");
        if (_sm == null)
        {
            Debug.LogError("404 SpawnManager in Asteroid");
        }
        if (_plr == null)
        {
            Debug.LogError("404 Player in Asteroid");
        }
        if (_p == null)
        {
            Debug.LogError("404 Player in Asteroid");
        }

        _wavenum = _sm.GetWave();

        if (tag == "Enemy")
        {
            transform.localScale = new Vector3(transform.localScale.x * .5f, transform.localScale.y * .5f, transform.localScale.z * .5f);
        }

        if (transform.localScale.z == .5)
        {
            _split = false;
        } else
        {
            _split = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tag == "Enemy")
        {
            transform.Translate(new Vector3(1f, -1f, 0) * _spd * Time.deltaTime);

            if (transform.position.y < -7.56f || transform.position.x >  10.74f)
            {
                Destroy(this.gameObject);
            }

            if (!_split && Vector3.Distance(transform.position, _p.gameObject.transform.position) < 5)
            {
                IsSplit();
            }
        } else
        {
            transform.Rotate(Vector3.forward * _rotspd * Time.deltaTime);
        }
    }

    private void IsSplit()
    {
        _split = true;
        GameObject ast2 = Instantiate(this.gameObject, transform.position, Quaternion.identity * Quaternion.Euler(0, 0, -45));
        ast2.transform.parent = _eContainer.transform;
        ast2.name = this.name;
        transform.rotation *= Quaternion.Euler(0, 0, 45);
        transform.localScale = new Vector3(transform.localScale.x * .5f, transform.localScale.y * .5f, transform.localScale.z * .5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" || other.tag == "HomingLaser" || other.tag == "RailLaser")
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GameObject explosion = Instantiate(_expl, transform.position, Quaternion.identity);
            explosion.transform.localScale = transform.localScale;

            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.8f);

            if (_wavenum <= 0)
            {
                _sm.StartSpawn();
                _wavenum = 1;
            }
            else
            {
                _plr.AddScore(20);
            }
        }

        if (other.tag == "Player")
        {
            if (_plr != null)
            {
                _plr.Damage();
            }
            GetComponent<SpriteRenderer>().enabled = false;
            GameObject explosion = Instantiate(_expl, transform.position, Quaternion.identity);
            explosion.transform.localScale = transform.localScale;

            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }
}
