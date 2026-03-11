using UnityEngine;

public enum VisualState
{
    Falling,
    Slashing,
    FlappinUp
}

public class Player_VisualController : MonoBehaviour
{
    public bool IsSlashing => currentState == VisualState.Slashing;
    private SpriteRenderer spriteRenderer;
    private VisualState currentState;
    private float slashTimer;

    [SerializeField] private Sprite slashingJoseph;
    [SerializeField] private Sprite flappingJoseph;
    [SerializeField] private Sprite fallingJoseph;
    [SerializeField] float slashDuration = 0.15f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetState(VisualState.Falling);
    }
    private void Update()
    {
        if (currentState == VisualState.Slashing)
        {
            slashTimer -= Time.deltaTime;
            if (slashTimer <= 0)
            {
                ExitSlash();
            }
        }
    }

    public void SetState(VisualState newState)
    {
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
                spriteRenderer.sprite = fallingJoseph;
                break;

            case VisualState.FlappinUp:
                spriteRenderer.sprite = flappingJoseph;
                break;
        }
    }
    public void PlaySlash()
    {
        currentState = VisualState.Slashing;

        slashTimer = slashDuration;
        spriteRenderer.sprite = slashingJoseph;
    }
    void ExitSlash()
    {
        float yVel = GetComponent<Rigidbody2D>().linearVelocity.y;

        if (yVel > 0.1f)
        {
            ApplyState(VisualState.FlappinUp);
        }
        else
            ApplyState(VisualState.Falling);
    }
}
