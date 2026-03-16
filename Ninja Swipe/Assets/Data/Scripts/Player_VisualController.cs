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

    private SkinData currentSkin;

    [SerializeField] float slashDuration = 0.15f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        SkinManager.Instance.ApplyEquippedSkin(this);
        spriteRenderer.sprite = currentSkin.fallingSprite;

    }
    private void Update()
    {
        if (currentState == VisualState.Slashing)
        {
            slashTimer -= Time.deltaTime;

            if (slashTimer <= 0)
                ExitSlash();
        }
    }

    public void ApplySkin(SkinData skin)
    {
        currentSkin = skin;
        SetState(VisualState.Falling);
    }

    void ApplyState(VisualState newState)
    {
        currentState = newState;

        if (currentSkin == null) return;

        switch (currentState)
        {
            case VisualState.Falling:
                spriteRenderer.sprite = currentSkin.fallingSprite;
                break;

            case VisualState.FlappinUp:
                spriteRenderer.sprite = currentSkin.flapSprite;
                break;
        }
    }
    public void SetState(VisualState newState)
    {
        if (currentState == newState) return;
        if (currentState == VisualState.Slashing) return;

        ApplyState(newState);
    }

    void ExitSlash()
    {
        float yVel = GetComponentInParent<Rigidbody2D>().linearVelocity.y;

        if (yVel > 0.1f)
            ApplyState(VisualState.FlappinUp);
        else
            ApplyState(VisualState.Falling);
    }
    public void PlaySlash()
    {
        if (currentSkin == null) return;

        currentState = VisualState.Slashing;

        slashTimer = slashDuration;
        spriteRenderer.sprite = currentSkin.slashSprite;
    }
}
