using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSave;
public class TestManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        EventManager<GameEvents>.Instance.AddListener(GameEvents.Load, LoadGame);
        EventManager<GameEvents>.Instance.PostEvent(GameEvents.Load, null, this);
        EventManager<GameEvents>.Instance.AddListener(GameEvents.CallSave, SaveGame);
    }
    void SaveGame(GameEvents eventType, Component sender, object param)
    {
        GameSaveManager.GameSave();
    }
    void LoadGame(GameEvents eventType, Component sender, object param)
    {
        GameSaveManager.GameLoad();
    }
    private void OnApplicationQuit()
    {
        EventManager<GameEvents>.Instance.PostEvent(GameEvents.CallSave, null, this);
    }
}
