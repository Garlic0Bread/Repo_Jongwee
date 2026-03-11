using UnityEngine;

public class RottenRooster : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] float shootVisualDuration = 0.15f;
    [SerializeField] private Sprite attackingRR;
    [SerializeField] private Sprite flyingRR;
    private SpriteRenderer spriteRenderer;
    private VisualState currentState;
    private float shootVisualTimer;
    public bool IsSlashing => currentState == VisualState.Slashing;

    [Header("Movement")]
    [SerializeField] private float floatSpeed = 2f;
    [SerializeField] private float floatHeight = 3f;
    private Vector3 _startPos;

    [Header("Attacking")]
    [SerializeField] private GameObject eggPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 2f;
    private float _fireTimer;

    private void Awake()
    {
        SetState(VisualState.Falling);
    }
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _startPos = transform.position;
    }
    private void Update()
    {
        float newY = _startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;//Make rotten rooster go up and down
        transform.position = new(_startPos.x, newY, _startPos.z);//could've used lerp but found better performance with asin

        _fireTimer += Time.deltaTime;
        if (_fireTimer >= fireRate)
        {
            ShootEgg();
            _fireTimer = 0;
        }

        if (currentState == VisualState.Slashing)
        {
            shootVisualTimer -= Time.deltaTime;
            if (shootVisualTimer <= 0)
            {
                ApplyState(VisualState.Falling);
            }
        }
    }

    private void ShootEgg()
    {
        currentState = VisualState.Slashing;//change state to slashing (slash sprite)
        shootVisualTimer = shootVisualDuration;
        spriteRenderer.sprite = attackingRR;

        Instantiate(eggPrefab, firePoint.position, Quaternion.identity);
    }
    void SetState(VisualState newState)
    {
        //using a state machine to swap sprites instead of animating them to save memory
        if (currentState == newState) { return; }
        if (currentState == VisualState.Slashing) { return; }

        ApplyState(newState);
    }
    void ApplyState(VisualState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case VisualState.Falling:
                spriteRenderer.sprite = flyingRR;
                break;
        }
    }
}
