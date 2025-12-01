using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Game/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string id;

    public string upgradeName;
    public string description;
    public Sprite icon;

    public bool isPercent;

    public UpgradeCategory category;

    public WeaponData weapon;

    [System.Serializable]
    public class UpgradeLevel
    {
        public int cost;
        public float value;
    }

    public UpgradeLevel[] levels;
}
