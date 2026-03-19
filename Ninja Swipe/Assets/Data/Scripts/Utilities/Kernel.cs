using UnityEngine;

public class Kernel : MonoBehaviour
{//used by kernel magnet to attract kernel to player when close eneough
    private Rigidbody2D rb2;
    public bool isAttracted = false;
    [SerializeField] private float moveSpeed;
    [SerializeField] private PlayerVFXProfile vfxProfile;

    private void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }
    private void OnDisable()
    {
        isAttracted = false;
    }
    private void FixedUpdate()
    {
        if (isAttracted)
            return;
        Vector2 velocity = rb2.linearVelocity;
        velocity.x = -moveSpeed;

        rb2.linearVelocity = velocity;
    }

    public void AttractTo(Transform target, float speed)
    {
        isAttracted = true;
        transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isAttracted = false;

            AudioClip vfxSoundEffect = vfxProfile.Get_SFX(VFXType.Collisions);
            AudioSource.PlayClipAtPoint(vfxSoundEffect, transform.position, 0.5f);

            GameProgressManager.Instance.AddKernels(1);
            gameObject.SetActive(false);
        }
    }
}
