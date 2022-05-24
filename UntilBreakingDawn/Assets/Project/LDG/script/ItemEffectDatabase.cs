using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
[System.Serializable]
public class ItemEffect
{
    public string itemName; //�������� �̸�(Ű��)
    public string[] part; //����
    public int[] num; //��ġ
}

public class Itemeffet : MonoBehaviour
{
    private ItemEffect[] itemEffects;

    //�ʿ��� ������Ʈ
    private StatusController thePlayerStatus;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    public void UseItem(Item _item)
    {
        if(_item.itemType == Item.ItemType.Potion)
        {
            for(int x=0;x<itemEffects.Length;x++)
            {
                if(itemEffects[x].itemName==_item.itemName)
                {
                    for (int y=0;y<itemEffects[x].part.Length;y++)
                    {
                        switch(itemEffects[x].part[y])
                        {
                            
                        }
                    }
                }
            }
        }
    }
}
