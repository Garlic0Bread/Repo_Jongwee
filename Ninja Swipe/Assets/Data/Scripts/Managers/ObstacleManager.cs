using DG.Tweening;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private Rigidbody2D rb2;
    private float moveSpeed;
    private float _startTime;
    private Vector3 _startPos;
    private bool _isDeflected = false;

    public float curveIntensity = 4f;//wideness of the swerve is
    [SerializeField] private bool isSwervingBeak;
    [SerializeField] private float curveFrequency = 2f; //how fast it swerves

    [Header("Item Type")]
    [SerializeField] private bool isRottenEgg;
    [SerializeField] private bool isNormalEgg;
    [SerializeField] private bool isGoldenEgg;
    [SerializeField] private bool isObstacle;

    [SerializeField] private PlayerVFXProfile vfxProfile;
    [SerializeField] private int bossDamage;
    [SerializeField] private float minSpeed;
    [SerializeField] private float maxSpeed;

    private void Update()
    {
        if (!isSwervingBeak) return;
        if (_isDeflected) return;

        //this is calculated for the swerving beak only
        //forward movement
        float newX = transform.position.x - (moveSpeed * Time.deltaTime);
        float sineOffset = Mathf.Sin((Time.time - _startTime) * curveFrequency) * curveIntensity;//sine wave movement

        transform.position = new Vector3(newX, _startPos.y + sineOffset, transform.position.z);

        //rotate to face the direction of the wave
        float angle = Mathf.Cos((Time.time - _startTime) * curveFrequency) * 30f;
        transform.rotation = Quaternion.Euler(0, 0, angle + 180f);
    }

    private void OnEnable()
    {
        rb2 = GetComponent<Rigidbody2D>();

        _startTime = Time.time;//syncing for sine wave movement
        moveSpeed = Random.Range(minSpeed, maxSpeed);

        if (isSwervingBeak) return;
        rb2.linearVelocity = Vector2.left * moveSpeed;//only move forward if we're NOT a swerving object
    }
    private void OnDisable()
    {
        _isDeflected = false;
        GetComponentInChildren<SpriteRenderer>().DOColor(Color.white, 0.2f);
        //reset object to OG colour after its disabled and re-enters the pool
    }

    public void Deflect()
    {
        if (_isDeflected) return;
        _isDeflected = true;

        //visual feedback: punches up the object in size
        transform.DOComplete();
        transform.DOPunchScale(Vector3.one * 0.3f, 0.2f);

        Vector2 targetDir = Vector2.right;
        rb2.linearVelocity = targetDir * (moveSpeed * 2f);
        transform.rotation = Quaternion.Euler(0, 0, 0); //point straight right

        GetComponentInChildren<SpriteRenderer>().DOColor(Color.cyan, 0.2f);
    }
    public void OnDestroyedByPlayer()
    {
        if (isRottenEgg || isSwervingBeak)
        {
            Deflect();
            return;
        }

        if (isObstacle)
        {
            if (GameManager.Instance._isTutorialPhase)
                TutorialManager.Instance.TriggerEvent(TutorialTrigger.ObstacleCleared);

            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region OBSTACLE COLLISION
        //IF THIS IS AN OBSTACLE = DAMAGE PLAYER
        if (!_isDeflected && collision.CompareTag("Player") && !isNormalEgg && !isGoldenEgg)
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(1);

            EggSplatUI.Instance.ShowSplat();

            #region VFX
            Vector3 direction = (transform.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
            GameObject vfxPrefab = vfxProfile.GetVFX(VFXType.Collisions);
            Vector3 contactPoint = transform.position;

            VFXPooler.Instance.SpawnFromPool(vfxPrefab, contactPoint, rotation, 1f);
            #endregion

            if (GameManager.Instance._isTutorialPhase)
                TutorialManager.Instance.TriggerEvent(TutorialTrigger.ObstacleCleared);

            gameObject.SetActive(false);
        }

        //IF THIS IS A DEFLECTED ENEMY ITEM = DAMAGE BOSS
        else if (_isDeflected && collision.gameObject.CompareTag("Boss"))
        {
            collision.GetComponent<BossHealth>().TakeDamage(bossDamage);
            _isDeflected = false;

            #region VFX
            Vector3 direction = (transform.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
            GameObject vfxPrefab = vfxProfile.GetVFX(VFXType.Collisions);
            Vector3 contactPoint = transform.position;

            VFXPooler.Instance.SpawnFromPool(vfxPrefab, contactPoint, rotation, 1f);
            #endregion

            gameObject.SetActive(false);
        }
        #endregion

        #region EGG COLLECTION COLLISION
        //IF THIS IS A NORMAL EGG 
        if (isNormalEgg && collision.CompareTag("Player"))
        {
            GameProgressManager.Instance.AddEggs(isGolden: false, collision.transform.position);
            gameObject.SetActive(false);
        }

        //IF THIS IS A GOLDEN EGG
        else if (isGoldenEgg && collision.CompareTag("Player"))
        {
            GameProgressManager.Instance.AddEggs(isGolden: true, collision.transform.position);
            gameObject.SetActive(false);
        }
        #endregion
    }
}
