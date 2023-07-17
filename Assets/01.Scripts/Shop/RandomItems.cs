using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomItems : MonoBehaviour
{
    private List<Dictionary<string, object>> itemTable;
    private List<int> itemList;

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
                    if (rand == id)
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

