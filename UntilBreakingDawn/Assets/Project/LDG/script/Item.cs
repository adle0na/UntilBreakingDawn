using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Itme", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string   itemName; //아이템의 이름
    public ItemType itemType; //아이템의 유형
    public Sprite   ItemImage; //아이템의 이미지
    public int      editableValue = 0;

    public enum ItemType
    {
        None,
        Equipment, 
        Potion,
        MagazineMain,
        MagazineSub,
        Ingredient,
        ETC
    }
}
