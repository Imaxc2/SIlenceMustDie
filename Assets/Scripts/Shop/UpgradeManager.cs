using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [SerializeField] private List<UpgradeData> allUpgrades;

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

    private UpgradeLevelEntry GetEntry(string id)
    {
        return GameManager.Instance.Data.upgradeLevels
            .Find(x => x.id == id);
    }

    private UpgradeLevelEntry GetOrCreateEntry(string id)
    {
        var entry = GetEntry(id);
        if (entry == null)
        {
            entry = new UpgradeLevelEntry { id = id, level = 0 };
            GameManager.Instance.Data.upgradeLevels.Add(entry);
        }
        return entry;
    }

    public int GetUpgradeLevel(string id)
    {
        return GetEntry(id)?.level ?? 0;
    }

    public int GetMaxUpgradeLevel(UpgradeData upgrade)
    {
        return upgrade.levels.Length;
    }

    public bool CanUpgrade(UpgradeData upgrade)
    {
        int current = GetUpgradeLevel(upgrade.id);
        return current < upgrade.levels.Length;
    }

    public bool CanAfford(UpgradeData upgrade)
    {
        int level = GetUpgradeLevel(upgrade.id);
        if (level >= upgrade.levels.Length)
            return false;

        int cost = upgrade.levels[level].cost;
        return MoneyManager.Instance.GetMoney() >= cost;
    }

    public int GetNextCost(UpgradeData upgrade)
    {
        int level = GetUpgradeLevel(upgrade.id);
        return (level >= upgrade.levels.Length) ? -1 : upgrade.levels[level].cost;
    }


    public float GetCurrentValue(UpgradeData upgrade)
    {
        int level = GetUpgradeLevel(upgrade.id);
        if (level <= 0) return 0f;

        return upgrade.levels[level - 1].value;
    }

    public float GetNextValue(UpgradeData upgrade)
    {
        int level = GetUpgradeLevel(upgrade.id);
        if (level >= upgrade.levels.Length)
            return -1f;

        return upgrade.levels[level].value;
    }

    public void BuyUpgrade(UpgradeData upgrade)
    {
        var entry = GetOrCreateEntry(upgrade.id);
        int currentLevel = entry.level;

        if (currentLevel >= upgrade.levels.Length)
            return;

        int cost = upgrade.levels[currentLevel].cost;
        if (!CanAfford(upgrade))
            return;

        MoneyManager.Instance.AddMoney(-cost);

        entry.level++;

        ApplyUpgrade(upgrade, entry.level);

        GameManager.Instance.SaveGame();
    }

    private void ApplyUpgrade(UpgradeData upgrade, int newLevel)
    {
        float value = upgrade.levels[newLevel - 1].value;
    }

    public List<UpgradeData> GetUpgradesForWeapon(WeaponData weapon)
    {
        return allUpgrades.FindAll(u =>
            u.category == UpgradeCategory.Weapon &&
            u.weapon == weapon
        );
    }

    public List<UpgradeData> GetGlobalUpgrades()
    {
        return allUpgrades.FindAll(u => u.category == UpgradeCategory.Global);
    }

    public float GetUpgradeValue(string id)
    {
        var up = allUpgrades.Find(u => u.id == id);
        if (up == null) return 0f;

        int lvl = GetUpgradeLevel(id);
        if (lvl <= 0) return 0f;

        return up.levels[lvl - 1].value;
    }

    public float GetUpgradeValueSum(string id)
    {
        var up = allUpgrades.Find(u => u.id == id);
        if (up == null) return 0;

        int lvl = GetUpgradeLevel(id);
        if (lvl <= 0) return 0;

        float sum = 0;
        for (int i = 0; i < lvl; i++)
            sum += up.levels[i].value;

        return sum;
    }


}
