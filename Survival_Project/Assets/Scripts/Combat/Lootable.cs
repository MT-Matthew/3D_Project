using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{
    public List<Lootpossibility> possibleLoot;
    public List<LootRecieved> finalLoot;

    public bool wasLootCalculated;
}

[System.Serializable]
public class Lootpossibility
{
    public GameObject item;
    public int amountMin;
    public int amountMax;
    public float dropPersent;
}

[System.Serializable]
public class LootRecieved
{
    public GameObject item;
    public int amount;
}
