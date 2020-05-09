using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenuManager : MonoBehaviour
{
    public void LevelSelectBtn(int levelIndex)
    {
        AudioManager.instance.Play("Click");
        SceneManager.LoadScene(levelIndex);
    }
}
