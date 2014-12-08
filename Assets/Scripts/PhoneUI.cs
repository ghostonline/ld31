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
    public Text ScoreText;
    public Text TimerText;
    public float TapReadyTime;

    // Disable this when ready to tap
    public CharacterMotor character;
    public MouseLook horizLook;
    public MouseLook vertLook;

    List<string> signs;
    Screen current;

    bool showTap;
    float tapTimer;

    float gameTimer;

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

        if (current == Screen.InGame)
        {
            gameTimer += Time.deltaTime;
            var minutes = Mathf.FloorToInt(gameTimer) / 60;
            var seconds = Mathf.FloorToInt(gameTimer) % 60;
            TimerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
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

    public void ShowVictory(int foundPoints, int foundTotal)
    {
        var minutes = Mathf.FloorToInt(gameTimer) / 60;
        var minuteStr = string.Format("{0} min", minutes);
      
        var seconds = Mathf.FloorToInt(gameTimer) % 60;
        var secondStr = string.Format("{0} sec", seconds);

        string fmt = "You found {0} out of {1} cubes in {2} and {3}.";
        if (foundPoints == 1) { fmt = "You found one out of {1} cubes in {2} and {3}."; }
        ScoreText.text = string.Format(fmt, foundPoints, foundTotal, minuteStr, secondStr);
        ShowScreen(Screen.Victory);
    }
}
