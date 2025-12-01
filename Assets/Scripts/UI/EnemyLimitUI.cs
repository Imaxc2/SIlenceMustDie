using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EnemyLimitUI : MonoBehaviour
{
    public TextMeshProUGUI EnemyPercentText;
    public Slider EnemyPercentSlider;
    public int EnemyLimit = 10;
    public GameObject Panel;
    public EnemySpawner EnemySpawner;

    public static bool GamePaused = false;

    public GameObject Bullet;
    public AudioMixer mixer;

    public static EnemyLimitUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Start()
    {
        EnemyLimit += (int)UpgradeManager.Instance.GetUpgradeValueSum("enemylimit");
    }

    private void OnEnable()
    {
        EnemySpawner.OnWaveChanged += UpdateUI;
    }

    private void OnDisable()
    {
        EnemySpawner.OnWaveChanged -= UpdateUI;
    }

    public void UpdateUI(int enemyAmount)
    {
        if (GamePaused)
            return;

        if (enemyAmount >= EnemyLimit)
        {

            GamePaused = true;

            EnemySpawner.OnWaveChanged -= UpdateUI;

            if (Panel != null)
                Panel.SetActive(true);

            return;
        }
        
        if (EnemyPercentText != null)
            EnemyPercentText.text = (enemyAmount * 100 / EnemyLimit) + "%";

        if (EnemyPercentSlider != null)
            EnemyPercentSlider.value = (float)enemyAmount / EnemyLimit;

        if (mixer != null)
            mixer.SetFloat("SFXLowpass", 20000 - Mathf.Min(0, (enemyAmount / EnemyLimit - 50)) * 40000f);
    }
}
