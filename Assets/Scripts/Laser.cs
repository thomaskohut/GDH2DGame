using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    //[SerializeField]
    //private int _divisor = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //The laser moves in a sin wave
        //Vector3 mt = new Vector3(Mathf.Sin(Time.fixedTime), _speed, 0);
        //transform.Translate(mt * Time.deltaTime);

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.y > 7.55)
        {
            Destroy(gameObject);
        }
    }
}
