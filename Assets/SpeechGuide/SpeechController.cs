using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Audio;
using UnityEngine;

public class SpeechController : MonoBehaviour
{
    public AudioSource source;
    
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    
    // Play the speech (reading of the text parameter)
    // If the text doesn't already exist, a request for it will be sent
    public void Play(string text)
    {
        // Hash the text to get the filename
        string filename = Md5(text);
        AudioClip speechSound = (AudioClip) Resources.Load(filename);

        if (speechSound == null)
        {
            StartCoroutine(RequestSpeechAudio(text, filename));
        }
        else
        {
            PlayLoadedSound(speechSound);
        }
    }
    
    private static string Md5 (string str)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding ();
        byte[] bytes = encoding.GetBytes (str);      
        var sha = new System.Security.Cryptography.MD5CryptoServiceProvider();
        
        return System.BitConverter.ToString (sha.ComputeHash (bytes));
    }
    
    // Send a POST request used for getting the speech sound
    private IEnumerator RequestSpeechAudio(string textRequest, string filename)
    {
        string reqQuery = "?text=" + textRequest + "&filename=" + filename;

        UnityWebRequest www = UnityWebRequest.Get("localhost:3000" + reqQuery);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogWarning(www.error);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            LoadAndPlaySound(filename);
        }
    }

    private void LoadAndPlaySound(string filename)
    {
        AudioClip speechSound = (AudioClip) Resources.Load(filename);

        if (speechSound == null)
        {
            Debug.LogException(new Exception("Could not load " + filename));
        }
        
        PlayLoadedSound(speechSound);
    }

    private void PlayLoadedSound(AudioClip sound)
    {
        source.clip = sound;
        source.Play();
    }
}
