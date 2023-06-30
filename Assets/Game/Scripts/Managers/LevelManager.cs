using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HyperlabCase.Managers
{
    public static class LevelManager
    {
        private static int gameLevel => Database.Instance.DataSO.GameLevel;
        private static int maxLevel = 3;


        public static void LoadNextLevel()
        {
            Database.Instance.DataSO.GameLevel++;

            if (gameLevel > maxLevel)
            {
                Database.Instance.DataSO.GameLevel = 1;
            }

            LoadLevel();
        }

        public static void LoadLevel()
        {
            SceneManager.LoadSceneAsync("Level " + gameLevel);
        }
    }
}
