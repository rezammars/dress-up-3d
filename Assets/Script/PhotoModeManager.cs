using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoModeManager : MonoBehaviour
{
    public GameObject dressUpUI;
    public GameObject photoModeUI;
    public Image dressUpBackground;
    public Image photoModeBackground;
    public List<Sprite> backgroundOptions;
    private int currentBackgroundIndex = 0;
    private Sprite savedBackground;

    void Start()
    {
        photoModeUI.SetActive(false);
        
        if (dressUpBackground != null)
        {
            savedBackground = dressUpBackground.sprite;
            dressUpBackground.transform.SetAsFirstSibling();
        }
    }

    public void EnterPhotoMode()
    {
        savedBackground = dressUpBackground.sprite;

        if (photoModeBackground && dressUpBackground)
        {
            photoModeBackground.sprite = dressUpBackground.sprite;
        }

        photoModeUI.SetActive(true);
        dressUpUI.SetActive(false);
    }

    public void ExitPhotoMode()
    {
        if (dressUpBackground) dressUpBackground.sprite = savedBackground;
        if (photoModeBackground) photoModeBackground.sprite = savedBackground;

        photoModeUI.SetActive(false);
        dressUpUI.SetActive(true);
    }

    public void ChangeBackground()
    {
        if (backgroundOptions.Count == 0) return;
        
        currentBackgroundIndex = (currentBackgroundIndex + 1) % backgroundOptions.Count;
        
        if (dressUpBackground) dressUpBackground.sprite = backgroundOptions[currentBackgroundIndex];
        if (photoModeBackground) photoModeBackground.sprite = backgroundOptions[currentBackgroundIndex];
    }

    public void TakeScreenshot()
    {
        StartCoroutine(CaptureScreenshot());
    }

    private IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();
        string fileName = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);

        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log("Screenshot saved to: " + filePath);
    }
}