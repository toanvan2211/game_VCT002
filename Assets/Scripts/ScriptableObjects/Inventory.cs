using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    public Item currentItem;
    public List<Item> items = new List<Item>();
    public int numberOfKey;

    public void AddItem(Item item)
    {
        if (item.isKey)
        {
            numberOfKey++;
        }
        else
        {
            if (!items.Contains(item))
            {
                items.Add(item);
            }
        }
    }
}
