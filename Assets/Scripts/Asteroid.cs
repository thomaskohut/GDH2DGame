using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotspd;

    [SerializeField]
    private GameObject _expl;


    private SpawnManager _sm;
    // Start is called before the first frame update
    void Start()
    {
        _sm = GameObject.Find("SpawnContainer").GetComponent<SpawnManager>();
        
        if (_sm == null)
        {
            Debug.LogError("404: SpawnManager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotspd * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            GetComponent<SpriteRenderer>().enabled = false;
            Instantiate(_expl, transform.position, Quaternion.identity);
            
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.8f);

            _sm.StartSpawn();
        }
    }
}
