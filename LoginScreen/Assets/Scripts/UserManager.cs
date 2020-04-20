using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    public enum Screen
    {
        Login,
        Register,
    }

    public InputField username;
    public InputField password;
    public InputField email;
    public InputField username2;
    public InputField password2;

    public GameObject loginScreen;
    public GameObject registerScreen;

    public Text massage;
    public void login()
    {
        if(string.IsNullOrEmpty(username.text) || string.IsNullOrEmpty(username.text))
        {
            massage.text = "username or password is empty";
            return;
        }
      
        string json = "{\"Username\":"+"\""+ username.text+"\"," + "\"Password\":"+"\"" + password.text + "\"}";
        JsonUtility.ToJson(json);
        print(json);
        StartCoroutine(PostRequest("https://ai36uwne3l.execute-api.us-east-1.amazonaws.com/default/LogIn", json));
    }

    public void registerUser()
    {
        string json = "{\"Username\":" + "\"" + username2.text + "\", \"Password\":" + "\"" + password2.text + "\" , \"Email\":" + "\"" + email.text + "\"}";
        JsonUtility.ToJson(json);
        print(json);
        StartCoroutine(RegisterUser("https://xqcxmxut5b.execute-api.us-east-1.amazonaws.com/default/Registration", json));
    }

    IEnumerator PostRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            massage.text = "Error While Sending: " + uwr.error;
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            massage.text = "Received: " + uwr.downloadHandler.text;
        }
    }

    IEnumerator RegisterUser(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "PUT");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();


        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            massage.text = "Error While Sending: " + uwr.error;
            StartCoroutine(ChangeScene(Screen.Login));
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            massage.text = "Received: " + uwr.downloadHandler.text;
            StartCoroutine(ChangeScene(Screen.Login));
        }
    }
    public void ChangeScene(int s)
    {
        StartCoroutine(ChangeScene((Screen)s));
    }

    IEnumerator ChangeScene(Screen s)
    {
        yield return new WaitForSeconds(1.0f);

        if (s == Screen.Login)
        {
            loginScreen.SetActive(true);
            registerScreen.SetActive(false);
        }
        else if (s == Screen.Register)
        {
            loginScreen.SetActive(false);
            registerScreen.SetActive(true);
        }
    }
}
