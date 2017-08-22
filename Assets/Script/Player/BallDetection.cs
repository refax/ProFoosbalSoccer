using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void BallIsNextToThePlayer( float i_Direction );


public class BallDetection : MonoBehaviour {

    [SerializeField]
    [Tooltip("Musti be 1 for Up direction and -1 for down direction")]
    float m_Direction = 1.0f;

    public event BallIsNextToThePlayer OnBallIsNextToThePlayer = null;

    private LayerMask m_BallLayer;

    private void OnTriggerEnter(Collider other)
    {
        int mask = 1 << other.gameObject.layer;
        if ((mask & m_BallLayer.value) != 0)
        {
            if (OnBallIsNextToThePlayer != null)
            {
                OnBallIsNextToThePlayer( m_Direction );
            }
        }
    }

    public LayerMask BallLayer
    {
        set { m_BallLayer = value; }
    }
}
