using HyperlabCase.Managers;
using HyperlabCase.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace HyperlabCase
{
    public class Database : Singleton<Database>
    {
        [SerializeField] private bool editorMode = false;
        [SerializeField] private GameDataSO data;

        public GameDataSO DataSO { get => data; }

        protected override void Awake()
        {
            base.Awake();
            data = LoadData();
        }

        private void Start()
        {
            LevelManager.LoadLevel();
        }

        private GameDataSO LoadData()
        {
            GameDataSO data;
            if (!editorMode)
            {
                if (File.Exists(Application.persistentDataPath + Path.DirectorySeparatorChar + "Database.txt"))
                {
                    data = ScriptableObject.CreateInstance<GameDataSO>();
                    var json = File.ReadAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "Database.txt");
                    JsonUtility.FromJsonOverwrite(json, data);
                }
                else
                {
                    data = Resources.Load<GameDataSO>("Data");
                    var json = JsonUtility.ToJson(data, true);
                    File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "Database.txt", json);
                }
            }
            else
            {
                data = Resources.Load<GameDataSO>("Data");
            }

            return data;
        }

        public void SaveData()
        {
            var json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + Path.DirectorySeparatorChar + "Database.txt", json);
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }
    }
}
