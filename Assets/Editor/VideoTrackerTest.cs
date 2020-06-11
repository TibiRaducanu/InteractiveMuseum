using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoTrackerTest : MonoBehaviour
{
    [Test]
    public void CheckFrameRate()
    {
        var currentVideo = GameObject.FindObjectOfType<VideoPlayer>();
        var currentSlider = GameObject.FindObjectOfType<Slider>();

        if (currentVideo && currentSlider)
        {
            Assert.IsTrue(currentSlider.value - (float)currentVideo.frame / (float)currentVideo.frameCount < 0.2);
        }
    }
}
