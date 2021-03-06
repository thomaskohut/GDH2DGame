using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameoverTxt;
    [SerializeField]
    private Text _continueTxt;
    [SerializeField]
    private Text _ammoTxt;
    [SerializeField]
    private Text _fuelTxt;
    [SerializeField]
    private Sprite[] _lives;
    [SerializeField]
    private Image _livesimg;
    [SerializeField]
    private Text _wavetxt;


    private GameManager _gm;

    void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(_gm == null)
        {
            Debug.LogError("404 GameManager in UIManager");
        }
    }

    public void UpdateAmmo(int pAmmo)
    {
        if (pAmmo >= 0)
        {
            _ammoTxt.text = "Ammo: " + pAmmo + "/15";
        }
    }

    public void UpdateScore(int pScore)
    {
        _scoreText.text = "Score: " + pScore;
    }

    public void UpdateWave(int wavenum)
    {
        _wavetxt.text = "Wave: " + wavenum;
    }

    public void UpdateLives(int currLives)
    {
        
        if (currLives <= 0)
        {
            _livesimg.sprite = _lives[0];
            GameOverSequence();
        } else
        {
            _livesimg.sprite = _lives[currLives];
        }
    }

    public void UpdateFuel(float fuel, float fuelMax)
    {
        float psum = (fuel / fuelMax) * 100;
        _fuelTxt.text = "Fuel: " + (int) psum + "%";

    }

    private void GameOverSequence()
    {
        _gm.GameOver();
        _continueTxt.gameObject.SetActive(true);
        StartCoroutine(flickerRoutine());
    }

    IEnumerator flickerRoutine()
    {
        while(true)
        {
            _gameoverTxt.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameoverTxt.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
