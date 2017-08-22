using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void BallIsInMyArea(int i_BarID, Ball m_Ball);
public delegate void PlayerHasDetectBall(int i_BarID);
public delegate void PlayerHasDetectBallNextToHim(int i_BarID, float m_Direction);

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class FootballPlayerOnBar : MonoBehaviour {

    [SerializeField]
    private GameObject m_MyBar;

    [SerializeField]
    private Color m_ColorOnSelection = Color.blue;

    [SerializeField]
    private LayerMask m_BallLayer;

    private int m_BarID = 0;

    private Animator m_Animator;

    private FootballPlayer[] m_Players;

    private MeshRenderer m_BarMeshRender;
    private Rigidbody m_RigidBody;

    private float m_UpDownMaxVel = 15000.0f;
    private float m_UpDownDirection = 0.0f;

    public event BallIsInMyArea OnBallIsInMyArea = null;
    public event PlayerHasDetectBall OnPlayerHasDetectBall = null;
    public event PlayerHasDetectBallNextToHim OnPlayerHasDetectBallNextToHim = null;


    private void Awake()
    {
        /* Error checking and private members initialisation */
        if( m_MyBar == null )
        {
            Debug.LogError("MyBar not assigned in "+ name);
           
        }
        else
        {
            m_BarMeshRender = m_MyBar.GetComponent<MeshRenderer>();

            if (m_BarMeshRender == null)
            {
                Debug.LogError("MyBar in " + name + " has not MeshRender");
            }
            
        }

        m_RigidBody = GetComponent<Rigidbody>();

        m_Players = GetComponentsInChildren<FootballPlayer>();

        if(m_Players.Length == 0)
        {
            Debug.LogError("No player controlled by " + name);
        }
        else
        {
            for(int i=0; i<m_Players.Length; i++)
            {
                m_Players[i].BallLayer = m_BallLayer;
                m_Players[i].OnBallIsInFrontOfPlayer += PlayerHasDetectBall;
                m_Players[i].OnBallIsIsNextToThePlayer += PlayerHasDetectBallNextToHimHandler;
            }
        }

        m_Animator = GetComponent<Animator>();

    }




    private void FixedUpdate()
    {
        m_RigidBody.velocity = new Vector3(0, 0, m_UpDownDirection * m_UpDownMaxVel * Time.fixedDeltaTime);
    }

    public void DoKick( bool load )
    {
        m_Animator.SetBool("LoadKick", load);
    }


    /* Change bar color for visual selection */
    public void SelectPlayers()
    {
        m_BarMeshRender.material.color = m_ColorOnSelection;
    }

    /* Change bar color for visual deselection */
    public void DeselectPlayers()
    {
        m_BarMeshRender.material.color = Color.white;
        m_UpDownDirection = 0.0f;

        m_Animator.SetBool("LoadKick", false);

    }

    public float UpDownDirection
    {
        set
        {
            m_UpDownDirection = value;
        }
    }

    public int BarID
    {
        set { m_BarID = value; }
        get { return m_BarID; }
    }

    private void OnTriggerEnter(Collider other)
    {
        int mask = 1 << other.gameObject.layer;
        if ((mask & m_BallLayer.value) != 0)
        {
            if(OnBallIsInMyArea != null)
            {
                OnBallIsInMyArea(m_BarID, other.GetComponent<Ball>());
            }
        }
    }


    public void PlayerHasDetectBall()
    {
        if(OnPlayerHasDetectBall != null)
        {
            OnPlayerHasDetectBall(m_BarID);
        }
    }

    public void PlayerHasDetectBallNextToHimHandler(float i_Direction)
    {
        if(OnPlayerHasDetectBallNextToHim != null)
        {
            OnPlayerHasDetectBallNextToHim(m_BarID, i_Direction);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < m_Players.Length; i++)
        {
            m_Players[i].OnBallIsInFrontOfPlayer -= PlayerHasDetectBall;
            m_Players[i].OnBallIsIsNextToThePlayer -= PlayerHasDetectBallNextToHimHandler;
        }
    }

    public FootballPlayer[] PlayerList
    {
        get { return m_Players;  }
    }


    private void OnCollisionExit(Collision collision)
    {
        /*
        int mask = 1 << collision.gameObject.layer;
        if ((mask & m_BallLayer.value) != 0)
        {
            Ball b = collision.gameObject.GetComponent<Ball>();

            if (b.GetCurrentVelocity().magnitude < 10)
            {
                b.AddImpulse(b.GetCurrentVelocity().normalized * 10);
            }
        }*/
    }

}
