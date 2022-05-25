using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Itme", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string   itemName; //�������� �̸�
    public ItemType itemType; //�������� ����
    public Sprite   ItemImage; //�������� �̹���
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
