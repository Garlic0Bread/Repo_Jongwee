using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Material mat;
    private float distance;

    [Range(0f, 0.5f)]
    [SerializeField] private float speed = 0.2f;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        distance += Time.deltaTime * speed;
        mat.SetTextureOffset("_MainTex", Vector2.right * distance);//sliding image/texture across a plane
    }
}
