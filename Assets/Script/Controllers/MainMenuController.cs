using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    [SerializeField]
    private GameObject m_MainMenuContent;
    [SerializeField]
    private GameObject m_HowToPlayContent;


    [SerializeField]
    Button m_OneVsCPUButton;

    [SerializeField]
    Button m_HowToPlayButton;

    [SerializeField]
    Button m_Quit;

    [SerializeField]
    Button m_BackToMainMenu;


    private void Awake()
    {
        m_OneVsCPUButton.onClick.AddListener(OnOnevsCPUClicked);
        m_Quit.onClick.AddListener(OnQuit);
        m_HowToPlayButton.onClick.AddListener(HowToPlay);
        m_BackToMainMenu.onClick.AddListener(BackToMain);

        m_MainMenuContent.SetActive(true);
        m_HowToPlayContent.SetActive(false);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnOnevsCPUClicked()
    {
        SceneManager.LoadScene("SinglePlayer", LoadSceneMode.Single);
    }

    private void OnQuit()
    {
       
        Application.Quit();
    }

    private void HowToPlay()
    {
        m_MainMenuContent.SetActive(false);
        m_HowToPlayContent.SetActive(true);
    }

    private void BackToMain()
    {
        m_MainMenuContent.SetActive(true);
        m_HowToPlayContent.SetActive(false);
    }
}
