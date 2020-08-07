using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHealth;
    public Sprite emptyHealth;
    public IntValue heartContainers;
    public IntValue playerCurrentHealth;

    // Start is called before the first frame update
    void Start()
    {
        InitHearts();
    }

    public void InitHearts()
    {
        for (int i = 0; i < heartContainers.initialValue; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHealth;
        }
    }

    public void UpdateHearts()
    {
        int tempHealth = playerCurrentHealth.RuntimeValue;
        for (int i = 0; i < heartContainers.initialValue; i++)
        {
            if (i <= tempHealth - 1)
            {
                hearts[i].sprite = fullHealth;
            }
            else if (i >= tempHealth)
            {
                hearts[i].sprite = emptyHealth;
            }
        }
    }
}
