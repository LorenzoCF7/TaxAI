using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Data/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
}
