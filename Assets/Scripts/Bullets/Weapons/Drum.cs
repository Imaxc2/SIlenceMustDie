using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class Drum : BulletBase
{
    public SpriteRenderer Inner;
    private BoxCollider2D col;
    private AudioSource source;

    [Header("Drum Sounds")]
    public AudioClip mainSound;
    public AudioClip secondarySound;
    public int repeatCount = 4;
    public float pitchStep = 0.1f;
    

    private int totalAttacks = 0;
    private int current = 0;

    private float basePitch = 1f;

    [Header("Animations")]
    public float minSize = 0.6f;
    public float maxSize = 1f;
    public Color minColor = Color.white;
    public Color maxColor = Color.yellow;
    public float changeSpeed = 0.05f;

    [Header("Balls")]
    public GameObject Ball;
    public AudioClip BallSound;
    private float _ballSize;

    public int FireDamage = 10;

    void Start()
    {
        float flat = UpgradeManager.Instance.GetUpgradeValueSum("blackhole_damage");
        float boost = UpgradeManager.Instance.GetUpgradeValueSum("fireboost_damage") / 100;

        maxSize = 1f + UpgradeManager.Instance.GetUpgradeValueSum("blackhole_size") / 100;
        Damage = (int)((BaseDamage + flat) * (1f + boost));

        FireDamage += (int) UpgradeManager.Instance.GetUpgradeValueSum("fireboost_damage");

        source = GetComponent<AudioSource>();
        col = GetComponent<BoxCollider2D>();
        if (col != null) col.enabled = false;
        _ballSize = 1f + UpgradeManager.Instance.GetUpgradeValueSum("light_size") / 100;
    }

    public override void Attack()
    {
        totalAttacks++;

        transform.DOKill();
        Inner.DOKill();

        Rotate();
        AnimateScale();
        AnimateColor();
        PlayDrumSound();
        StartCoroutine(Collider());

        if (totalAttacks % 6 == 0 )
        { 
            if (WeaponManager.Instance.IsBought("2")) Attack1();
            totalAttacks = 0;
        }
    }

    private void PlayDrumSound()
    {
        if (current < repeatCount)
        {
            source.pitch = basePitch + (pitchStep * current);
            source.PlayOneShot(mainSound);
            current++;
        }
        else
        {
            source.pitch = 1f;
            source.PlayOneShot(secondarySound);
            current = 0;
        }
    }

    private void AnimateScale()
    {
        float size = current < repeatCount ? minSize : maxSize;

        transform.DOScale(Vector3.one * size, changeSpeed)
            .SetEase(Ease.InOutSine);
    }

    private void Rotate()
    {
        float nextZ = (transform.eulerAngles.z + 45f) % 360f;

        transform.DORotate(
            new Vector3(0f, 0f, nextZ),
            changeSpeed,
            RotateMode.Fast
        ).SetEase(Ease.InOutSine);
    }

    private void AnimateColor()
    {
        Color target = current < repeatCount ? minColor : maxColor;

        Inner.DOColor(target, changeSpeed)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }


    private IEnumerator Collider()
    {
        if (col == null)
            yield break;

        if (current != repeatCount)
        {
            yield return new WaitForSeconds(changeSpeed);
            col.enabled = true;
            yield return new WaitForFixedUpdate();
            col.enabled = false;
        } 
    }

    private void Attack1()
    {
        source.PlayOneShot(BallSound);

        GameObject ball1 = Instantiate(Ball);
        GameObject ball2 = Instantiate(Ball);
        ball1.transform.localScale *= _ballSize;
        ball2.transform.localScale *= _ballSize;

        StartCoroutine(OrbitOnce(ball1, ball2));
    }

    private IEnumerator OrbitOnce(GameObject b1, GameObject b2)
    {
        float duration = 0.5f;
        float timer = 0f;
        float angle = 0f;
        float radius = 3f;

        while (timer < duration)
        {
            angle += (360f / duration) * Time.deltaTime;

            float rad = angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;

            b1.transform.position = transform.position + offset;
            b2.transform.position = transform.position - offset;

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(b1);
        Destroy(b2);
    }


}
