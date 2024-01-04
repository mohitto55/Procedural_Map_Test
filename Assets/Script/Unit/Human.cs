using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSave;
public class Human : Unit
{
    protected  void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        float h = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Fast fastbuff = new Fast();
            AddBuff(fastbuff);
            Debug.Log("버프 추가 " + BuffList.Count + " 버프 스택 : " + GetBuff("Fast").Stack);
            Debug.Log("속도 : " + Speed);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Fast fastbuff = new Fast();
            RemoveBuff("Fast");
            Debug.Log("버프 삭제 " + BuffList.Count);
            Debug.Log("속도 : " + Speed);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveData save = new SaveData();
            GameSaveManager.GameSave();
        }
        Rigid.velocity = new Vector2(h * Speed, v * Speed);
    }
}
