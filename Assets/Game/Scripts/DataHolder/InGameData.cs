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
}

[Serializable]
public class EndGamePanelObjects
{
    public GameObject EndGamePanel;
}