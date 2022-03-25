using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _eContainer;
    [SerializeField]
    private GameObject _enemyprefab;
    [SerializeField]
    private GameObject[] _Powerup;

    [SerializeField]
    private bool _spActive = true;

    //Start wave enemy waves wave = 1 enemy type, wave 2 = 2 enemy types, wave 3 = boss.  do it by score/enemy count
    [SerializeField]
    private int _wave;
    [SerializeField]
    private int _eleft;

    private UIManager _ui; 

    private void Start()
    {
        _ui = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_ui == null)
        {
            Debug.LogError("404 UI Manager in Player");
        }
    }

    public void StartSpawn()
    {
        _wave = 1;
        _eleft = 5;
        _spActive = true;
        _ui.UpdateWave(_wave);
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }

    public void OnDeath()
    {
        _spActive = false;
    }

    private void NewWave(int wavenum)
    {
        _wave = wavenum;
        _eleft = 5 * wavenum;
        _ui.UpdateWave(_wave);
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }

    //Spawns enemy every 5 seconds
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(3.5f);
        while (_spActive && _eleft >= 0)
        {
            if (_eleft <= 0)
            {
                StopCoroutine(SpawnEnemy());
                StopCoroutine(SpawnPowerUp());
                NewWave(_wave + 1);
            }
            else
            {
                Vector3 spawnPos = new Vector3(Random.Range(-9f, 9f), 7.56f, 0);
                GameObject newEnemy = Instantiate(_enemyprefab, spawnPos, Quaternion.identity);
                newEnemy.transform.parent = _eContainer.transform;
                _eleft--;
                yield return new WaitForSeconds(3.0f);
            }
        }
    }

    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(3.5f);
        while (_spActive && _eleft > 0)
        {           
            Vector3 spawnPos = new Vector3(Random.Range(-9f, 9f), 7.56f, 0);
            GameObject newPUp = Instantiate(_Powerup[Random.Range(0,20)], spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(4,9));
        }
    }
}


