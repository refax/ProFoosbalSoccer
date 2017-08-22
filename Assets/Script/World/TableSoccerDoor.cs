using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ScoreGoal(int PlayerID);

public class TableSoccerDoor : MonoBehaviour {

    public event ScoreGoal OnGoalScored = null;
    [SerializeField]
    private LayerMask m_BallLayer;

    [SerializeField]
    [Tooltip("1 for Player One, 2 for Player Two")]
    private int m_PlayerID = 1;


    private void OnTriggerExit(Collider other)
    {
        int mask = 1 << other.gameObject.layer;
        if ((mask & m_BallLayer.value) != 0)
        {
            if (OnGoalScored != null)
            {
                OnGoalScored(m_PlayerID);
            }
        }

    }
}
