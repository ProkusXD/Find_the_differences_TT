using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variobles
    [Header("LevelSetting")]
    [Range(0,10)]public int maxPlayerHealth = 3;
    [Header("UI")]
    public List<RectTransform> diffAreas;
    public RectTransform differcesAreaDiffImg;
    public RectTransform fallImg;

    public int NowPlayerHealth { get; set; }

    public int CountAllCrntDiffArea { get; private set; } = 0;
    public int CountNowFoundDiffArea { get; private set; } = 0;

    public bool IsLose { get; private set; } = false;
    public bool IsWin { get; private set; } = false;
    public static GameManager Instance { get; private set; }
    #endregion

    #region Aweke/Start/Update
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        FindAllDiffAreas();
    }
    private void Update()
    {
        if (IsLose || IsWin || UI_GameManager.Instance.IsPause)
            return;


        if (Input.GetMouseButtonDown(0))
            ReycastCheck();
    }
    #endregion

    #region Пошук і клонування зон відмінностей
    private void FindAllDiffAreas() // Пошук всіх встановлених зон відмінностей які є на сцені
    {
        DifferencesArea[] allDiffAreas = FindObjectsOfType<DifferencesArea>();

        if (allDiffAreas.Length == 0)
        {
            Debug.LogError("Зони відмінностей не знайденні на сцені");
            return;
        }

        CountAllCrntDiffArea = allDiffAreas.Length;
        StarBar.Instance.Spawn(CountAllCrntDiffArea);

        for (int i = 0; i < allDiffAreas.Length; i++)
        {
            diffAreas.Add(allDiffAreas[i].GetComponent<RectTransform>());
        }

        DublicateDiffArea();
    }
    private void DublicateDiffArea() // Клоновання зон відмінностей з оригінального забраження на інше
    {
        if (diffAreas != null)
        {
            for (int i = 0; i < diffAreas.Count; i++)
            {
                RectTransform dublicateArea = Instantiate(diffAreas[i],differcesAreaDiffImg);

                diffAreas[i].GetComponent<DifferencesArea>().CloneThisDiffArea = dublicateArea.gameObject;
                dublicateArea.GetComponent<DifferencesArea>().CloneThisDiffArea = diffAreas[i].gameObject;
            }
        }
    }
    #endregion

    #region Взаємодія гри з гравцем
    private void ReycastCheck() // Взаємодія з обєктами на зображеннях по яким клікнули 
    {
        Vector2 touchPos = new Vector2();
        #if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount == 1)
            touchPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        else return;
#else
        touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif

        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (hit.collider != null)
        {
            switch (hit.collider.tag)
            {
                case "DiffArea":
                    Correct(hit.collider.GetComponent<DifferencesArea>());
                    break;
                case "Image":
                    Fall();
                    SpawnFallIng( hit.point);
                    break;
            }
        }
    }

    private void SpawnFallIng(Vector2 pos) // Відобраєення зображення помилки при не натисканні не на зону відмінностей
    {
        fallImg.position = pos;
        fallImg.GetComponent<Animator>().SetTrigger("Start");
    }
    private void Correct(DifferencesArea difArea) //Дії при  попаданні на зонну вдмінностей
    {
        if (IsWin || IsLose || difArea.IsPlayerFind)
            return;


        difArea.Correct();
        CountNowFoundDiffArea++;
        StarBar.Instance.OpenStar(CountNowFoundDiffArea);

        AudioManager.instance.Play("Correct");
        Debug.Log("Correct");

        if (CountNowFoundDiffArea == CountAllCrntDiffArea)
            Win();
    }
    private void Fall() // Дії при не попаданні на зонну вдмінностей
    {
        if (IsLose || IsWin)
            return;

        HealthBar.Instance.TakeDamage(1);

        AudioManager.instance.Play("Error");
        Debug.Log("Fall");

        if (NowPlayerHealth <= 0)
            Lose();
    }
    #endregion

    #region Win/Lose
    private void Win()
    {
        IsWin = true;
        Debug.Log("Win!");
        UI_GameManager.Instance.winMenu.SetActive(true);
    }
    private void Lose()
    {
        IsLose = true;
        Debug.Log("Lose!");
        UI_GameManager.Instance.loseMenu.SetActive(true);
    }
    #endregion
}
