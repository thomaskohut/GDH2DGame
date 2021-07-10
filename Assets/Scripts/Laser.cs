using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    [SerializeField]
    private int _div = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 sinmt = new Vector3(Mathf.Sin(Time.fixedTime), _speed, 0);
        //transform.Translate(sinmt * Time.deltaTime);


        Vector3 circlemt = new Vector3(Mathf.Sin(Time.fixedTime), Mathf.Cos(Time.fixedTime), 0);
        transform.Translate(circlemt * (Time.deltaTime*_div));

        //transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.y > 7.55)
        {
            Destroy(gameObject);
        }
    }
}
