using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;

    private bool _isELas = false;
    private bool _isRELas = false;

    private GameObject _targ;

    void Update()
    {
        if(!_isELas && tag != "HomingLaser")
        {
            MoveUp();
        } else if (tag == "HomingLaser")
        {
            StartCoroutine(HomingMove());
        } else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        
        if (transform.position.y > 7.55 && tag != "RailLaser")
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    IEnumerator HomingMove()
    {
        float spd = 5f;
        Vector3 targPos = new Vector3(0f, 8f, 0f); ;
        while ((transform.position.y < 7.55 || transform.position.y > -7.55))
        {

            if (_targ == null)
            {
                print("hello");
                _targ = FindTarg(gameObject);
                Vector3.MoveTowards(transform.position, targPos, spd * Time.deltaTime);
            }
            else if (targPos == transform.position)
            {
                print("b");
                Vector3.MoveTowards(transform.position, new Vector3(0, 8f, 0), spd * Time.deltaTime);
            } else
            {
                print("c");
                targPos = _targ.transform.position;
                transform.position = Vector3.MoveTowards(transform.position, targPos, spd * Time.deltaTime);
            }
            yield return new WaitForSeconds(.2f);
        }
        Destroy(gameObject);
    }

    private GameObject FindTarg(GameObject oldtarg)
    {
        float dist = Mathf.Infinity;
        Vector3 currpos = transform.position;

        GameObject retval = null;
        GameObject[] eList = GameObject.FindGameObjectsWithTag("Enemy");

        if (eList.Length <= 0)
        {
            print("B");
            return null;
        }

        foreach (GameObject go in eList)
        {
            Vector3 posdist = go.transform.position - currpos;
            float currdist = posdist.sqrMagnitude;

            if(currdist < dist && go != oldtarg) 
            {
                retval = go;
                dist = currdist;
            }
        }
        return retval;
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
        if (other.tag == "Player" && (_isELas|| _isRELas))
        {
            Player plr = other.GetComponent<Player>();
            if (plr != null)
            {
                plr.Damage();
                Destroy(this.gameObject);
            } else
            {
                Debug.LogError("404 Player");
            }
        }
        /**
        if (tag == "HomingLaser" && (other.tag != "Player" || other.tag == "Powerups"))
        {
            Destroy(this.gameObject);
        }
    */
    }

    public void AssignELAs()
    {
        _isELas = true;
    }

    public void AssignRELas()
    {
        _isRELas = true;
    }
}




