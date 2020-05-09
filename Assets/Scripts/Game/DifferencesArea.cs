using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifferencesArea : MonoBehaviour
{
    public ParticleSystem hintEffect;
    public bool IsPlayerFind { get; set; } = false;

    private GameManager gameManager { get => GameManager.Instance; }

    public GameObject CloneThisDiffArea { get; set; }


    private void Awake()
    {
        GetComponent<Image>().enabled = false;
    }
    public void Correct()
    {
        if (!IsPlayerFind)
        {
            GetComponent<Image>().enabled = true;
            CloneThisDiffArea.GetComponent<Image>().enabled = true;

            IsPlayerFind = true;
            CloneThisDiffArea.GetComponent<DifferencesArea>().IsPlayerFind = true;
        }
    }
    public void PlayHintEffect()
    {
        hintEffect.Play();
        CloneThisDiffArea.GetComponent<DifferencesArea>().hintEffect.Play();
    }
}
