using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(SpeechController))]
public class GuideController : MonoBehaviour
{
    public enum SelectedLanguage
    {
        En = 0,
        Fr = 1
    };

    private SelectedLanguage _currSelectedLang = SelectedLanguage.En;

    public string textEn;
    public string textFr;
    
    [FormerlySerializedAs("OnAudioEnd")] public UnityEvent onAudioEnd;

    private SpeechController _speechController;
    private Animator _anim;
    private bool _isTalking = false;

    private Coroutine _checkAudioEndCoroutine;
    
    // Start is called before the first frame update
    private void Awake()
    {
        _speechController = GetComponent<SpeechController>();
        _anim = GetComponent<Animator>();
    }

    public void OnLanguageChanged(int option)
    {
        _currSelectedLang = (SelectedLanguage)option; 
        StopTalk();
    }

    public void Talk()
    {
        gameObject.SetActive(true);
        _isTalking = true;

        switch (_currSelectedLang)
        {
            case SelectedLanguage.En:
                _speechController.Play(textEn, _currSelectedLang);
                break;
            case SelectedLanguage.Fr:
                _speechController.Play(textFr, _currSelectedLang);
                break;
        }
        _anim.SetBool("isTalking", true);
    }

    public void OnAudioStarted()
    {
        if (!_isTalking)
        {
            return;
        }

        _checkAudioEndCoroutine = StartCoroutine(CheckAudioEnd());
    }

    private IEnumerator CheckAudioEnd()
    {
        while (_speechController.source.isPlaying)
        {
            yield return null;
        }
        
        StopTalk();
    }

    private void CancelAudioCheck()
    {
        if (_checkAudioEndCoroutine == null)
        {
            return;
        }

        StopCoroutine(_checkAudioEndCoroutine);
    }

    private void PauseTalk()
    {
        _isTalking = false;
        
        _speechController.source.Pause();
        _anim.SetBool("isTalking", false);

        CancelAudioCheck();
        onAudioEnd.Invoke();
    }

    public void StopTalk()
    {
        _isTalking = false;
        
        _speechController.source.Stop();
        _anim.SetBool("isTalking", false);

        CancelAudioCheck();

        if (_checkAudioEndCoroutine != null)
        {
            onAudioEnd.Invoke();
        }
    }

    public void ToggleTalk()
    {
        if (_isTalking)
        {
            PauseTalk();
        }
        else
        {
            Talk();
        }
    }
}
