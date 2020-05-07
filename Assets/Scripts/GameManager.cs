using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<RectTransform> diffAreas;
    public RectTransform differcesAreaDiffImg;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        FindAllDiffAreas();
        DublicateDiffArea();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        ReycastCheck();
    }

    private void FindAllDiffAreas()
    {
        GameObject[] allDiffAreas = GameObject.FindGameObjectsWithTag("DiffArea");

        for (int i = 0; i < allDiffAreas.Length; i++)
        {
            diffAreas.Add(allDiffAreas[i].GetComponent<RectTransform>());
        }
    }
    private void DublicateDiffArea()
    {
        if (diffAreas != null)
        {
            for (int i = 0; i < diffAreas.Count; i++)
            {
                RectTransform dublicateArea = Instantiate(diffAreas[i]);
                dublicateArea.SetParent(differcesAreaDiffImg);
                dublicateArea.localPosition = diffAreas[i].localPosition;
                dublicateArea.localScale = diffAreas[i].localScale;
            }
        }
    }
    private void ReycastCheck()
    {
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("DiffArea"))
                Correct();
            else if (hit.collider.CompareTag("Image"))
                Fall();
        }
    }

    private void Fall()
    {
        print("Fall");
    }
    private void Correct()
    {
        print("Correct");
    }
}
