using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI MoneyText;
    private int previousMoney;

    private void Start()
    {
        previousMoney = MoneyManager.Instance.GetMoney();
        MoneyText.text = "0";
    }

    private void OnEnable()
    {
        MoneyManager.OnMoneyChanged += UpdateUI;
    }

    private void OnDisable()
    {
        MoneyManager.OnMoneyChanged -= UpdateUI;
    }

    private void UpdateUI(int newAmount)
    {
        MoneyText.text = (newAmount - previousMoney).ToString();
    }
}
