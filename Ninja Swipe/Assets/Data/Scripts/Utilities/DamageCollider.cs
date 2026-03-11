using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int damageToInflict = 1;
    [SerializeField] private float bouncePower = 15f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            EggSplatUI.Instance.ShowSplat();

            if (health != null && rb != null)
            {
                health.TakeDamage(damageToInflict);

                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);//reset player's current falling speed
                rb.AddForce(Vector2.up * bouncePower, ForceMode2D.Impulse);

            }
        }
    }
}
