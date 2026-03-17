using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private PlayerVFXProfile vfxProfile;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Rigidbody2D rb2;

    private void OnEnable()
    {
        rb2 = GetComponent<Rigidbody2D>();
        rb2.linearVelocity = Vector2.right * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool obstacle = collision.CompareTag("Obstacle");
        bool tutorial_obstacle = collision.CompareTag("Tutorial_Obstacle");

        var bossHealth = collision.GetComponent<BossHealth>();

        if (obstacle || tutorial_obstacle)
        {
            ObstacleManager obstacleManager = collision.GetComponent<ObstacleManager>();

            #region VFX & SFX
            Vector2 contactPoint = collision.ClosestPoint(transform.position);
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);

            GameObject vfxPrefab = vfxProfile.GetVFX(VFXType.Hit_Enemy);
            AudioClip vfxSoundEffect = vfxProfile.Get_SFX(VFXType.Hit_Enemy);
            AudioSource.PlayClipAtPoint(vfxSoundEffect, contactPoint, 0.5f);

            VFXPooler.Instance.SpawnFromPool(vfxPrefab, contactPoint, rotation, 1f);
            #endregion

            if (obstacleManager != null)
                obstacleManager.OnDestroyedByPlayer();

            if(bossHealth != null)
            {
                bossHealth.TakeDamage(10);
            }
        }
    }
}

