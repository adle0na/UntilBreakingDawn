
public enum WeaponName {AssaultRifle = 0}

[System.Serializable]
public class WeaponSetting
{
    public WeaponName _WeaponName;
    public int        _currentAmmo;
    public int        _maxAmmo;
    public float      _attackRate;
    public float      _attackDistance;
    public bool       _isAutomaticAttack;
}
