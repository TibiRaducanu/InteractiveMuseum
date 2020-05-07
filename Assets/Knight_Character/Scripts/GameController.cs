using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject gameCanvas;
    public GameObject guideFrame;
    public GameObject knightsScene;
    
    public void OnImageTracked()
    {
        knightsScene.SetActive(false);
        gameCanvas.SetActive(false);
    } 
    
    public void OnImageLost()
    {
        knightsScene.SetActive(false);
        guideFrame.SetActive(true);
    } 
    
    public void OnPlayGame()
    {
        knightsScene.SetActive(true);
        gameCanvas.SetActive(true);
        guideFrame.SetActive(false);
    }
}
