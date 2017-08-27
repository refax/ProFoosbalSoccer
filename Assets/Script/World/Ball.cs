using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour {

    public float m_MaxVel = 1.0f;

    private Rigidbody m_rb;

    private void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        //Physics hotfix
 
        if(m_rb.velocity.sqrMagnitude > m_MaxVel * m_MaxVel)
        {
            m_rb.velocity = m_rb.velocity.normalized * m_MaxVel;
        }

        
        
        if (m_rb.velocity.sqrMagnitude < 10 * 10)
        {
            m_rb.velocity = m_rb.velocity.normalized * 10;
        }
        


    }

    public void SetVelocity(Vector3 i_Velocity)
    {
        m_rb.velocity = i_Velocity;
    }

    public void SetPosition(Vector3 i_Position)
    {
        m_rb.transform.position = i_Position;
    }

    


    public void SetMaxSpeed()
    {
        m_rb.velocity = m_rb.velocity.normalized * m_MaxVel;
    }

    public void AddImpulse (Vector3 i_Impulse)
    {
        m_rb.AddForce(i_Impulse, ForceMode.Force);
    }

    public Vector3 GetCurrentVelocity()
    {
        return m_rb.velocity;
    }
    
   
    public Rigidbody GetRigidBody()
    {
        return m_rb;
    }




}
