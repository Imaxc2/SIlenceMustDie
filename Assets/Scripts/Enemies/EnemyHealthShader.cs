using UnityEngine;

public class EnemyHealthShader : MonoBehaviour
{
    public SpriteRenderer circle;
    private Material mat;
    public float maxHP = 100f;
    private float currentHP;

    void Start()
    {
        mat = circle.material;
        currentHP = maxHP;
        float fill = currentHP / maxHP;
        mat.SetFloat("_FillAmount", fill);
    }

    public void TakeDamage(float dmg)
    {
        currentHP = Mathf.Max(0, currentHP - dmg);
        float fill = currentHP / maxHP;
        Debug.Log($"currentHP - {currentHP}, maxHP - {maxHP}");
        mat.SetFloat("_FillAmount", fill);
    }
}

