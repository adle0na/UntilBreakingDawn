
public enum WeaponName {AssaultRifle = 0, Revolver, CombatKnife, HandGrenade}

[System.Serializable]
public struct WeaponSetting
{
    public WeaponName _WeaponName;          // ���� �̸�
    public int        _damage;              // ���� ���ݷ�
    public int        _currentMagazine;     // ���� źâ ��
    public int        _maxMagazine;         // �ִ� źâ ��
    public int        _currentAmmo;         // ���� ź�� ��
    public int        _maxAmmo;             // �ִ� ź�� ��
    public float      _attackRate;          // ���� �ӵ�
    public float      _attackDistance;      // ���� ��Ÿ�
    public bool       _isAutomaticAttack;   // ���� ���� ����
}
