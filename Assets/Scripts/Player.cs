using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 _inactiveThrust = new Vector3(0.5f, 0.2f, 0f);
    private Vector3 _activeThrust = new Vector3(1f, 1f, 0f);
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMult = 1.5f;
    [SerializeField]
    private float _offset = 1.0f;
    [SerializeField]
    private float _rof = 0.2f;
    private float _cd = 0.0f;
    [SerializeField]
    private float _sHealthDisp;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tShotPrefab;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject _expl;
    [SerializeField]
    private GameObject _railLasPrefab;
    private GameObject _raillas;
    [SerializeField]
    private GameObject _hominglas;
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private GameObject[] _engines;

    [SerializeField]
    private AudioClip _lasSound;
    private AudioSource _audioSrc;

    [SerializeField]
    private int _health = 3;
    [SerializeField]
    private int _shieldHealth;   
    private int _score = 0;

    private int _ammo = 15;
    private float _fuel = 600;
    private float _fuelmax = 600;

    private UIManager _ui;
    private SpawnManager _sm;

    private bool _tShotActive = false;
    private bool _speedBoostActive = false;
    private bool _shieldActive = false;
    private bool _railgunActive = false;
    private bool _homingActive = false;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _ui = GameObject.Find("Canvas").GetComponent<UIManager>();
        _sm = GameObject.Find("SpawnContainer").GetComponent<SpawnManager>();
        _audioSrc = GetComponent<AudioSource>();

        if(_ui == null)
        {
            Debug.LogError("404 UI Manager in Player");
        }

        if(_audioSrc == null)
        {
            Debug.LogError("404 Player Audio Source in Player");
        } else
        {
            _audioSrc.clip = _lasSound;
        }
    }

    void Update()
    {
        Movement();
        Wrap();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _cd && _ammo > 0)
        {
            Fire();
        }
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !_speedBoostActive && _fuel >= 1)
        {
            _speed = 6.25f;
            _fuel--;
            _ui.UpdateFuel(_fuel, _fuelmax);
            _thruster.transform.position = new Vector3(transform.position.x, transform.position.y -1.65f, transform.position.z);
            _thruster.transform.localScale = _activeThrust;
        }
        else if (_speedBoostActive) { } else
        {
            _speed = 5f;
            _thruster.transform.localScale = _inactiveThrust;
            _thruster.transform.position = new Vector3(transform.position.x, transform.position.y -0.95f, transform.position.z);                        
        }

        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.right * hInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * vInput * _speed * Time.deltaTime);

        //1 line version of below 2 lines
        //transform.Translate(new Vector3(hInput, vInput, 0)* _speed * Time.deltaTime);
    }

    void Wrap()
    {
        //To prevent objects from leaving screen (no wrap), use Mathf.Clamp()
        if (transform.position.x < -11.3f || transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-1 * transform.position.x, transform.position.y, transform.position.z);
        }

        if (transform.position.y < -7.56f || transform.position.y > 7.56f)
        {
            transform.position = new Vector3(transform.position.x, -1 * transform.position.y, transform.position.z);
        }
    }

    void Fire()
    {
        _cd = Time.time + _rof;
        _offset = 1f;
        if (_tShotActive)
        {   
            Instantiate(_tShotPrefab,
                new Vector3(transform.position.x, transform.position.y + _offset, transform.position.z), Quaternion.identity);
        } else if (_railgunActive)
        {
            _offset = 6.5f;
            _raillas = Instantiate(_railLasPrefab,
                new Vector3(transform.position.x, transform.position.y + _offset, transform.position.z), Quaternion.identity);
            StartCoroutine(RailgunPDown());
            StartCoroutine(RShot(_raillas));
        } else if (_homingActive)
        {
            StartCoroutine(HomingPDown());
            Instantiate(_hominglas,
                new Vector3(transform.position.x, transform.position.y + _offset, transform.position.z), Quaternion.identity);
        } else
        {
            Instantiate(_laserPrefab,
                new Vector3(transform.position.x, transform.position.y + _offset, transform.position.z), Quaternion.identity);
        }


        _ammo--;
        _ui.UpdateAmmo(_ammo);
        _audioSrc.Play();
    }

    public void Damage()
    {
        if (_shieldActive)
        {
            if (_shieldHealth >= 1)
            {
                _sHealthDisp *= 0.5f;
                _shieldHealth--;
                _shield.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, _sHealthDisp);
            }
            else
            {
                _shieldActive = false;
                _shield.SetActive(false);
            }
            return;
        }
      
        _health--;
        _ui.UpdateLives(_health);

        if (!_engines[0].activeInHierarchy && !_engines[1].activeInHierarchy)
        {
            _engines[Random.Range(0, 2)].SetActive(true);
        } else if (_engines[0].activeInHierarchy)
        {
            _engines[1].SetActive(true);
        } else if (_engines[1].activeInHierarchy)
        {
            _engines[0].SetActive(true);
        }

        if (_health <= 0)
        {
            _sm.OnDeath();
            Destroy(this.gameObject);
            Instantiate(_expl, transform.position, Quaternion.identity);          
        }
    }

    public void HomingActive()
    {
        if (!_homingActive)
        {
            _homingActive = true;
        }
    }

    IEnumerator HomingPDown()
    {
        while(_homingActive)
        {
            yield return new WaitForSeconds(5.0f);
            _homingActive = false;
        }
    }

    public void TShotActive()
    {
        if (!_tShotActive)
        {
            _tShotActive = true;
            StartCoroutine(TShotPowerDown());
        }
    }

    IEnumerator TShotPowerDown()
    {
        while (_tShotActive)
        {
            yield return new WaitForSeconds(5.0f);
            _tShotActive = false;
        }
    }

    public void SpeedBoostActive()
    {
        if (!_speedBoostActive)
        {
            _speedBoostActive = true;
            _speed *= _speedMult;
            StartCoroutine(SpeedBoostPDown());
        }
    }

    IEnumerator SpeedBoostPDown()
    {
        while (_speedBoostActive)
        {
            yield return new WaitForSeconds(5.0f);
            _speedBoostActive = false;
            _speed /= _speedMult;
        }
    }

    public void ShieldActive()
    {
        if (!_shieldActive)
        {
            _shieldHealth = 3;
            _sHealthDisp = 1f;
            _shield.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, _sHealthDisp);
            _shieldActive = true;
            _shield.SetActive(true);
        }
    }

    public void RailgunActive()
    {
        if(!_railgunActive)
        {
            _railgunActive = true;
            //StartCoroutine(RailgunPDown());
        }
    }

    IEnumerator RShot(GameObject x)
    {
        yield return new WaitForSeconds(0.05f);
            Destroy(x.gameObject);
    }

    IEnumerator RailgunPDown()
    {
        yield return new WaitForSeconds(5.0f);
        _railgunActive = false;
    }

    public void AddScore(int num)
    {
        _score += num;
        _ui.UpdateScore(_score);
    }

    public void AmmoResupply()
    {
        if (_ammo <= 5)
        {
            _ammo += 10;
        }
        else
        {
            _ammo = 15;
        }
        if(_ammo == 15 && _fuel <= 480)
        {
            _fuel += 120;
        } else
        {
            _fuel = 600;
        }

        _ui.UpdateAmmo(_ammo);
    }

    public void AddHealth()
    {
        if (_health < 3 && _health > 0)
        {
            _health++;        
        }
        _ui.UpdateLives(_health);
        if (_engines[0].activeInHierarchy)
        {
            _engines[0].SetActive(false);
        } else if(_engines[1].activeInHierarchy)
        {
            _engines[1].SetActive(false);
        }
    }
}