using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class WeaponPosition
{
    public Transform transform;
    public bool isAvailable = true;
}

[Serializable]
public class EnemyData
{
    public float Health;
}

[Serializable]
public class GateData
{
    public float IncreaseValue;
    public GateIncrementalType IncrementalType;
}

[Serializable]
public class EndGameColumnData
{
    public float Health;
}

[Serializable]
public class StartGamePanelObjects
{
    public GameObject StartGamePanel;
    public Button ClearLevelButton;
    public Button IncomeButton;
    public TMP_Text ClearLevelLevelText;
    public TMP_Text ClearLevelCostText;
    public TMP_Text IncomeLevelText;
    public TMP_Text IncomeCostText;
}

[Serializable]
public class EndGamePanelObjects
{
    public GameObject EndGamePanel;
    public Button DamageButton;
    public Button FireRateButton;
    public TMP_Text DamageLevelText;
    public TMP_Text DamageCostText;
    public TMP_Text FireRateText;
    public TMP_Text FireRateCostText;
}