using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleGUI : MonoBehaviour
{
    public GUISkin customSkin;
    void OnGUI()
    {
        GUI.skin = customSkin;
        float buttonWidth = 100;
        float buttonHeight = 50;
        float halfScreenW = Screen.width / 2;
        float halfButtonW = buttonWidth / 2;

        if (GUI.Button(new Rect(halfScreenW-halfButtonW, 560, buttonWidth, buttonHeight), "Play Game")){
            SceneManager.LoadScene("Game");
        }
            
    }
}
