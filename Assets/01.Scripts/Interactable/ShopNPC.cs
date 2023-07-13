using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopNPC : Interactable
{
    List<Dictionary<string, object>> itemTable;
    List<int> itemList = new List<int>();

    private void Start()
    {
        itemTable = CSVReader.Read("Item/ItemTable");

    }

    public override void OnInteract(Player player)
    {
        // 상점 주인 상호작용 구현

    }

    [ContextMenu("랜덤 아이템 가져오기")]
    public void GetRandomItems()
    {
        itemList.Clear();
        itemList = GetItemID();
        foreach (var item in itemList)
        {
            Debug.Log(itemTable[item]["ItemName"]);
        }
    }
    
    private List<int> GetItemID()
    {
        List<int> result = new List<int>();
        
        while (result.Count < 4)
        {
            while (true)
            {
                int rand = Random.Range(0, itemTable.Count);
                bool notEqual = true;
                foreach (var id in result)
                {
                    if(rand == id)
                    {
                        notEqual = false;
                        break;
                    }
                }
                if (notEqual == true)
                {
                    result.Add(rand);
                    break;
                }
            }
        }
        return result;
    }
}
