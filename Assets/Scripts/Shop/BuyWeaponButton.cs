using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponBuyButton : MonoBehaviour
{
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private WeaponData weapon;
    private UpgradePanelSpawner spawner;

    public void Setup(WeaponData weapon, UpgradePanelSpawner spawner)
    {
        this.weapon = weapon;
        this.spawner = spawner;

        priceText.text = weapon.price.ToString();

        buyButton.onClick.AddListener(Buy);
    }

    private void Buy()
    {
        if (WeaponManager.Instance.CanBuy(weapon))
        {
            WeaponManager.Instance.BuyWeapon(weapon);

            spawner.ShowWeaponUpgrades(weapon);
        }
    }
}
