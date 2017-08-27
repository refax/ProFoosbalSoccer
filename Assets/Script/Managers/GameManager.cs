using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Ball m_Ball;

    [SerializeField]
    private TableSoccerDoor[] m_Doors;

    [SerializeField]
    private UIManager m_UIManager;

    [SerializeField]
    private GameObject m_DefaultBallPositionForPlayerOne;
    [SerializeField]
    private GameObject m_DefaultBallPositionForPlayerTwo;

    private int m_PlayerOnePoint = 0;
    private int m_PlayerTwoPoint = 0;


    public GameObject m_PlayerOne;
    public GameObject m_PlayerTwo;

    [SerializeField]
    private float m_MatchDurationInMillisecond = 240;

    private bool m_MatchIsOver = false;


    // Use this for initialization
    private void Awake()
    {
        if( m_Ball == null)
        {
            Debug.LogError("Ball not assigned in " + name);
        }

        if (m_Doors.Length == 0)
        {
            Debug.LogError("Doors non assigned in " + name);
        }
        else
        {
            for(int i=0; i< m_Doors.Length; i++)
            {
                m_Doors[i].OnGoalScored += ScoreGoal;
            }
        }

        if (m_UIManager == null)
        {
            Debug.LogError("UIManager not assigned in " + name);
        }
        Physics.gravity = new Vector3(0, -40, 0);
    }

    private void Start()
    {
        m_UIManager.UpdateTimeLeft(m_MatchDurationInMillisecond);
    }

    private void Update()
    {
        m_MatchDurationInMillisecond -= Time.deltaTime;

        if(m_MatchDurationInMillisecond < 0 && !m_MatchIsOver)
        {
            m_UIManager.UpdateTimeLeft(0);
            m_UIManager.MatchIsOver();
            m_MatchIsOver = true;
        }
        else if(!m_MatchIsOver)
        {
            m_UIManager.UpdateTimeLeft(m_MatchDurationInMillisecond);
        }
    }

    /**
     * Player ID is 1 for Player One, 2 for player TWO 
     */
    private void ScoreGoal(int m_PlayerID)
    {
       
        Vector3 position = Vector3.zero;
        switch(m_PlayerID)
        {
            case 1: //Goal for Player TWO
                m_PlayerTwoPoint++;
                position = m_DefaultBallPositionForPlayerOne.transform.position;
                break;
            case 2: //Goal for Player ONE
                m_PlayerOnePoint++;
                position = m_DefaultBallPositionForPlayerTwo.transform.position;
                break;
        }

        m_UIManager.UpdateScore(m_PlayerOnePoint, m_PlayerTwoPoint);

        ResetAllPostiion(position);


    }

    private void ResetAllPostiion( Vector3 i_BallNewPosition )
    {
        Vector3 velocity = Vector3.zero;
        m_Ball.gameObject.SetActive(false);
        m_Ball.SetVelocity(velocity);
        m_Ball.SetPosition(i_BallNewPosition);
        m_PlayerOne.GetComponent<PlayerController>().ResetPlayerPosition();

        if (m_PlayerTwo.GetComponent<IAController>() != null)
            m_PlayerTwo.GetComponent<IAController>().ResetPlayerPosition();
        else
            m_PlayerTwo.GetComponent<PlayerController>().ResetPlayerPosition();

        m_Ball.gameObject.SetActive(true);
    }

}
