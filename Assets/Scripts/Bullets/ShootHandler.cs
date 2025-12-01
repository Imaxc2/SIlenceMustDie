using DG.Tweening;
using UnityEngine;

public class ShootHandler : MonoBehaviour
{
    public GameObject Bullet;
    private BulletBase bullet;

    public float ShootRate;

    private float _mainShot;

    private SpriteRenderer firstOutline;
    private Material firstMat;

    void Start()
    {
        ShootRate /= (1 + UpgradeManager.Instance.GetUpgradeValueSum("blackhole_firerate") / 100);
        bullet = Bullet.GetComponent<BulletBase>();
        firstOutline = Bullet.GetComponentInChildren<SpriteRenderer>();
        firstMat = Instantiate(firstOutline.material);
        firstOutline.material = firstMat;
    }

    private void Update()
    {
        if (EnemyLimitUI.GamePaused) return;
        float mainProgress = 1f - ((_mainShot > 0f ? _mainShot : 0f) / ShootRate);

        firstMat.SetFloat("_Progress", mainProgress);
        if (_mainShot <= 0f)
        {
            bullet.Attack();
            _mainShot = ShootRate;
        }
        _mainShot -= Time.deltaTime;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        Bullet.transform.DOMove(mousePos, 0.2f).SetEase(Ease.OutQuad);
    }
}
