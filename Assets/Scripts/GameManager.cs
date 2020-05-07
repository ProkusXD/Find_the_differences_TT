using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("LevelSetting")]
    [Header("HealthBar")]
    [Range(0,10)]public int maxPlayerHealth = 3;

    public List<RectTransform> diffAreas;
    public RectTransform differcesAreaDiffImg;
    public RectTransform fallImg;

    public int NowPlayerHealth { get; set; }

    private int countAllCrntDiffArea = 0;
    private int countNowFoundDiffArea = 0;  

    public bool IsLose { get; private set; } = false;
    public bool IsWin { get; private set; } = false;
    public static GameManager Instance { get; private set; }

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
        if (IsLose || IsWin)
            return;

        if (Input.GetMouseButtonDown(0))
            ReycastCheck();
    }

    private void FindAllDiffAreas()
    {
        DifferencesArea[] allDiffAreas = FindObjectsOfType<DifferencesArea>();

        if (allDiffAreas.Length == 0)
        {
            Debug.LogError("Зони відмінностей не знайденні на сцені");
            return;
        }

        countAllCrntDiffArea = allDiffAreas.Length;
        StarBar.Instance.Spawn(countAllCrntDiffArea);

        for (int i = 0; i < allDiffAreas.Length; i++)
        {
            diffAreas.Add(allDiffAreas[i].GetComponent<RectTransform>());
        }

        DublicateDiffArea();
    }
    private void DublicateDiffArea()
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
    private void ReycastCheck()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

    private void SpawnFallIng(Vector2 pos)
    {
        fallImg.position = pos;
        fallImg.GetComponent<Animator>().SetTrigger("Start");
    }
    private void Correct(DifferencesArea difArea)
    {
        if (IsWin || IsLose)
            return;

        difArea.Correct();
        countNowFoundDiffArea++;
        StarBar.Instance.OpenStar(countNowFoundDiffArea);
        if (countNowFoundDiffArea == countAllCrntDiffArea)
            Win();
    }
    private void Fall()
    {
        if (IsLose || IsWin)
            return;
        HealthBar.Instance.TakeDamage(1);
        if (NowPlayerHealth <= 0)
            Lose();
    }
    private void Win()
    {
        IsWin = true;
        Debug.Log("Win!");
    }
    private void Lose()
    {
        IsLose = true;
        Debug.Log("Lose!");
    }
}
