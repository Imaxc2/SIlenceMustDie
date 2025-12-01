using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int coins;
    public int currentWave;

    public float musicVolume;
    public float sfxVolume;
    public bool isFullscreen;
    public List<UpgradeLevelEntry> upgradeLevels;

    public List<string> boughtWeapons = new List<string>();

    public GameData()
    {
        coins = 0;
        currentWave = 1;
        musicVolume = 50;
        sfxVolume = 50;
        isFullscreen = true;

        upgradeLevels = new List<UpgradeLevelEntry>();
        boughtWeapons = new List<string>();
    }
}