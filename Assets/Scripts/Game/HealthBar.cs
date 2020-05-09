using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private GameManager gameManager { get => GameManager.Instance; }
    private int healthMax;
    private int healthMin = 0;
    private int health;

    private Slider slider;

    public static HealthBar Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        healthMax = gameManager.maxPlayerHealth;
        health = healthMax;
        gameManager.NowPlayerHealth = health;

        slider = GetComponent<Slider>();
        slider.maxValue = healthMax;
        slider.value = health;
        slider.minValue = healthMin;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, healthMin, healthMax);
        slider.value = health;
        gameManager.NowPlayerHealth = health;
    }
}
