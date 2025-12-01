using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Game/Weapon")]
public class WeaponData : ScriptableObject
{
    public string id;
    public string weaponName;
    public Sprite icon;

    public int price;
    public bool isDefault;
}
