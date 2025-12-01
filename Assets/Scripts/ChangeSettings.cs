using UnityEngine;
using UnityEngine.UI;

public class ChangeSettings : MonoBehaviour
{
    public Slider SoundSlider;
    public Slider MusicSlider;
    public ApplySettings applySettings;

    private void Start()
    {
        SoundSlider.value = GameManager.Instance.Data.sfxVolume;
        MusicSlider.value = GameManager.Instance.Data.musicVolume;
    }

    public void UpdateUI()
    {
        SoundSlider.value = GameManager.Instance.Data.sfxVolume;
        MusicSlider.value = GameManager.Instance.Data.musicVolume;
    }

    public void OnSliderChanged()
    {
        GameManager.Instance.Data.sfxVolume = Mathf.Round(SoundSlider.value);
        GameManager.Instance.Data.musicVolume = Mathf.Round(MusicSlider.value);
        GameManager.Instance.SaveGame();
        if (applySettings != null) applySettings.ApplyAll();
    }

}
