using System.Collections;
using System.Collections.Generic;
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
    private bool _spActive = false;

    public void StartSpawn()
    {
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnPowerUp());
    }

    public void OnDeath()
    {
        _spActive = true;
    }

    //Spawns enemy every 5 seconds
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(3.5f);
        while (!_spActive)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-9f, 9f), 7.56f, 0);
            GameObject newEnemy = Instantiate(_enemyprefab, spawnPos, Quaternion.identity);
            newEnemy.transform.parent = _eContainer.transform;
            yield return new WaitForSeconds(3.0f);
        }
    }

    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(3.5f);
        while (!_spActive)
        {           
            Vector3 spawnPos = new Vector3(Random.Range(-9f, 9f), 7.56f, 0);
            GameObject newPUp = Instantiate(_Powerup[Random.Range(6,6)], spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(4,9));
        }
    }
}


