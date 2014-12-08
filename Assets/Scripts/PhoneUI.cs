using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PhoneUI : MonoBehaviour {

    enum Screen
    {
        Welcome,
        Sign,
        Victory,
        InGame,
    }

    public GameObject SignPanel;
    public GameObject VictoryPanel;
    public GameObject InGamePanel;
    public GameObject WelcomePanel;
    public GameObject TapPrompt;

    // 3D Game UI components
    public GameObject GameUI3DRoot;
    public GameObject WelcomeUI3DRoot;

    public Text SignText;
    public float TapReadyTime;

    // Disable this when ready to tap
    public CharacterMotor character;
    public MouseLook horizLook;
    public MouseLook vertLook;

    List<string> signs;
    Screen current;

    bool showTap;
    float tapTimer;

    void Awake()
    {
        signs = new List<string>();
        current = Screen.Welcome;
    }
    
    void Start()
    {
        ShowScreen(current);
    }

    void Update()
    {
        character.canControl = current == Screen.InGame;
        horizLook.enabled = current == Screen.InGame;
        vertLook.enabled = current == Screen.InGame;

        if (showTap && tapTimer > 0)
        {
            tapTimer -= Time.deltaTime;
            if (tapTimer <= 0)
            {
                TapPrompt.SetActive(true);
            }
        }
    }

    void ShowScreen(Screen screen)
    {
        SignPanel.SetActive(screen == Screen.Sign);
        VictoryPanel.SetActive(screen == Screen.Victory);
        InGamePanel.SetActive(screen == Screen.InGame);
        GameUI3DRoot.SetActive(screen == Screen.InGame);
        WelcomePanel.SetActive(screen == Screen.Welcome);
        WelcomeUI3DRoot.SetActive(screen == Screen.Welcome);
        TapPrompt.SetActive(false);
        showTap = screen == Screen.Welcome || screen == Screen.Sign;
        tapTimer = TapReadyTime;


        current = screen;

        if (signs.Count > 0) { SignText.text = signs[0]; }
    }

    public void OnScreenTap()
    {
        if (showTap && tapTimer > 0) { return; }

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
        else if (current == Screen.Welcome)
        {
            ShowScreen(Screen.InGame);
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
