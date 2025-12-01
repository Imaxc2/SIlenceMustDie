using System.Collections;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    public int BaseDamage;
    public int Damage;

    public abstract void Attack();
}
