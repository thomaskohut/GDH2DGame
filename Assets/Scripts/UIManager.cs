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
    private Sprite[] _lives;
    [SerializeField]
    private Image _livesimg;

    private GameManager _gm;

    void Start()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(_gm == null)
        {
            Debug.LogError("GameManager not found");
        }
    }

    public void UpdateScore(int pScore)
    {
        _scoreText.text = "Score: " + pScore;
    }

    public void UpdateLives(int currLives)
    {
        _livesimg.sprite = _lives[currLives];
        if (currLives <= 0)
        {
            GameOverSequence();
        }
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
