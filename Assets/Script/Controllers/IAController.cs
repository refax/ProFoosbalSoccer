using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour {

    enum IASTATES
    {
        DETECTING,
        LOAD_KICK,
        DO_KICK
    }

    private float m_ThinkTimerCounter = 0.0f;
    private float m_NearestResetTimerCounter = 0.0f;

    [SerializeField]
    private float m_ThinkTime = 1.0f;

    private Vector3 m_TargetPosition;

    [SerializeField]
    private Ball m_BallToFollow;



    [SerializeField]
    [Tooltip("From Goalkeeper to Attack. Order is IMPORTANT!")]
    private FootballPlayerOnBar[] m_PlayersUnderControl;

    private int m_SelectedPlayer = 0;


    IASTATES m_CurrentState;

    private float m_FastKickTimer = 0.0f;

    private static float LOAD_KICK_TIME_4_FAST_KICK = 0.2f; //ms

    private FootballPlayer[] m_NearestPlayers;



    /* UNITY Stardard function */
    private void Awake()
    {
        /* By default selct player in the middle*/
        m_SelectedPlayer = m_PlayersUnderControl.Length / 2;

        if (m_PlayersUnderControl.Length == 0)
        {
            Debug.LogError("Not player assigned to " + name);
        }

        if(m_BallToFollow == null)
        {
            Debug.LogError("Not ball assigned to " + name);
        }
        else
        {
            m_TargetPosition = new Vector3(m_BallToFollow.transform.position.x, m_BallToFollow.transform.position.y, m_BallToFollow.transform.position.z);
        }

        for (int i = 0; i < m_PlayersUnderControl.Length; i++)
        {
            m_PlayersUnderControl[i].BarID = i;
            //m_PlayersUnderControl[i].OnBallIsInMyArea += OnBallInAreaHandler;
            m_PlayersUnderControl[i].OnPlayerHasDetectBall += OnPlayerHasDetectBall;
        }

        m_NearestPlayers = new FootballPlayer[4];

        for (int i = 0; i < m_PlayersUnderControl.Length; i++)
        {
            m_NearestPlayers[i] = null;
        }
    }

    private void Start()
    {
        m_PlayersUnderControl[m_SelectedPlayer].SelectPlayers();
        m_PlayersUnderControl[m_SelectedPlayer + 1].SelectPlayers();
        m_CurrentState = IASTATES.DETECTING;
    }

    private void Update()
    {
        m_ThinkTimerCounter += Time.deltaTime;

        if (m_ThinkTimerCounter > m_ThinkTime )
        {
            // Update target position
            m_TargetPosition = new Vector3(m_BallToFollow.transform.position.x, m_BallToFollow.transform.position.y, m_BallToFollow.transform.position.z);
            m_ThinkTimerCounter = 0.0f;

            /* Change selection if required TODO generalized*/
            if(m_TargetPosition.x > 5)
            {
                SelectPlayersBar(0);
            }
            else
            {
                SelectPlayersBar(2);
            }

            FollowTheTarget();
        }

        if(m_NearestResetTimerCounter > m_ThinkTime*4)
        {
            for (int i = 0; i < m_PlayersUnderControl.Length; i++)
            {
                m_NearestPlayers[i] = null;
            }
        }
    }
    


    private void FollowTheTarget()
    {
        for(int i=m_SelectedPlayer; i<=(m_SelectedPlayer+1); i++)
        {
            if (m_NearestPlayers[i] == null)
            {
                m_NearestPlayers[i] = ExtractNearestToBall(i, m_BallToFollow.transform.position);
            }

            float distance = Mathf.Abs(m_NearestPlayers[i].transform.position.z - m_BallToFollow.transform.position.z);

            if ( m_TargetPosition.x - m_PlayersUnderControl[i].transform.position.x < -0.5) //se la palla è davanti segui
            {
                if (distance < 0.2)
                {
                    m_PlayersUnderControl[i].UpDownDirection = 0.0f;
                }
                else if ((m_NearestPlayers[i].transform.position.z) > m_BallToFollow.transform.position.z)
                {
                    m_PlayersUnderControl[i].UpDownDirection = -0.5f;
                }
                else if ((m_NearestPlayers[i].transform.position.z) < m_BallToFollow.transform.position.z)
                {
                    m_PlayersUnderControl[i].UpDownDirection = 0.5f;
                }
            }
            else // fuggi
            {
                if (distance < 1)
                {
                    if(m_PlayersUnderControl[i].transform.position.z >= 0)
                    {
                        m_PlayersUnderControl[i].UpDownDirection = -0.5f;
                    }
                    else
                    {
                        m_PlayersUnderControl[i].UpDownDirection = 0.5f;
                    }
                }
                else
                {
                    m_PlayersUnderControl[i].UpDownDirection = 0.0f;
                }

            }
        }
    }

    
    private void OnDestroy()
    {
        for (int i = 0; i < m_PlayersUnderControl.Length; i++)
        {
            //m_PlayersUnderControl[i].OnBallIsInMyArea -= OnBallInAreaHandler;
            m_PlayersUnderControl[i].OnPlayerHasDetectBall -= OnPlayerHasDetectBall;
        }
    }
    



    /*
    private void OnBallInAreaHandler(int i_BarID, Ball i_Ball)
    {
        SelectPlayersBar(i_BarID);
        m_BallToFollow = i_Ball;
       // m_CurrentState = IASTATES.DETECTING;

        
    }
    */


    private void SelectPlayersBar(int i_BarID)
    {
        m_PlayersUnderControl[m_SelectedPlayer].DeselectPlayers();
        m_PlayersUnderControl[m_SelectedPlayer + 1].DeselectPlayers();

        m_SelectedPlayer = i_BarID;

        m_PlayersUnderControl[m_SelectedPlayer].SelectPlayers();
        m_PlayersUnderControl[m_SelectedPlayer + 1].SelectPlayers();

    }

    
    private void OnPlayerHasDetectBall(int m_BarID)
    {
        Debug.Log("Ball detected");
        m_PlayersUnderControl[m_BarID].DoKick(true);
        // SelectPlayersBar(m_BarID);


        // m_CurrentState = IASTATES.LOAD_KICK;
    }
    




    /* IA State Machine */
    private void STM_Handler( float i_DeltaTime )
    {
        switch(m_CurrentState)
        {
            case IASTATES.DETECTING:
               // DetectingPhase();
                break;
            case IASTATES.LOAD_KICK:
                
              //  LoadKickPhase();
                break;
            case IASTATES.DO_KICK:
              //  DoKickPhase(i_DeltaTime);
                break;
        }
    }

    /** STATES FUNCTION */
    private void DetectingPhase()
    {
        /* Follow Ball */
        /*if(m_BallToFollow != null)
        {
            FootballPlayer nearestPlayer = ExtractNearestToBall(m_BallToFollow.transform.position);

            float distance = Mathf.Abs(nearestPlayer.transform.position.z - m_BallToFollow.transform.position.z);

            if (distance < 0.2)
            {
                m_PlayersUnderControl[m_SelectedPlayer].UpDownDirection = 0.0f;
            }
            else if((nearestPlayer.transform.position.z) > m_BallToFollow.transform.position.z)
            {
                m_PlayersUnderControl[m_SelectedPlayer].UpDownDirection = -1.0f;
            }
            else if((nearestPlayer.transform.position.z) < m_BallToFollow.transform.position.z)
            {
                m_PlayersUnderControl[m_SelectedPlayer].UpDownDirection = 1.0f;
            }
        }*/
    }

    private void LoadKickPhase()
    {
        m_PlayersUnderControl[m_SelectedPlayer].DoKick(true);
        m_CurrentState = IASTATES.DO_KICK;
        m_FastKickTimer = 0.0f;
    }

    private void DoKickPhase(float i_DeltaTime)
    {
        m_FastKickTimer += i_DeltaTime;

        if(m_FastKickTimer > LOAD_KICK_TIME_4_FAST_KICK)
        {
            m_PlayersUnderControl[m_SelectedPlayer].DoKick(false);
            m_CurrentState = IASTATES.DETECTING;
        }

    }

    FootballPlayer ExtractNearestToBall( int i_BarID, Vector3 i_BallPosition )
    {
        FootballPlayer[] playerList = m_PlayersUnderControl[i_BarID].PlayerList;
        FootballPlayer result = playerList[0];
        float minDistance = Mathf.Abs(result.transform.position.z - i_BallPosition.z);

        for(int i=1; i<playerList.Length; i++)
        {
            float distance = Mathf.Abs(playerList[i].transform.position.z - i_BallPosition.z);

            if(distance < minDistance)
            {
                minDistance = distance;
                result = playerList[i];
            }
        }
        


        return result;
    }


    public void ResetPlayerPosition()
    {
        for (int i = 0; i < m_PlayersUnderControl.Length; i++)
        {
            Vector3 pos = m_PlayersUnderControl[i].transform.position;
            m_PlayersUnderControl[i].transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }

}
