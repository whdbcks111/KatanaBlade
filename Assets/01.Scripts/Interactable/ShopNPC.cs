using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;


public class ShopNPC : Interactable
{
    
    private List<Dictionary<string, object>> itemTable;
    private Sprite[] itemSprites = new Sprite[10];
    private List<int> itemIDs;
    private bool[] bought = new bool[4] { false, false, false, false };

    private void Start()
    {
        itemTable = CSVReader.Read("Item/ItemTable");
        GameManager.instance.ShopPanel.SetActive(false);
        for (int i = 0; i < 10; i++)
        {
            Sprite loadImage = Resources.Load<Sprite>("Item/Icon/Accessory/" + itemTable[i]["ItemIcon"]);
            itemSprites[i] = loadImage;
        }

        itemIDs = GetItemID();
    }

    public override void OnInteract(Player player)
    {

        GameManager.instance.ShopPanel.SetActive(!GameManager.instance.ShopPanel.activeSelf);
        if (!GameManager.instance.ShopPanel.activeSelf) return;
        for (int i = 0; i < 4; i++)
        {
            int gold = int.Parse(itemTable[itemIDs[i]]["Gold"].ToString().Replace("G", ""));
            string className = itemTable[itemIDs[i]]["Class"].ToString();
            ShopDisplay display = GameManager.instance.ItemListUI[i].GetComponent<ShopDisplay>();

            display.SetCanBuy(!bought[i]);
            display.itemName = itemTable[itemIDs[i]]["ItemName"].ToString();
            display.itemDescription = itemTable[itemIDs[i]]["Item"].ToString() + "\n"
                + itemTable[itemIDs[i]]["Abillty"].ToString();
            display.itemIcon = itemSprites[itemIDs[i]];
            var itemIcon = GameManager.instance.ItemListUI[i].transform.Find("Image").GetComponent<Image>();
            itemIcon.sprite = itemSprites[itemIDs[i]];
            var goldText = GameManager.instance.ItemListUI[i].GetComponentInChildren<TextMeshProUGUI>();
            goldText.text = itemTable[itemIDs[i]]["Gold"].ToString();

            if (bought[i])
            {
                itemIcon.sprite = Resources.Load<Sprite>("UI/FoxSprite");
                goldText.text = "SOLD";
            }

            GameManager.instance.ItemListUI[i].GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                if (GameManager.instance.Gold - gold >= 0 && display.canBuy == true)
                {
                    System.Type type = System.Type.GetType(className);
                    Item item = System.Activator.CreateInstance(type) as Item;
                    Player.Instance.Inventory.AddItem(item);
                    GameManager.instance.Gold -= gold;
                    display.SetCanBuy(false);
                    bought[i] = true;
                    itemIcon.sprite = Resources.Load<Sprite>("UI/FoxSprite");
                    goldText.text = "SOLD";
                }
            });
        }
    }
    
    private List<int> GetItemID()
    {
        List<int> result = new List<int>();
        
        while (result.Count < 4)
        {
            while (true)
            {
                int rand = Random.Range(0, 10);
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

    public void ExitShop() => GameManager.instance.ShopPanel.SetActive(false);

}
