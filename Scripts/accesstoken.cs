using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static combined;

public class accesstoken : MonoBehaviour
{

    private const string client_id = "RXUwZCdXhtlMxPvUDNW5T7x8PJA3oLJ2";
    private const string client_secret = "Rl51n6YC8YcAHMG5";
    public class token
    {
        public string access_token;
    }
    public token data;
    private combined combined;
    private string acctoken;

    private void Awake()
    {
        combined = FindObjectOfType<combined>();
    }

    public void click2()
    {
        StartCoroutine(Upload());
    }
    
    private IEnumerator Upload()
    {
        WWWForm form = new WWWForm();

        form.AddField("client_id", client_id);
        form.AddField("client_secret", client_secret);
        form.AddField("grant_type", "client_credentials");
        form.AddField("scope", "data:write data:read bucket:create bucket:delete");

        UnityWebRequest www = UnityWebRequest.Post("https://developer.api.autodesk.com/authentication/v1/authenticate", form);
        www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");


        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            data = JsonUtility.FromJson<token>(www.downloadHandler.text);
            Debug.Log("completed 0");
            acctoken = data.access_token;
            Debug.Log(acctoken);
            
            combined.get(acctoken);
        }

    }
}