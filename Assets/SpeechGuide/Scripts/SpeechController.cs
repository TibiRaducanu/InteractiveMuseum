using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.Events;

public class SpeechController : MonoBehaviour
{
    public AudioSource source;
    public UnityEvent onAudioStarted;
    
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    
    // Play the speech (reading of the text parameter)
    // If the text doesn't already exist, a request for it will be sent
    public void Play(string text, GuideController.SelectedLanguage lang)
    {
        // Hash the text to get the filename
        string filename = Md5(text);
        AudioClip speechSound = (AudioClip) Resources.Load(filename);

        if (speechSound == null)
        {
            StartCoroutine(RequestSpeechAudio(text, filename, lang));
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
        
        return BitConverter.ToString (sha.ComputeHash (bytes));
    }
    
    // Send a GET request used for getting the speech sound
    private IEnumerator RequestSpeechAudio(string textRequest, string filename, GuideController.SelectedLanguage lang)
    {
        string languageId = "";
        
        switch (lang)
        {
            case GuideController.SelectedLanguage.En:
                languageId = "en";
                break;
            case GuideController.SelectedLanguage.Fr:
                languageId = "fr";
                break;
        }
        
        string reqQuery = "?text=" + textRequest + "&lang=" + languageId;
        
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip("localhost:3000" + reqQuery, AudioType.WAV)) {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError || uwr.isHttpError) {
                Debug.LogWarning(uwr.error);
                
                yield break;
            }

            AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
            
            // use audio clip
            SaveAndPlaySound(clip, filename);
        }
    }
    
    // Write the received audio file to disk, so we don't request for it every time
    private void SaveAndPlaySound(AudioClip speechSound, string filename)
    {
        SavWav.Save(filename, speechSound);
        
        PlayLoadedSound(speechSound);
    }

    private void PlayLoadedSound(AudioClip sound)
    {
        source.clip = sound;
        source.Play();
        
        onAudioStarted.Invoke();
    }
}
