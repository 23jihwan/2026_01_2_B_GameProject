using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Inventory/ItemSO")]
public class ItemSO : ScriptableObject
{
    public int id;
    public string itemName;
    public string description;
    public string nameEng;
    

   
    public ItemType itemType;
    public int price;
    public int power;
    public int level;
    public bool isStackedble;
    public Sprite icon;

    public string DisplayName
    {
        get { return string.IsNullOrEmpty(nameEng) ? itemName : nameEng; }
    }
}
