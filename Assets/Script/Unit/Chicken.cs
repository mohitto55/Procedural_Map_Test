using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Unit
{
    float MoveCulTIme = 1;
    float moveCulTIme = 1;
    /// <summary>
    /// Update에 호출시 매 호출시 마다 쿨타임을 줄여주고 쿨타임이 0이하가 되면 참을 반환함
    /// </summary>
    /// <param name="cullTIme"></param>
    /// <param name="culTIme"></param>
    /// <returns></returns>
    bool CulTime(float cullTIme, ref float culTIme)
    {
        if(culTIme >= 0)
        {
            culTIme -= Time.deltaTime;
            return false;
        }
        else
        {
            culTIme = cullTIme;
            return true;
        }
    }
    private void Update()
    {
        if(CulTime(moveCulTIme, ref MoveCulTIme))
        {
            Vector2 RandomPos = (Vector2)transform.position + new Vector2(Random.Range(-2, 3), (Random.Range(-2, 3)));
            MoveToPos(RandomPos);
        }
        else
        {

        }
    }
}
