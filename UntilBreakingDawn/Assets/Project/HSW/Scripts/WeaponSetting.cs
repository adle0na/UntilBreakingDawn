
public enum WeaponName {AssaultRifle = 0, Revolver, CombatKnife, HandGrenade}

[System.Serializable]
public struct WeaponSetting
{
    public WeaponName _WeaponName;          // 무기 이름
    public int        _damage;              // 무기 공격력
    public int        _currentMagazine;     // 현재 탄창 수
    public int        _maxMagazine;         // 최대 탄창 수
    public int        _currentAmmo;         // 현재 탄약 수
    public int        _maxAmmo;             // 최대 탄약 수
    public float      _attackRate;          // 공격 속도
    public float      _attackDistance;      // 공격 사거리
    public bool       _isAutomaticAttack;   // 연속 공격 여부
}
