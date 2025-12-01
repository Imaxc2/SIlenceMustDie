using UnityEngine;

public class ClearWeapon : BulletBase
{
    public override void Attack() { }

    private void Start()
    {
        float flat = UpgradeManager.Instance.GetUpgradeValueSum("light_damage");
        Damage = (int) (BaseDamage + flat);
    }
}
