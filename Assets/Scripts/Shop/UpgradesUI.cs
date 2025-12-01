using TMPro;
using UnityEngine;

public class UpgradesUI : MonoBehaviour
{
    public UpgradePanelSpawner Spawner;
    public WeaponData Weapon;
    public TextMeshProUGUI WeaponName;

    private void Start()
    {
        OpenUpgrades();
    }

    public void OpenUpgrades()
    {
        if (Weapon == null) Spawner.ShowGlobalUpgrades();
        else
        {
            Spawner.ShowWeaponUpgrades(Weapon);
            WeaponName.text = Weapon.weaponName;
        }
    }

    public void ChangeWeapon(WeaponData weapon)
    {
        Weapon = weapon;
        OpenUpgrades();
    }

}
