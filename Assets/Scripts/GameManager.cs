using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;

    private void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("game"); //Game scene
        }

        if(_isGameOver && Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("mainmenu");
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }
}
