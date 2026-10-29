using UnityEngine;

public enum ColliderType
{
    Player,
    Ground,
    Obstacle,
    FallCollider
}

public class AABBCollider : MonoBehaviour
{
    // will be using 3d square collider
    [Header("Collider Size")]
    [SerializeField] private float _width = 1f;
    [SerializeField] private float _length = 1f;
    [SerializeField] private float _depth = 1f;
    [SerializeField] private ColliderType colliderType;

    public Vector3 min { get; private set; }
    public Vector3 max { get; private set; }

    // gets half of each collider dimension
    public ColliderType Type => colliderType;

    public float HalfWidth => _width * 0.5f;
    public float HalfHeight => _length * 0.5f;
    public float HalfDepth => _depth * 0.5f;

    private void OnEnable()
    {
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.Register(this);

        UpdateBounds();
    }

    private void Start()
    {
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.Register(this);

        UpdateBounds();
    }

    private void OnDisable()
    {
        if (CollisionManager.Instance != null)
            CollisionManager.Instance.Unregister(this);
    }

    public void UpdateBounds()
    {
        // calcu the min and max points
        Vector3 halfSize = new(HalfWidth, HalfHeight, HalfDepth);
        min = transform.position - halfSize;
        max = transform.position + halfSize;
    }

    public Vector3 GetMinAt(Vector3 position)
    {
        return position - new Vector3(HalfWidth, HalfHeight, HalfDepth);
    }

    public Vector3 GetMaxAt(Vector3 position)
    {
        return position + new Vector3(HalfWidth, HalfHeight, HalfDepth);
    }

    private void Update()
    {
        UpdateBounds();

        Vector3 a = min;
        Vector3 b = max;

        Debug.DrawLine(new Vector3(a.x, a.y, a.z), new Vector3(b.x, a.y, a.z), Color.blue);
        Debug.DrawLine(new Vector3(a.x, b.y, a.z), new Vector3(b.x, b.y, a.z), Color.blue);
        Debug.DrawLine(new Vector3(a.x, a.y, b.z), new Vector3(b.x, a.y, b.z), Color.blue);
        Debug.DrawLine(new Vector3(a.x, b.y, b.z), new Vector3(b.x, b.y, b.z), Color.blue);

        Debug.DrawLine(new Vector3(a.x, a.y, a.z), new Vector3(a.x, b.y, a.z), Color.blue);
        Debug.DrawLine(new Vector3(b.x, a.y, a.z), new Vector3(b.x, b.y, a.z), Color.blue);
        Debug.DrawLine(new Vector3(a.x, a.y, b.z), new Vector3(a.x, b.y, b.z), Color.blue);
        Debug.DrawLine(new Vector3(b.x, a.y, b.z), new Vector3(b.x, b.y, b.z), Color.blue);

        Debug.DrawLine(new Vector3(a.x, a.y, a.z), new Vector3(a.x, a.y, b.z), Color.blue);
        Debug.DrawLine(new Vector3(b.x, a.y, a.z), new Vector3(b.x, a.y, b.z), Color.blue);
        Debug.DrawLine(new Vector3(a.x, b.y, a.z), new Vector3(a.x, b.y, b.z), Color.blue);
        Debug.DrawLine(new Vector3(b.x, b.y, a.z), new Vector3(b.x, b.y, b.z), Color.blue);
    }
}