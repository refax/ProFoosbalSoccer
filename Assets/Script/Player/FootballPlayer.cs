using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void BallIsInFrontOfPlayer();
public delegate void BallIsIsNextToThePlayer(float m_Direction);

public class FootballPlayer : MonoBehaviour {

    public event BallIsInFrontOfPlayer OnBallIsInFrontOfPlayer = null;
    public event BallIsIsNextToThePlayer OnBallIsIsNextToThePlayer = null;

    private LayerMask m_BallLayer;
    private BallDetection[] m_EdgeBallDetection;

    private void Awake()
    {
        /*
        m_EdgeBallDetection = GetComponentsInChildren<BallDetection>();   
        for(int i=0; i<m_EdgeBallDetection.Length; i++)
        {
            m_EdgeBallDetection[i].OnBallIsNextToThePlayer += OnBallIsNextToThePlayerHandler;
        }
        */
    }

    /* Signal redirection */
    private void OnBallIsNextToThePlayerHandler(float i_Direction)
    {
        if (OnBallIsIsNextToThePlayer != null)
        {
            OnBallIsIsNextToThePlayer(i_Direction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int mask = 1 << other.gameObject.layer;
        if ((mask & m_BallLayer.value) != 0)
        {   
            if (OnBallIsInFrontOfPlayer != null)
            {
                OnBallIsInFrontOfPlayer();
            }
        }
    }

    public LayerMask BallLayer
    {
        set
        {
            m_BallLayer = value;
            /* Update Layer to all child */
            /*
            for(int i=0; i< m_EdgeBallDetection.Length; i++)
            {
                m_EdgeBallDetection[i].BallLayer = m_BallLayer;
            }
            */
        }
    }

    private void OnDestroy()
    {
        /*
        for (int i = 0; i < m_EdgeBallDetection.Length; i++)
        {
            m_EdgeBallDetection[i].OnBallIsNextToThePlayer -= OnBallIsNextToThePlayerHandler;
        }
        */
    }

    private void OnCollisionExit(Collision collision)
    {
        Ball b = collision.gameObject.GetComponent<Ball>();

        if(b!=null)
        {
            Debug.Log("AAA");
          //  b.SetMaxSpeed();
        }
    }

}
