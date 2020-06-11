using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Assertions;

public class VideoTracker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public VideoPlayer video;
    public Slider tracker;
    bool isSliding;
    float cooldown;
    float cooldownValue;

    public void OnPointerDown(PointerEventData eventData)
    {
        isSliding = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        cooldown = cooldownValue;
        float currentFrame = tracker.value * video.frameCount;
        video.frame = (long) Math.Floor(currentFrame);
        isSliding = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        tracker = GetComponent<Slider>();
        isSliding = false;
        cooldown = 0f;
        cooldownValue = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSliding && cooldown < 0.01f)
        {
            tracker.value = (float)video.frame / (float)video.frameCount;
        }

        Assert.IsTrue(tracker.value - (float)video.frame / (float)video.frameCount < 0.1f);
        cooldown -= Time.deltaTime;
        if (cooldown < 0f) cooldown = 0f;
    }
}
