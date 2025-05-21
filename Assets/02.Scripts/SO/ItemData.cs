using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Eqipable,
    Consumable,
    Resource
}

public enum ConsumableType
{
    Health,
    Hunger
}
[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName ="Item", menuName ="New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string des;
    public ItemType type;
    public Sprite icon;
    public GameObject prefab;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

}
