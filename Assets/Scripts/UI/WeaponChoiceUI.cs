using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChoiceUI : MonoBehaviour
{

    public GameObject Shadow;
    public List<GameObject> Weapons = new List<GameObject>();
    public void ChangeIndex(int index)
    {
        Shadow.transform.position = Weapons[index].transform.position;
    }
}
