using TMPro;
using UnityEngine;

public class LosePanelUI : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public TextMeshProUGUI waveText;

    private void OnEnable()
    {
        int previousWave = GameManager.Instance.Data.currentWave;
        waveText.text = $"Wave {enemySpawner.Wave} {(enemySpawner.Wave > previousWave ? $" (+{100 * (enemySpawner.Wave - previousWave)}$)" : "")}";
        GameManager.Instance.Data.currentWave = enemySpawner.Wave;
        if (enemySpawner.Wave > previousWave) MoneyManager.Instance.AddMoney(100 * (enemySpawner.Wave - previousWave));
        GameManager.Instance.SaveGame();
    }
}
