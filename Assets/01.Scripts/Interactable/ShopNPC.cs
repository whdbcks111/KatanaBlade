using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopNPC : Interactable
{
    public GameObject ShopPanel;
    public GameObject[] ItemListUI = new GameObject[4];
    private List<Dictionary<string, object>> itemTable;
    private Sprite[] itemSprites = new Sprite[10];


    private void Start()
    {
        itemTable = CSVReader.Read("Item/ItemTable");
        ShopPanel.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            Sprite loadImage = Resources.Load<Sprite>("Item/Icon/Accessory/" + itemTable[i]["ItemIcon"]);
            itemSprites[i] = loadImage;
        }
    }

    public override void OnInteract(Player player)
    {
        ShopPanel.SetActive(!ShopPanel.activeSelf);
        List<int> itemIDs = GetItemID();
        // 상점 주인 상호작용 구현
        for (int i = 0; i < 4; i++)
        {
            ItemListUI[i].GetComponentInChildren<TextMeshProUGUI>().text = itemTable[itemIDs[i]]["ItemName"].ToString();
            ItemListUI[i].transform.Find("Image").GetComponent<Image>().sprite = itemSprites[itemIDs[i] - 1];
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
