using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void PlayBtn(string PlayScene)
    {
        SceneManager.LoadScene(PlayScene);
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}