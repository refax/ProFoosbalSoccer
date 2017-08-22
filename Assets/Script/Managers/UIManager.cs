using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private Text m_ScoreText;

    [SerializeField]
    private Text m_TimeLeft;


    [SerializeField]
    private GameObject m_GameScene;

    [SerializeField]
    private GameObject m_PauseUI;

    [SerializeField]
    private GameObject m_ScoreUI;

    [SerializeField]
    private GameObject m_FinishUI;

    [SerializeField]
    private Button m_ResumeButton;


    [SerializeField]
    private Button m_BackToMainMenu;
    [SerializeField]
    private Button m_BackToMainMenu2;


    private void Awake()
    {
        m_ScoreUI.SetActive(true);
        m_PauseUI.SetActive(false);
        m_FinishUI.SetActive(false);

        m_BackToMainMenu.onClick.AddListener(BacktoMain);
        m_BackToMainMenu2.onClick.AddListener(BacktoMain);
        m_ResumeButton.onClick.AddListener(Resume);
  
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !m_FinishUI.activeSelf)
        {
          //  m_ScoreUI.SetActive(false);
            m_PauseUI.SetActive(true);

            Time.timeScale = 0.0f;
        }
    }

    public void UpdateScore(int i_PlayerOnePoints, int i_PlayerTwoPoints)
    {
        m_ScoreText.text = i_PlayerOnePoints + " - " + i_PlayerTwoPoints;
    }


    private void BacktoMain()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void Resume()
    {
        //m_ScoreUI.SetActive(true);
        m_PauseUI.SetActive(false);

        Time.timeScale = 1.0f;
    }


    private void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateTimeLeft( float timeMS)
    {
        //float totSec = timeMS / 1000f;

        int minutes = ((int)(timeMS) % 3600) / 60;
        int seconds = ((int)(timeMS) % 3600) % 60;

        m_TimeLeft.text = "Time left: " + minutes.ToString() + ":" + seconds.ToString();
    }


    public void MatchIsOver()
    {
        m_PauseUI.SetActive(false);
        m_FinishUI.SetActive(true);

        Time.timeScale = 0.0f;
    }
    
}
