using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Sprite playBtn;
    public Sprite pauseBtn;

    private Image btnImage;
    private bool pauseImage = true;
    
    // Start is called before the first frame update
    void Start()
    {
        btnImage = GetComponent<Image>();
    }

    public void SetImageOnPause()
    {
        pauseImage = true;
        btnImage.sprite = pauseBtn;
    }

    public void SetImageOnPlay()
    {
        pauseImage = false;
        btnImage.sprite = playBtn;
    }

    public void ToggleImage()
    {
        if (pauseImage)
        {
            SetImageOnPlay();
        }
        else
        {
            SetImageOnPause();
        }
    }
}
