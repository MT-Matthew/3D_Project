using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint
{
    public string itemName;
    public string Req1;
    public string Req2;

    public int Req1Amount;
    public int Req2Amount;

    public int numOfRequirements;
    public int numberOfItemToProduce;

    public Blueprint(string name, int producedItems, int reqNUM, string R1, int R1num, string R2, int R2num)
    {
        itemName = name;

        numOfRequirements = reqNUM;
        numberOfItemToProduce = producedItems;

        Req1 = R1;
        Req2 = R2;

        Req1Amount = R1num;
        Req2Amount = R2num;
    }
}
