using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UpgradeButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("UI")]
    public Image outlineImage;
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI nameText;
    public Image priceImage;
    public Image upgradeIcon;

    [Header("Upgrade Settings")]
    public UpgradeData upgradeData;
    public float holdTime = 1f;

    private Material mat;
    private bool isHolding = false;
    private float holdProgress = 0f;

    private void Awake()
    {
        mat = Instantiate(outlineImage.material);
        outlineImage.material = mat;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        MoneyManager.OnMoneyChanged += OnMoneyChangedHandler;
    }

    private void OnDisable()
    {
        MoneyManager.OnMoneyChanged -= OnMoneyChangedHandler;
    }

    private void OnMoneyChangedHandler(int _)
    {
        UpdateUI();
    }

    private void Update()
    {
        if (mat == null) return;
        if (isHolding)
        {
            holdProgress += Time.deltaTime / holdTime;
            holdProgress = Mathf.Clamp01(holdProgress);

            mat.SetFloat("_HoldProgress", holdProgress);

            if (holdProgress >= 1f)
            {
                TryBuy();
                StopHold();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!UpgradeManager.Instance.CanAfford(upgradeData))
            return;

        if (!UpgradeManager.Instance.CanUpgrade(upgradeData))
            return;

        isHolding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopHold();
    }

    private void StopHold()
    {
        isHolding = false;
        holdProgress = 0f;
        mat.SetFloat("_HoldProgress", 0f);
    }

    private void TryBuy()
    {
        if (UpgradeManager.Instance.CanAfford(upgradeData) &&
            UpgradeManager.Instance.CanUpgrade(upgradeData))
        {
            UpgradeManager.Instance.BuyUpgrade(upgradeData);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        int level = UpgradeManager.Instance.GetUpgradeLevel(upgradeData.id);
        int max = UpgradeManager.Instance.GetMaxUpgradeLevel(upgradeData);

        float upgradeProgress = (float)level / max;
        mat.SetFloat("_UpgradeProgress", upgradeProgress);

        nameText.text = upgradeData.upgradeName;

        float currentValue = UpgradeManager.Instance.GetCurrentValue(upgradeData);


        float nextValue = UpgradeManager.Instance.GetNextValue(upgradeData);

        if (nextValue < 0)
        {
            valueText.text = "MAX";
            priceImage.enabled = false;
            priceText.enabled = false;
        }
        else
        {
            valueText.text = $"+{nextValue}{(upgradeData.isPercent ? "%" : "")}";

            priceImage.enabled = true;
            priceText.enabled = true;
        }
        if (upgradeIcon != null)
            upgradeIcon.sprite = upgradeData.icon;

        int cost = UpgradeManager.Instance.GetNextCost(upgradeData);
        priceText.text = (cost == -1) ? "MAX" : $"{cost}";
    }
}
