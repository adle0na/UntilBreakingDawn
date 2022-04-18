using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[SerializeField]
[System.Serializable]
public class ItemEffect
{
    public string itemName; //아이템의 이름(키값)
    public string[] part; //부위
    public int[] num; //수치
}

public class Itemeffet : MonoBehaviour
{
    private ItemEffect[] itemEffects;

    //필요한 컴포넌트
    private StatusController thePlayerStatus;

    private const string HP = "HP", SP = "SP", DP = "DP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY", SATISFY = "SATISFY";

    public void UseItem(Item _item)
    {
        if(_item.itemType == Item.ItemType.Used)
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
