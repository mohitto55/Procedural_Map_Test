using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameSave;
[Serializable]
public class UnitBuff
{
    public Buff buff;
    public float updateCycle;
    public float Timer;

    public UnitBuff(Buff buff)
    {
        this.buff = buff;
        this.updateCycle = buff.UpdateCycle;
        Timer = buff.UpdateCycle;
    }
}
[RequireComponent(typeof(Rigidbody2D))]
public class Unit : MonoBehaviour , ISave
{
    [SerializeField]
    int id;
    [SerializeField]
    int maxHp;
    [SerializeField]
    int mp;
    [SerializeField]
    int hp;
    [SerializeField]
    int atk;
    [SerializeField]
    int speed;
    public int MaxHP { get { return maxHp; } set { maxHp = value; } }
    public int MP { get { return mp; } set { mp = value; } }
    public int ATK { get { return atk; } set { atk = value; } }
    public int Speed
    {
        get
        {
            if (SpeedBuffs != null)
            {
                return speed + SpeedBuffs(this);
            }
            else return speed;
        }
        set { speed = value; }
    }

    public delegate int SpeedBuff(Unit unit);
    public event SpeedBuff SpeedBuffs;
    [SerializeField]
    protected List<UnitBuff> BuffList = new List<UnitBuff>();
    IEnumerator C_BuffTimer;
    protected Rigidbody2D Rigid;
    Coroutine C_MoveToPos;
    protected virtual void Start()
    {
        Rigid = GetComponent<Rigidbody2D>();
        EventManager<GameEvents>.Instance.AddListener(GameEvents.Save, GameSave);
    }
    public void AddBuff(Buff buff)
    {
        if (C_BuffTimer == null)
        {
            C_BuffTimer = BuffTimer();
            StartCoroutine(C_BuffTimer);
        }
        if(BuffList.Count == 0)
        {
            StartCoroutine(C_BuffTimer);
        }
        for (int i = 0; i < BuffList.Count; i++)
        {
            if (BuffList[i].buff.ID == buff.ID)
            {
                BuffList[i].buff.BuffOverlay(this);
                return;
            }
        }
        UnitBuff unitBuff = new UnitBuff(buff);
        BuffList.Add(unitBuff);
        unitBuff.buff.BuffStart(this);

    }
    public Buff GetBuff(string buffID)
    {
        for (int i = 0; i < BuffList.Count; i++)
        {
            if (BuffList[i].buff.ID == buffID)
            {
                return BuffList[i].buff;
            }
        }
        return null;
    }
    public void RemoveBuff(string ID)
    {

        for (int i = 0; i < BuffList.Count; i++)
        {
            if (BuffList[i].buff.ID == ID)
            {
                BuffList[i].buff.BuffEnd(this);
                BuffList.RemoveAt(i);
                break;
            }
        }

        if (BuffList.Count > 0)
            StopCoroutine(C_BuffTimer);
    }
    IEnumerator BuffTimer()
    {
        if(BuffList.Count > 0)
        {
            for(int i = 0; i < BuffList.Count; i++)
            {
                if (!BuffList[i].buff.Endless)
                {
                    BuffList[i].Timer -= Time.deltaTime;
                    if(BuffList[i].Timer >= 0)
                    {
                        BuffList[i].buff.BuffUpdate(this);
                        BuffList[i].Timer = BuffList[i].updateCycle;
                    }
                }
            }
        }
        yield return null;
    }
    public void MoveUnit(Vector2 dir)
    {
        Vector2 Movedir = (dir * Speed * Time.deltaTime);
        Rigid.MovePosition(Rigid.position + Movedir);
    }
    IEnumerator CMoveToPos(Vector2 pos)
    {
        Debug.Log("무빙 시작");
        while (Vector2.Distance((Vector2)transform.position, pos) > 0.5f)
        {
            Debug.Log("이동할 위치 : " + pos);
            Vector2 dir = pos - Rigid.position;
            MoveUnit(dir.normalized);
            yield return null;
        }
    }
    public void MoveToPos(Vector2 pos)
    {
        if(C_MoveToPos != null)
        StopCoroutine(C_MoveToPos);
        C_MoveToPos = StartCoroutine(CMoveToPos(pos));
    }
    public void GameSave(GameEvents eventType ,Component component, object param)
    {
        UnitSaveManager.Instance.SaveData(this);
    }
    public virtual SaveData GetSaveData()
    {
        SaveData data = new SaveData();
        data.AddData("id", new SaveData(id));
        data.AddData("hp", new SaveData(hp));
        data.AddData("maxHp", new SaveData(maxHp));
        data.AddData("atk", new SaveData(atk));
        data.AddData("speed", new SaveData(speed));
        return data;
    }

    public virtual void LoadFromSaveData(SaveData data)
    {
        hp = data.GetInt("hp");
        maxHp = data.GetInt("maxHp");
        atk = data.GetInt("atk");
        speed = data.GetInt("speed");
    }
}
