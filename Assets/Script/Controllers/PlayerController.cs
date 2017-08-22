using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private string m_VerticalInputName4LeftPlayer = "P1_VerticalPLeft";
    [SerializeField]
    private string m_VerticalInputName4RightPlayer = "P1_VerticalPRight";


    [SerializeField]
    private string m_KickButtonLeft = "KickButtonP1Left";
    [SerializeField]
    private string m_KickButtonRight = "KickButtonP1Right";

    [SerializeField]
    private string m_ChangePlayerSelectionInputName = "ChangePlayerSelection";


    [SerializeField]
    [Tooltip("From Goalkeeper to Attack. Order is IMPORTANT!")]
    private FootballPlayerOnBar[] m_PlayersUnderControl;

   
    private float m_RotationInput;

    private int m_SelectedPlayer = 0;

    private bool m_RightPlayersAreSelected;


    private void Awake()
    {
        /* By default selct player in the middle*/
        m_SelectedPlayer = m_PlayersUnderControl.Length / 2;

        if(m_PlayersUnderControl.Length == 0)
        {
            Debug.LogError("Not player assigned to " + name);
        }

        for(int i=0; i<m_PlayersUnderControl.Length; i++)
        {
            m_PlayersUnderControl[i].BarID = i;
        }
    }

    private void Start()
    {
        m_PlayersUnderControl[m_SelectedPlayer].SelectPlayers();
        m_PlayersUnderControl[m_SelectedPlayer+1].SelectPlayers();
        m_RightPlayersAreSelected = true;
    }

    // Update is called once per frame
    void Update () {
       
        /* Input handling */
        HandlePlayerSelection();
        HandleUpDownInput();
        HandleTorqueInput();

    }


    private void HandlePlayerSelection()
    {
        m_PlayersUnderControl[m_SelectedPlayer].DeselectPlayers();
        m_PlayersUnderControl[m_SelectedPlayer + 1].DeselectPlayers();

        if (Input.GetButtonDown(m_ChangePlayerSelectionInputName) && m_RightPlayersAreSelected)
        {
            m_SelectedPlayer = 0;
            m_RightPlayersAreSelected = false;
        }
        else if (Input.GetButtonDown(m_ChangePlayerSelectionInputName) && !m_RightPlayersAreSelected)
        {
            m_SelectedPlayer = m_PlayersUnderControl.Length / 2;
            m_RightPlayersAreSelected = true;
        }

        

        m_PlayersUnderControl[m_SelectedPlayer].SelectPlayers();
        m_PlayersUnderControl[m_SelectedPlayer + 1].SelectPlayers();

    }

    private void HandleUpDownInput()
    {
        float m_LeftPlayerVMovement = Input.GetAxis(m_VerticalInputName4LeftPlayer);
        float m_RightPlayerVMovement = Input.GetAxis(m_VerticalInputName4RightPlayer);
        m_PlayersUnderControl[m_SelectedPlayer].UpDownDirection = m_LeftPlayerVMovement;
        m_PlayersUnderControl[m_SelectedPlayer+1].UpDownDirection = m_RightPlayerVMovement;
    }

    private void HandleTorqueInput()
    {
        // m_RotationInput = Input.GetAxis(m_RotationInputName);
        m_PlayersUnderControl[m_SelectedPlayer].DoKick(Input.GetButton(m_KickButtonLeft));
        m_PlayersUnderControl[m_SelectedPlayer+1].DoKick(Input.GetButton(m_KickButtonRight));
    }


    public void ResetPlayerPosition()
    {
       for(int i=0; i < m_PlayersUnderControl.Length; i++)
        {
            Vector3 pos = m_PlayersUnderControl[i].transform.position;
            m_PlayersUnderControl[i].transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }
}
