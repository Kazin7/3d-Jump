using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType{
    Consumable
}

public enum ConsumableType
{
    Health,
    Speed,
    Jump
}
[Serializable]
public class ItemDataConsumable{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName ="Item",menuName ="New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;

    [Header("Consumable Info")]
    public ItemDataConsumable consumable;
}
