using UnityEngine;

[System.Flags]
public enum PlayerPermissions
{
    None = 0,
    Flap = 1,
    Attack = 2,
    Ability = 4,
    Gameplay = 8
}

public class Player_Controller : MonoBehaviour
{
    private Rigidbody2D rb2;
    private RaycastHit2D[] hits;
    private int currentAttack = 0;

    [SerializeField] private PlayerVFXProfile vfxProfile;
    [SerializeField] private Player_VisualController visualController;

    [Header("Flap Physics")]
    [SerializeField] private float maxHeight = 6f;
    [SerializeField] private float maxFallSpeed = -12f;

    [Header("Rotation")]
    [SerializeField] private float upRotation = 25f;
    [SerializeField] private float downRotation = -60f;
    [SerializeField] private float rotationLerpSpeed = 6f;

    [Header("Projectile Ability")]
    [SerializeField] private float abilityCooldown = 5f;
    [SerializeField] private AbilityCooldownUI abilityCooldownUI;

    [Header("Melee Attack Settings")]
    [SerializeField] private float timeSinceAttack = 0.0f;
    [SerializeField] private float timeBtwAttacks = 0.25f;
    [SerializeField] private Transform flapPoof_Transform;
    [SerializeField] private Transform attackTransform;
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float attackRange;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransform.position, attackRange);
    }
    private void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        timeSinceAttack = timeBtwAttacks;
    }
    private void Update()
    {
        HandleRotation();
        timeSinceAttack += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        ClampFallSpeed();
    }

    void HandleRotation()
    {
        if (!GameManager.Instance.HasAbility(PlayerPermissions.Flap))
            return;

        float targetZ = rb2.linearVelocity.y > 0 ? upRotation : downRotation;

        float newZ = Mathf.LerpAngle(
            transform.eulerAngles.z,
            targetZ,
            rotationLerpSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(0, 0, newZ);
    }
    void ClampFallSpeed()
    {
        if (!GameManager.Instance.HasAbility(PlayerPermissions.Flap))
        {
            rb2.gravityScale = 0;
            return;
        }

        rb2.gravityScale = 3;

        if (rb2.linearVelocity.y < maxFallSpeed)
        {
            rb2.linearVelocity = new Vector2(rb2.linearVelocity.x, maxFallSpeed);
        }

        if (rb2.linearVelocity.y < -0.1f && !visualController.IsSlashing)
        {
            visualController.SetState(VisualState.Falling);
        }
    }

    public void Melee_Slash()
    {
        if (!GameManager.Instance.HasAbility(PlayerPermissions.Attack))
            return;

        if (timeSinceAttack >= timeBtwAttacks)
        {
            visualController.PlaySlash();//SLASH SPRITE

            #region Slash VFX & SFX
            AudioClip sfx_Slash = vfxProfile.Get_SFX(VFXType.Melee_Slash);//SLASH SOUND
            GameObject vfx_SlashPrefab = vfxProfile.GetVFX(VFXType.Melee_Slash);//SLASH VFX

            SFXPooler.Instance.PlaySFX(sfx_Slash, 0.7f);
            VFXPooler.Instance.SpawnFromPool(vfx_SlashPrefab, attackTransform.position, vfx_SlashPrefab.transform.rotation, 1f);
            #endregion

            //ATTACKING LOGIC :)
            hits = Physics2D.CircleCastAll(attackTransform.position, attackRange, transform.right, 0f, attackableLayer);

            for (int i = 0; i < hits.Length; i++)
            {
                #region HIT VFX
                Vector3 direction = (hits[i].transform.position - transform.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
                Vector3 contactPoint = hits[i].transform.position;

                GameObject vfxPrefab = vfxProfile.GetVFX(VFXType.Hit_Enemy);
                VFXPooler.Instance.SpawnFromPool(vfxPrefab, contactPoint, rotation, 1f);
                #endregion

                ObstacleManager obstacleManager = hits[i].collider.GetComponent<ObstacleManager>();
                obstacleManager.OnDestroyedByPlayer();//object handles its own death
            }

            currentAttack++;
            if (currentAttack > 3)
                currentAttack = 1;

            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            timeSinceAttack = 0.0f;
        }
    }
    public void Projectile_Slash()
    {
        if (!GameManager.Instance.HasAbility(PlayerPermissions.Ability))
            return;

        if (timeSinceAttack < timeBtwAttacks)
            return;

        if (abilityCooldownUI.IsCoolingDown)
            return;

        GameObject projectilePrefab = vfxProfile.GetVFX(VFXType.Projectile);
        VFXPooler.Instance.SpawnFromPool(projectilePrefab, attackTransform.position, projectilePrefab.transform.rotation, 5f);

        abilityCooldownUI.StartCooldown(abilityCooldown);

        timeSinceAttack = 0f;
    }
    public void Flap(float flapForce)
    {
        if (!GameManager.Instance.HasAbility(PlayerPermissions.Flap))
            return;

        if(transform.position.y >= maxHeight) 
            return;

        rb2.linearVelocity = new Vector2(rb2.linearVelocity.x, 0f);
        rb2.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);
        visualController.SetState(VisualState.FlappinUp);

        GameObject vfxPrefab = vfxProfile.GetVFX(VFXType.FlapPoof);
        VFXPooler.Instance.SpawnFromPool(vfxPrefab, flapPoof_Transform.position, vfxPrefab.transform.rotation, 0.6f);
    }
}
