using UnityEngine;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance;
    [SerializeField] private List<GameObject> environments;

    private void Awake()
    {
        Instance = this;
    }

    public void SetEnvironment(GameObject target)
    {
        foreach (var env in environments)
        {
            env.SetActive(env == target);
        }
    }
}
