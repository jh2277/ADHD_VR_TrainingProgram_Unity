using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Char_Selected : MonoBehaviour
{
    public void GameSceneCtrlChoice()
    {
        SceneManager.LoadScene(1); //캐릭터 선택 씬으로 이동
    }
    public void GameSceneCtrl()
    {
        SceneManager.LoadScene(2); //Game Start
    }


}
