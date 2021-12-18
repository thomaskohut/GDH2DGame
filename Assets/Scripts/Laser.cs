using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    private bool _isELas = false;

    void Update()
    {
        if(!_isELas)
        {
            MoveUp();
        } else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);


        if (transform.position.y > 7.55)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -7.55)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _isELas == true)
        {
            Player plr = other.GetComponent<Player>();
            if(plr != null)
            {
                plr.Damage();
                Destroy(this.gameObject);
            } else
            {
                Debug.LogError("404 Player");
            }
        }
    }

    public void AssignELAs()
    {
        _isELas = true;
    }
}




