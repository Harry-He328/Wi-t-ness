using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text text;
    public Slider slider;
    public bool isPlayerTurn;
    public static GameManager Instance;
    public float g = 9.8f;

    [Header("MovePoints")] 
    public int playerActionPoint;
    public int enemyActionPoint;
    public int currentPlayerActionPoint;
    public int currentEnemyActionPoint;
    
    

    void Start()
    {
        Instance = this;
        isPlayerTurn = true;
        InitializeActionPoint();
    }

    private void OnEnable()
    {
        SetMaxActionPoint();
    }


    void Update()
    {
        UpdateActionPoint();
    }
    

    public void SwitchTurn()
    {
        isPlayerTurn = !isPlayerTurn;
        
        if(isPlayerTurn) Debug.Log("Is Player Turn");
        else Debug.Log("Is Enemy Turn");
    }

    public void SetMaxActionPoint()
    {
        if (isPlayerTurn) slider.maxValue = playerActionPoint;
        else slider.maxValue = enemyActionPoint;
    }

    public void UpdateActionPoint()
    {
        if (isPlayerTurn)
        {
            text.text = currentPlayerActionPoint.ToString();
            slider.value = currentPlayerActionPoint;
        }
        else
        {
            text.text = currentEnemyActionPoint.ToString();
            slider.value = currentEnemyActionPoint;
        }
    }

    public void InitializeActionPoint()
    {
        currentPlayerActionPoint = playerActionPoint;
        currentEnemyActionPoint = enemyActionPoint;
    }
}

