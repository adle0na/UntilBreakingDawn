using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WeaponDamage_Rock
{
    Hand = 2,
    Knife = 5,
    Pickax = 30,
    Axe = 10
}
public class Resource_Rock : Resource_Base
{
    WeaponDamage_Rock weaponDamage = WeaponDamage_Rock.Knife; // 차후 수정 : 기본 데미지는 hand

    public AudioSource[] AudioSources;

    private void OnEnable()
    {
        base.HP = 10; // ******************************** 본래 수치 = 100 **************************************
    }
    public override void OnHit()
    {
        // Resource_Base의 damage를 수정된 데미지수치로 전환 후 데미지 계산 함수 실행
        base.damage = (int)weaponDamage;
        //Debug.Log(damage);
        base.OnHit();
        //Debug.Log(HP);
        AudioSources[0].Play();
    }
    // 접촉한 무기의 종류 확인
    private void OnCollisionExit(Collision collision)
    {
        // 접촉한 무기(컬라이더)의 종류에 따라 데미지 수정
        if (collision.gameObject.CompareTag("Player"))
        {
            weaponDamage = WeaponDamage_Rock.Hand;
        }
        OnHit();
        // 자원의 HP==0시 자원 파괴
        if (HP <= 0)
        {
            AudioSources[1].Play();
            Destroyed();
        }
    }

    // 영역 내에 플레이어가 들어올 시 자원의 기본정보 세팅
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            resource = this.gameObject;
        }
    }
}
