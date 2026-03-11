using UnityEngine;

public class PlayerCKernelMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    public float magnetRadius = 2.5f;
    public float pullSpeed = 6f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }

    private void FixedUpdate()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, magnetRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Kernel"))
            {
                Kernel kernel = hit.GetComponent<Kernel>();
                if (kernel != null)
                    kernel.AttractTo(transform, pullSpeed);
            }
        }
    }
}
