using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameData Data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        SaveSystem.Save(Data);
    }

    public void LoadGame()
    {
        Data = SaveSystem.Load();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void SetCoins(int amount)
    {
        Data.coins = amount;
        SaveGame();
    }
}
