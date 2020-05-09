using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarBar : MonoBehaviour
{
    [SerializeField] private Sprite starOpenImg;
    [SerializeField] private Sprite starClosedImg;

    [SerializeField] private GameObject starPrefab;

    private List<Image> starImages;


    public static StarBar Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Spawn(int countDiffArea)
    {
        starImages = new List<Image>();

        for (int i = 0; i < countDiffArea; i++)
        {
            GameObject star = Instantiate(starPrefab,transform);
            starImages.Add(star.GetComponent<Image>());
            starImages[i].sprite = starClosedImg;
        }
    }

    public void OpenStar(int index)
    {
        starImages[index-1].sprite = starOpenImg;
    }
}
