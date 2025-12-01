using TMPro;
using UnityEngine;

public class MoneyShopUI : MonoBehaviour
{
    public TextMeshProUGUI MoneyText;


    private void OnEnable()
    {
        MoneyManager.OnMoneyChanged += UpdateUI;

        MoneyText.text = MoneyManager.Instance.GetMoney().ToString();
    }

    private void OnDisable()
    {
        MoneyManager.OnMoneyChanged -= UpdateUI;
    }

    private void UpdateUI(int newAmount) {
        MoneyText.text = newAmount.ToString();
    }

}
