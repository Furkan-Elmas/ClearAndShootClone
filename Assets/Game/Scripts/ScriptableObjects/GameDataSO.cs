using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HyperlabCase.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Scriptable Objects/Game Data")]
    public class GameDataSO : ScriptableObject
    {
        public PlayerDataSO PlayerData;
        public int GameLevel;
        public float Currency;
    }
}
