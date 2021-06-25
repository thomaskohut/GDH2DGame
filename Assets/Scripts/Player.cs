using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Determines speed
    [SerializeField]
    private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        //take current position = new position (0,0,0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

        //transform.Translate(Vector3.up * speed * Time.deltaTime);
        //transform.Translate(Vector3.down * speed * Time.deltaTime);
        //transform.Translate(Vector3.left * (speed * 1.5f) * Time.deltaTime);

        transform.Translate(Vector3.right * speed/2 * Time.deltaTime);
    }
}
