using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Tests
{
    public class NewTestScript
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
}
