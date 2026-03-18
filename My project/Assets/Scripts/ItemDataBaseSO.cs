using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBuseSO", menuName = "Inventory/ItemDataBuseSO")]
public class ItemDataBaseSO : ScriptableObject
{
    public List<ItemSO> items=new List<ItemSO>();



    private Dictionary<int, ItemSO> itemsByld;
    private Dictionary<string, ItemSO> itemsByName;

    public void Initialze()
    {
        itemsByld = new Dictionary<int, ItemSO>();
        itemsByName =new Dictionary<string, ItemSO>();

        foreach (var item in items)
        {
            itemsByld[item.id] = item;
            itemsByName[item.itemName] = item;
        }
    }

    public ItemSO GetItmeByld(int id)
    {
        if (itemsByld == null)
        {
            Initialze();   
        }

        if (itemsByName.TryGetValue(name, out ItemSO item))
            return item;

        return null;
    }

    public List<ItemSO>GetitemByType(ItemType type)
    {
        return items.FindAll(item => item.itemType == type);
    }    
}
