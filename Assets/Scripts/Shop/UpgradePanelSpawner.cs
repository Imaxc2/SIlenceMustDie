using UnityEngine;

public class UpgradePanelSpawner : MonoBehaviour
{
    public UpgradeButtonHold buttonPrefab;
    public Transform content;
    public WeaponBuyButton buyWeaponPrefab;
    public Transform buttonCenter;


    public void ShowWeaponUpgrades(WeaponData weapon)
    {
        Clear();

        if (!WeaponManager.Instance.IsBought(weapon.id))
        {
            var buyBtn = Instantiate(buyWeaponPrefab, buttonCenter);
            buyBtn.Setup(weapon, this);

            return;
        }

        var list = UpgradeManager.Instance.GetUpgradesForWeapon(weapon);

        foreach (var upgrade in list)
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.upgradeData = upgrade;
        }
    }


    public void ShowGlobalUpgrades()
    {
        Clear();

        var list = UpgradeManager.Instance.GetGlobalUpgrades();

        foreach (var upgrade in list)
        {
            var btn = Instantiate(buttonPrefab, content);
            btn.upgradeData = upgrade;
        }
    }

    private void Clear()
    {
        if (content != null)
            foreach (Transform child in content)
            Destroy(child.gameObject);

        if (buttonCenter != null)
        foreach (Transform child in buttonCenter)
            Destroy(child.gameObject);
    }

}
