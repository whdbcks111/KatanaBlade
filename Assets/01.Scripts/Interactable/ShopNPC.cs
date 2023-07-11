using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : Interactable
{
    List<Dictionary<string, object>> itemTable;

    private void Start()
    {
        itemTable = CSVReader.Read("Item/ItemTable");

        for (int i = 0; i < itemTable.Count; i++)
        {
            Debug.Log(itemTable[i]["ItemName"]);
        }
    }

    public override void OnInteract(Player player)
    {
        // 상점 주인 상호작용 구현
    }
}
