using UnityEngine;

public class CoreLoader : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerPrefab;
    [SerializeField] private GameObject moneyManagerPrefab;
    [SerializeField] private GameObject upgradeManagerPrefab;
    [SerializeField] private GameObject weaponManagerPrefab;

    private void Awake()
    {
        if (GameManager.Instance == null)
            Instantiate(gameManagerPrefab);

        if (MoneyManager.Instance == null)
            Instantiate(moneyManagerPrefab);

        if (UpgradeManager.Instance == null)
            Instantiate(upgradeManagerPrefab);

        if (WeaponManager.Instance == null)
            Instantiate(weaponManagerPrefab);
    }
}
