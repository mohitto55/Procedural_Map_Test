using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSave
{
    public interface ISave
    {
        SaveData GetSaveData();
        void LoadFromSaveData(SaveData data);
    }
}
