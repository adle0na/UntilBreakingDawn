
public enum WeaponName {AssaultRifle = 0}

[System.Serializable]
public class WeaponSetting
{
    public WeaponName _WeaponName;          // ���� �̸�
    public int        _currentMagazine;     // ���� źâ ��
    public int        _maxMagazine;         // �ִ� źâ ��
    public int        _currentAmmo;         // ���� ź�� ��
    public int        _maxAmmo;             // �ִ� ź�� ��
    public float      _attackRate;          // ���� �ӵ�
    public float      _attackDistance;      // ���� ��Ÿ�
    public bool       _isAutomaticAttack;   // ���� ���� ����
}