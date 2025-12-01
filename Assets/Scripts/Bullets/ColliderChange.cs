using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class PolygonColliderUpdater : MonoBehaviour
{
    private SpriteRenderer sr;
    private PolygonCollider2D col;
    private Sprite lastSprite;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<PolygonCollider2D>();
        UpdateCollider();
    }

    void LateUpdate()
    {
        if (sr.sprite != lastSprite)
        {
            UpdateCollider();
        }
    }

    void UpdateCollider()
    {
        lastSprite = sr.sprite;
        col.pathCount = sr.sprite.GetPhysicsShapeCount();

        List<Vector2> path = new List<Vector2>();
        for (int i = 0; i < col.pathCount; i++)
        {
            path.Clear();
            sr.sprite.GetPhysicsShape(i, path);
            col.SetPath(i, path);
        }
    }
}
