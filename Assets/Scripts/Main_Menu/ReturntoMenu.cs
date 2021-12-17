using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturntoMenu : MonoBehaviour
{
    public void LoadMenu()
    {
        SceneManager.LoadScene(1);
    }
}
