using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int actionPoint;
    public Text text;
    public Slider slider;

    public static GameManager Instance;

    void Start()
    {
        Instance = this;
        slider.maxValue = actionPoint;
    }

    
    void Update()
    {
        text.text = actionPoint.ToString();

        slider.value = actionPoint;
    }

    public bool GetActionPoint()
    {
        return actionPoint > 0;
    }
    
    public void SetActionPoint()
    {
        actionPoint--;
    }
    
}

