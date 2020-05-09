using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_GameManager : MonoBehaviour
{
    public GameObject gameMenu;
    public GameObject loseMenu;
    public GameObject winMenu;

    public Image soundBtnImg;
    public Sprite[] sondSprites;

    private static bool isSoundOn = true;
    public bool IsPause { get; private set; } = false;
    private GameManager gameManager { get => GameManager.Instance; }

    public static UI_GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CheckSound();

        gameMenu.SetActive(false);
        loseMenu.SetActive(false);
        winMenu.SetActive(false);
    }

    public void GameMenuBtn()
    {
        gameMenu.SetActive(true);
        IsPause = true;
        AudioManager.instance.Play("Click");
    }

    public void ContinuetBtn()
    {
        gameMenu.SetActive(false);
        IsPause = false;
        AudioManager.instance.Play("Click");
    }
    public void ToMainMenuBtn()
    {
        SceneManager.LoadScene(0);
        AudioManager.instance.Play("Click");
    }
    public void RestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioManager.instance.Play("Click");
    }
    public void SoundBtn()
    {
        isSoundOn = !isSoundOn;


        CheckSound();
        AudioManager.instance.Play("Click");
    }
    public void HintBtn() // Кнопка підказки
    {
        List<RectTransform> noFindDiffAreas = new List<RectTransform>();

        for (int i = 0; i < gameManager.diffAreas.Count; i++)
        {
            if (!gameManager.diffAreas[i].GetComponent<DifferencesArea>().IsPlayerFind)
                noFindDiffAreas.Add(gameManager.diffAreas[i]);
        }

        if (noFindDiffAreas.Count == 0)
            return;

        int rndIndex = Random.Range(0, noFindDiffAreas.Count);
        noFindDiffAreas[rndIndex].GetComponent<DifferencesArea>().PlayHintEffect();
        AudioManager.instance.Play("Click");
    }

    private void CheckSound()
    {
        switch (isSoundOn)
        {
            case true:
                AudioListener.volume = 1f;
                soundBtnImg.sprite = sondSprites[0];
                break;
            case false:
                AudioListener.volume = 0f;
                soundBtnImg.sprite = sondSprites[1];
                break;
        }
    }
}
