using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    public List<WeaponData> allWeapons;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        AddDefaultWeapons();
    }

    public bool IsBought(string weaponId)
    {
        return GameManager.Instance.Data.boughtWeapons.Contains(weaponId);
    }

    public void BuyWeapon(WeaponData weapon)
    {
        if (IsBought(weapon.id)) return;
        if (MoneyManager.Instance.GetMoney() < weapon.price) return;

        MoneyManager.Instance.AddMoney(-weapon.price);
        GameManager.Instance.Data.boughtWeapons.Add(weapon.id);

        GameManager.Instance.SaveGame();
    }
    private void AddDefaultWeapons()
    {
        foreach (var weapon in allWeapons)
        {
            if (weapon.isDefault && !GameManager.Instance.Data.boughtWeapons.Contains(weapon.id))
            {
                GameManager.Instance.Data.boughtWeapons.Add(weapon.id);
            }
        }
    }
    public bool CanBuy(WeaponData weapon)
    {
        return !IsBought(weapon.id) &&
               MoneyManager.Instance.GetMoney() >= weapon.price;
    }

}
