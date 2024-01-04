using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public enum Equip
{
    Head = 1, Armor = 2, Gloves = 4, leg  = 8
}
public class Bit : MonoBehaviour
{
    public Text text;
    int equip = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            EquipGet(Equip.Head);
        if (Input.GetKeyDown(KeyCode.S))
            EquipGet(Equip.Armor);
        if (Input.GetKeyDown(KeyCode.D))
            EquipGet(Equip.Gloves);
        if (Input.GetKeyDown(KeyCode.F))
            EquipGet(Equip.leg);
        if (Input.GetKeyDown(KeyCode.Q))
            EquipDrop(Equip.Head);
        if (Input.GetKeyDown(KeyCode.W))
            EquipDrop(Equip.Armor);
        if (Input.GetKeyDown(KeyCode.E))
            EquipDrop(Equip.Gloves);
        if (Input.GetKeyDown(KeyCode.R))
            EquipDrop(Equip.leg);
        text.text = Convert.ToString(equip, 2) + '\n'
                  + "Head : " + (equip & 1).ToString() + '\n'
                  + "Armor : " + (equip & 2).ToString() + '\n'
                   + "Gloves : " + (equip & 4).ToString() + '\n'
                  + "leg : " + (equip & 8).ToString() + '\n';

    }
    void EquipGet(Equip parts)
    {
        int partsbit = (int)parts;
        if ((equip & partsbit) != partsbit)
            equip |= partsbit;
        Debug.Log(parts);
    }
    void EquipDrop(Equip parts)
    {
        int partsbit = (int)parts;
        if ((equip & partsbit) == partsbit)
        equip ^= partsbit;
    }
}
