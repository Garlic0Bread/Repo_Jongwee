using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameZARInit : MonoBehaviour
{
    [SerializeField] private GameObject errorScreen;
    [SerializeField] private GameObject inactiveScreen;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private Button subscribeButton;

    private const string ApiUrl = "https://api.gamezar.co.za/get-user-studio-status";
    private const string StudioId = "2";

    private void Start()
    {
      
        var url = Application.absoluteURL;
        var queryParams = UrlHelper.GetQueryParams(url);

        if (!queryParams.ContainsKey("msisdn"))
        {
            ShowErrorScreen();
            return;
        }

        string msisdn = queryParams["msisdn"];
        StartCoroutine(CheckUserStatus(msisdn));
    }

    private IEnumerator CheckUserStatus(string msisdn)
    {
        
        var jsonBody = JsonUtility.ToJson(new RequestBody { msisdn = msisdn, studioId = StudioId });
        var request = new UnityWebRequest(ApiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("API Request Failed: " + request.error);
            ShowErrorScreen();
            yield break;
        }

        var response = JsonUtility.FromJson<ResponseBody>(request.downloadHandler.text);

        if (response.status == "Active")
        {
            gameplayUI.SetActive(true);
        }
        else
        {
            gameplayUI.SetActive(false);
            ShowInactiveScreen();
        }
    }

    private void ShowErrorScreen()
    {
        errorScreen.SetActive(true);
    }

    private void ShowInactiveScreen()
    {
        inactiveScreen.SetActive(true);
        subscribeButton.onClick.AddListener(() =>
        {
            Application.OpenURL("https://gamezar.co.za");
        });
    }

    [System.Serializable]
    private class RequestBody
    {
        public string msisdn;
        public string studioId;
    }

    [System.Serializable]
    private class ResponseBody
    {
        public string status;
        public string result;
    }
}
