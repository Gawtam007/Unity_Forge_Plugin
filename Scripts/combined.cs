﻿using Autodesk.Forge.ARKit;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class combined : MonoBehaviour
{
    

    private ForgeLoader forgeLoader;

    public GameObject inputbucket;
    public GameObject inputfile;
 
    /// <summary>
    /// /////////////////////////////////////
    /// </summary>
    /// 

    public string bucketName;
    public string fileName;
    private string acctoken;
    private string URN;
    private string urn;


    [System.Serializable]
    public class Project
    {
        public Meta prj;
    }

    [System.Serializable]
    public class Meta
    {
        public string urn;
    }

    /// <summary>
    /// ///////////////////////////////////////////
    /// </summary>
    /// 
    [System.Serializable]
    public class Project1
    {
        public Meta input;
        public Output output;
    }


    [System.Serializable]
    public class Output
    {
        public List<Formats> formats;
    }

    [System.Serializable]
    public class Formats
    {
        public string type;
        public string scene;
    }

    /// <summary>
    /// ////////////////////////////////
    /// </summary>
    /// 
    private void Awake()
    {
        forgeLoader = FindObjectOfType<ForgeLoader>();
    }


    public void get(string access_token)
    {
        acctoken = access_token;
    }

    public void click()
    {      
        bucketName = inputbucket.GetComponent<Text>().text;
        fileName = inputfile.GetComponent<Text>().text;
        StartCoroutine(Upload1());
    }
   

    private IEnumerator Upload1()
    {
        urn = "urn:adsk.objects:os.object:" + bucketName + "/" + fileName;
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(urn);
        URN = Convert.ToBase64String(bytesToEncode);
        char[] MyChar = { '=' };
        URN = URN.TrimEnd(MyChar);
        //Debug.Log(URN);

        //Debug.Log("create scene access token : " + acctoken);

        /*
        PhotonView photonView = GameObject.Find("Forge Loader").GetComponent<PhotonView>();
        photonView.RPC("values", RpcTarget.All, URN, acctoken, "hello worked");
        */

        forgeLoader.values(URN, acctoken);
        forgeLoader.send();

        Project writePlayer = new Project()
        {
            prj = new Meta()
            {
                urn = URN,
            }
        };
        string jsonString = JsonUtility.ToJson(writePlayer);
        Debug.Log(jsonString);


        using (UnityWebRequest www = UnityWebRequest.Put("https://developer-api-beta.autodesk.io/arkit/v1/" + URN + "/scenes/crgscene", jsonString))
        {
            www.SetRequestHeader("Authorization", "Bearer " + acctoken);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                ///Debug.Log(www.error);
                //Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("completed 1");
                //Debug.Log(www.responseCode);
                //Debug.Log(www.downloadHandler.text);
                click2();
            }
        }

    }

    ////////////////////////////////
    ///
    public void click2()
    {
        StartCoroutine(Upload2());
    }

    private IEnumerator Upload2()
    {
        /*
        urn = "urn:adsk.objects:os.object:" + bucketName + "/" + fileName;
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(urn);
        string URN = Convert.ToBase64String(bytesToEncode);
        */
        Debug.Log(URN);
       


        Project1 writePlayer = new Project1()
        {

            input = new Meta()
            {
                urn = URN,
            },

            output = new Output()
            {
                formats = new List<Formats>
                {
                    new Formats()
                    {
                        type = "arkit",
                        scene = "crgscene"
                    }

                }
            }

        };
        string jsonString = JsonUtility.ToJson(writePlayer);
        Debug.Log(jsonString);


        using (UnityWebRequest www = UnityWebRequest.Put("https://developer-api-beta.autodesk.io/modelderivative/v2/arkit/job", jsonString))
        {
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Authorization", "Bearer " + acctoken);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("complete 2");
               // Debug.Log(www.responseCode);
                //Debug.Log(www.downloadHandler.text);
                click3();
            }
        }


    }

    ////////////////////////
    ///

    public void click3()
    {
        StartCoroutine(Upload3());
    }

    private IEnumerator Upload3()
    {


        using (UnityWebRequest www = UnityWebRequest.Get("https://developer-api-beta.autodesk.io/modelderivative/v2/arkit/" + URN + "/manifest"))
        {
            // www.method = UnityWebRequest.kHttpVerbPUT;
            www.SetRequestHeader("Authorization", "Bearer " + acctoken);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("completed 3");
               // Debug.Log(www.responseCode);
                Debug.Log(www.downloadHandler.text);
            }
        }

    }
}
