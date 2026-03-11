using UnityEngine;

public class TataCrow : MonoBehaviour
{
    [Header("Attacking")]
    [SerializeField] private float fireRate;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject beakPrefab;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private Vector2 radius = new(2f, 4f);

    private float _angle;
    private float _shootTimer;
    private Vector3 _centerPos;

    void Start()
    {
        _centerPos = transform.position;
    }
    void Update()
    {
        //calculatioon for elliptical movement
        _angle += Time.deltaTime * moveSpeed;
        float x = Mathf.Cos(_angle) * radius.x;
        float y = Mathf.Sin(_angle) * radius.y;
        transform.position = _centerPos + new Vector3(x, y, 0);

        _shootTimer += Time.deltaTime;
        if (_shootTimer > fireRate)
        {
            ShootBeak();
            _shootTimer = 0;
        }
    }

    private void ShootBeak()
    {
        VFXPooler.Instance.SpawnFromPool(beakPrefab, firePoint.position, Quaternion.identity, 7);
    }
}
