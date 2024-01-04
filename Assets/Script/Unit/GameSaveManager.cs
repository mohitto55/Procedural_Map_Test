using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSave;
namespace GameSave {
    public static class GameSaveManager
    {
        public static void GameSave()
        {
            EventManager<GameEvents>.Instance.PostEvent(GameEvents.Save, null, null);
            List<SaveData> list = UnitSaveManager.Instance.GetUnitList();
            SaveData unitdata = new SaveData(list);
            SaveManager.Instance.SaveData(1, unitdata);
            Debug.Log("Game Save...");
        }
        public static void GameLoad()
        {
            UnitSaveManager.Instance.LoadData();
            Debug.Log("Game Loading...");
        }
    }
}
