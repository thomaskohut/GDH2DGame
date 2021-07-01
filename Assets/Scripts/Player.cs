using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Determines speed
    [SerializeField]
    private float _speed = 5.0f;
   

    // Start is called before the first frame update
    void Start()
    {
        //take current position = new position (0,0,0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");

        //1 line version of below 2 lines
        //transform.Translate(new Vector3(hInput, vInput, 0)* _speed * Time.deltaTime);

        transform.Translate(Vector3.right * hInput * _speed * Time.deltaTime);
        transform.Translate(Vector3.up * vInput * _speed * Time.deltaTime);

        //if playerpos on x is greater than 9 or less than -9, then it is set to its edge
        if(transform.position.x < -11.3f || transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-1*transform.position.x, transform.position.y, transform.position.z);
        }
        if (transform.position.y < -7.56f || transform.position.y > 7.56f)
        {
            transform.position = new Vector3(transform.position.x, -1 * transform.position.y, transform.position.z);
        }
        //To prevent objects from leaving screen (no wrap), use Mathf.Clamp()
    }
    }
