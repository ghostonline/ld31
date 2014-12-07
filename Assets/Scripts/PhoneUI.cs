using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PhoneUI : MonoBehaviour {
    
    public GameObject SignPanel;
    public GameObject VictoryPanel;
    public GameObject InGamePanel;

    // 3D Game UI components
    public GameObject GameUI3DRoot;

    public void Start()
    {
        SignPanel.SetActive(false);
        VictoryPanel.SetActive(false);
    }

    public void OnScreenTap()
    {
        Debug.Log("Screen tapped");
    }
}
