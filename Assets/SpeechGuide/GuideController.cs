using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpeechController))]
public class GuideController : MonoBehaviour
{
    public string text;
    public UnityEvent OnAudioEnd;

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

    public void Talk()
    {
        _isTalking = true;
        
        _speechController.Play(text);
        _anim.SetBool("isTalking", true);

        _checkAudioEndCoroutine = StartCoroutine(CheckAudioEnd());
    }

    private IEnumerator CheckAudioEnd()
    {
        while (_speechController.source.isPlaying)
        {
            yield return null;
        }
        
        StopTalk();
        OnAudioEnd.Invoke();
    }

    public void CancelAudioCheck()
    {
        if (_checkAudioEndCoroutine == null)
        {
            return;
        }

        StopCoroutine(_checkAudioEndCoroutine);
    }

    public void PauseTalk()
    {
        _isTalking = false;
        
        _speechController.source.Pause();
        _anim.SetBool("isTalking", false);

        CancelAudioCheck();
    }

    public void StopTalk()
    {
        _isTalking = false;
        
        _speechController.source.Stop();
        _anim.SetBool("isTalking", false);

        CancelAudioCheck();
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
