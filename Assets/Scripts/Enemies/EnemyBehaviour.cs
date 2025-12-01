using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Movement")]
    public float Speed = 1f;
    public Vector2 MoveDirection;

    [Header("HP")]
    public int HpRangeLow;
    public int HpRangeHigh;
    public int HP;

    [Header("References")]
    public EnemySpawner spawner;
    public ParticleSystem DamageParticles;
    private EnemyHealthShader healthShader;
    private SpriteRenderer innerSprite;

    public int MoneyAmount;

    private Color baseColor;

    private bool isDead = false;
    private bool isVisible = false;

    private void Awake()
    {
        HP = Random.Range(HpRangeLow, HpRangeHigh);
    }

    private void Start()
    {
        innerSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        baseColor = innerSprite.color;

        healthShader = GetComponentInChildren<EnemyHealthShader>();
        healthShader.maxHP = HP;
    }

    private void Update()
    {
        if (EnemyLimitUI.GamePaused)
            return;
        if (isDead)
            return;

        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        if (!isVisible && vp.x >= 0 && vp.x <= 1 && vp.y >= 0 && vp.y <= 1)
        {
            isVisible = true;
            spawner.RegisterActiveEnemy(gameObject);
        }

        transform.Translate(MoveDirection * (Speed * Time.deltaTime), Space.World);

        transform.Rotate(0, 0, 180f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;

        if (collision.CompareTag("Bullet"))
        {
            BulletBase bullet = collision.GetComponent<BulletBase>();
            Drum drum = collision.GetComponent<Drum>();
            TakeDamage(bullet.Damage);
            if (drum != null)
            {
                if (WeaponManager.Instance.IsBought("1"))
                {
                    StartCoroutine(TakeFire(drum.FireDamage));
                }
            }
        }
    }

    private IEnumerator TakeFire(int fireDamage)
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            Instantiate(DamageParticles, transform.position, Quaternion.identity);
            TakeDamage(fireDamage);
        }
    }

    private void TakeDamage(int dmg)
    {
        if (isDead)
            return;

        if (EnemyLimitUI.GamePaused)
            return;

        FlashColor();
        healthShader.TakeDamage(dmg);

        HP -= dmg;

        if (HP <= 0)
        {
            MoneyManager.Instance.AddMoney(MoneyAmount);
            StartCoroutine(Die());
        }
    }



    private void FlashColor()
    {
        if (innerSprite == null) return;

        innerSprite.DOKill();
        innerSprite.color = Color.white;

        innerSprite.DOColor(baseColor, 0.2f)
                   .SetEase(Ease.InOutSine);
    }


    private IEnumerator Die()
    {
        isDead = true;

        KillTweens();

        Tween shrink = transform.DOScale(0.1f, 0.25f)
                                .SetEase(Ease.InOutSine);

        yield return shrink.WaitForCompletion();

        CleanupAndDestroy();
    }


    private void OnBecameInvisible()
    {
        if (!isDead)
            CleanupAndDestroy();
    }

    private void CleanupAndDestroy()
    {
        KillTweens();

        if (spawner != null)
            spawner.RemoveEnemy(gameObject);

        Destroy(gameObject);
    }

    private void KillTweens()
    {
        if (this == null) return;

        transform.DOKill();
        if (innerSprite != null)
            innerSprite.DOKill();
    }
}
