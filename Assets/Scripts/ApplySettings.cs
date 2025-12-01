using UnityEngine;
using UnityEngine.Audio;

public class ApplySettings : MonoBehaviour
{
    public AudioMixer mixer;
    public float VolumeMultiplayer;

    private void Start()
    {
        ApplyAll();
    }

    public void ApplyAll()
    {
        SetSFX(GameManager.Instance.Data.sfxVolume);
        SetMusic(GameManager.Instance.Data.musicVolume);
        mixer.SetFloat("SFXLowpass", 20000);
    }

    private void SetSFX(float v)
    {
        float linear = Mathf.Clamp01(v / 100f);
        mixer.SetFloat("SFXVolume", LinearToDB(linear));
    }

    private void SetMusic(float v)
    {
        float linear = Mathf.Clamp01(v / 100f) * VolumeMultiplayer;
        mixer.SetFloat("MusicVolume", LinearToDB(linear));
    }

    public float LinearToDB(float x)
    {
        x = Mathf.Clamp01(x);

        float perceptual = Mathf.Pow(x, 1.3f);


        if (perceptual <= 0.0001f)
            return -80f;

        return Mathf.Log10(perceptual) * 20f;
    }

}
