using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSave;
using System.Xml.Serialization;
namespace GameSave
{
    public class UnitSaveManager : Singleton<UnitSaveManager>
    {
        List<SaveData> UnitDataList = new List<SaveData>();

        public void SaveData(Unit unit)
        {
            SaveData unitData = unit.GetSaveData();
            UnitDataList.Add(unitData);
            Debug.Log("저장할 데이터 : " + unitData.GetData("hp").GetIntSelf());
        }
        public void LoadData()
        {
            SaveData LoadUnitData = SaveManager.Instance.LoadData(1);
            if (LoadUnitData == null)
                return;
            Debug.Log("로딩유닛 데이터 있음");
            List<SaveData> LoadunitDataList = LoadUnitData.GetListSelf();
            if (LoadunitDataList != null)
            {
                Debug.Log("저장된 유닛 갯수 : " + LoadunitDataList.Count);
                foreach (SaveData unitData in LoadunitDataList)
                {
                    Debug.Log("유닛 HP : " + unitData.GetData("hp").GetIntSelf());
                }
            }
        }
        public List<SaveData> GetUnitList()
        {
            Debug.Log("리스트 카운트 : " + UnitDataList.Count);
            return UnitDataList;
        }
    }
}
