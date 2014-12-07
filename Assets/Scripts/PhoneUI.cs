using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PhoneUI : MonoBehaviour {

    enum Screen
    {
        Sign,
        Victory,
        InGame,
    }

    public GameObject SignPanel;
    public GameObject VictoryPanel;
    public GameObject InGamePanel;

    // 3D Game UI components
    public GameObject GameUI3DRoot;

    public Text SignText;

    List<string> signs;
    Screen current;

    void Awake()
    {
        signs = new List<string>();
        current = Screen.InGame;
    }
    
    void Start()
    {
        PushSign("Hello!");
    }

    void ShowScreen(Screen screen)
    {
        SignPanel.SetActive(screen == Screen.Sign);
        VictoryPanel.SetActive(screen == Screen.Victory);
        InGamePanel.SetActive(screen == Screen.InGame);
        GameUI3DRoot.SetActive(screen == Screen.InGame);
        current = screen;

        if (signs.Count > 0) { SignText.text = signs[0]; }
    }

    public void OnScreenTap()
    {
        if (current == Screen.Sign)
        {
            signs.RemoveAt(0);
            if (signs.Count > 0)
            {
                ShowScreen(Screen.Sign);
            }
            else
            {
                ShowScreen(Screen.InGame);
            }
        }
    }

    public void PushSign(string text)
    {
        if (!signs.Contains(text))
        {
            signs.Add(text);
        }

        if (current != Screen.Sign)
        {
            ShowScreen(Screen.Sign);
        }
    }
}
