using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance { get; private set; }

    private readonly List<AABBCollider> colliders = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void Register(AABBCollider collider)
    {
        if (collider != null && !colliders.Contains(collider))
            colliders.Add(collider);
    }

    public void Unregister(AABBCollider collider)
    {
        colliders.Remove(collider);
    }

    // check for overlapping to each other
    public bool CheckOverlap(
        AABBCollider player,
        Vector3 position,
        ColliderType type,
        out AABBCollider hit)
    {
        hit = null;

        Vector3 playerMin = player.GetMinAt(position);
        Vector3 playerMax = player.GetMaxAt(position);

        foreach (AABBCollider other in colliders)
        {
            if (other == null || other == player || other.Type != type)
                continue;

            other.UpdateBounds();

            if (playerMin.x < other.max.x &&
                playerMax.x > other.min.x &&
                playerMin.y < other.max.y &&
                playerMax.y > other.min.y &&
                playerMin.z < other.max.z &&
                playerMax.z > other.min.z)
            {
                hit = other;
                return true;
            }
        }

        return false;
    }
}