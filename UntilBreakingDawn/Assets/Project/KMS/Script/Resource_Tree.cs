using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WeaponDamage_Tree
{
    Hand = 3,
    Knife = 15,
    Pickax = 10,
    Axe = 30,
    Default = 5
}
public class Resource_Tree : Resource_Base
{    
    WeaponDamage_Tree weaponDamage = WeaponDamage_Tree.Knife;

    public AudioSource[] AudioSources;

    public override void OnHit()
    {
        // Resource_Base의 damage를 수정된 데미지수치로 전환 후 데미지 계산 함수 실행
        base.damage = (int)weaponDamage;
        base.OnHit();
        AudioSources[0].Play();
        if (HP <= 0)
        {
            AudioSources[1].Play();
            base.ResourceGenerate();
            ResourceObject.Inst.Type = 1;
            base.Destroyed();
        }
    }

    // 접촉한 무기의 종류 확인
    // 영역 내에 플레이어가 들어올 시 자원의 종류 세팅
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ******************************** test를 위해 Player tag 사용 중 차후 근접무기 tag로 변경할것 ****************************
        {
            resource = this.gameObject;
            weaponDamage = WeaponDamage_Tree.Hand;
        }
        else
        {
            weaponDamage = WeaponDamage_Tree.Default;
        }
    }
}
