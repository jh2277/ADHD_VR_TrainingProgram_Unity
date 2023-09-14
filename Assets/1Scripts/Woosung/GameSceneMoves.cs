using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneMoves : MonoBehaviour
{

    public void GameSceneCtrl()
    {
        SceneManager.LoadScene("JH_W"); //어떤 씬 이름으로 이동할 건지
    }
    public void GameSceneCtrl_Choice()
    {
        SceneManager.LoadScene("Choice"); //어떤 씬 이름으로 이동할 건지
    }


}
