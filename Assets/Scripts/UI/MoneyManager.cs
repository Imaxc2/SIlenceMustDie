using UnityEngine;
using System;

public class MoneyManager : MonoBehaviour
{
    static public MoneyManager Instance { get; private set; }

    public static event Action<int> OnMoneyChanged;

    private int _money;
    private float _saveTimer;
    public float SaveTime = 10f;
    private bool _dirty;

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
        _money = GameManager.Instance.Data.coins;
        OnMoneyChanged?.Invoke(_money);
    }

    public void AddMoney(int amount)
    {
        _money += amount;
        _dirty = true;
        OnMoneyChanged?.Invoke(_money);
    }

    private void Update()
    {
        if (_dirty)
        {
            _saveTimer += Time.deltaTime;
            if (_saveTimer >= SaveTime)
            {
                Save();
                _saveTimer = 0f;
                _dirty = false;
            }
        }
    }

    private void Save()
    {
        GameManager.Instance.Data.coins = _money;
        GameManager.Instance.SaveGame();
    }

    public int GetMoney() => _money;
}
