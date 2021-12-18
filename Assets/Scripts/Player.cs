using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Determines speed
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMult = 1.5f;
    [SerializeField]
    private float _offset = 1.0f;
    [SerializeField]
    private float _rof = 0.2f;
    private float _cd = 0.0f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tShotPrefab;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject[] _engines;

    [SerializeField]
    private AudioClip _lasSound;
    private AudioSource _audioSrc;

    [SerializeField]
    private int _health = 3;
    private int _score = 0;

    private UIManager _ui;

    [SerializeField]
    private bool _tShotActive = false;
    private bool _speedBoostActive = false;
    private bool _shieldActive = false;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _ui = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSrc = GetComponent<AudioSource>();

        if(_ui == null)
        {
            Debug.LogError("404 UI Manager");
        }

        if(_audioSrc == null)
        {
            Debug.LogError("404 Player Audio Source");
        } else
        {
            _audioSrc.clip = _lasSound;
        }
    }

    void Update()
    {
        Movement();
        Wrap();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _cd)
        {
            Fire();
        }
    }

    void Movement()
    {
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

        if (_tShotActive)
        {
            Instantiate(_tShotPrefab,
                new Vector3(transform.position.x, transform.position.y + _offset, transform.position.z), Quaternion.identity);
        } else
        {
            Instantiate(_laserPrefab,
                new Vector3(transform.position.x, transform.position.y + _offset, transform.position.z), Quaternion.identity);
        }

        _audioSrc.Play();
    }

    public void Damage()
    {
        if (_shieldActive)
        {
            _shieldActive = false;
            _shield.SetActive(false);
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
            Destroy(this.gameObject);
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
            _shieldActive = true;
            _shield.SetActive(true);
        }
    }

    public void AddScore()
    {
        _score += 10;
        _ui.UpdateScore(_score);
    }
}