using UnityEngine;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance;

    [SerializeField] private List<EnvironmentData> environments;

    private void Awake()
    {
        Instance = this;
    }

    public void ActivateEnvironment(EnvironmentData environment)
    {
        foreach (var env in environments)
        {
            env.environmentRoot.SetActive(false);
        }

        environment.environmentRoot.SetActive(true);

        PlayerPrefs.SetString("ActiveEnvironment", environment.itemID);
        PlayerPrefs.Save();
    }

    public void LoadEnvironment()
    {
        string envID = PlayerPrefs.GetString("ActiveEnvironment", "");

        foreach (var env in environments)
        {
            if (env.itemID == envID)
            {
                ActivateEnvironment(env);
                return;
            }
        }

        // fallback
        if (environments.Count > 0)
            ActivateEnvironment(environments[0]);
    }
}
