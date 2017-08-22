using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarScript : MonoBehaviour {

    public float vel = 200.0f;
    public float rotVel = 500000.0f;
    public float maxAngularVel = 30.0f;
    public GameObject[] players;
    // Use this for initialization

    private float m_VerticalInput = 0.0f;
    private float m_HorizontalInput = 0.0f;

    private float v = 0;
    private float h = 0;


    void Start () {

        GetComponent<Rigidbody>().maxAngularVelocity = maxAngularVel;
    }
	
	// Update is called once per frame
	void Update () {




    }


    private void FixedUpdate()
    {

        v = Input.GetAxis("Vertical") * vel * Time.fixedDeltaTime;
        h = Input.GetAxis("Horizontal") * rotVel * players.Length * Time.fixedDeltaTime;
        GetComponent<Rigidbody>().maxAngularVelocity = maxAngularVel;

        if( m_VerticalInput >= 0 && v <= 0 )
        {
            m_VerticalInput = v;
            v = 0.0f;
            
        }

        if( m_HorizontalInput >= 0 && h <= 0 )
        {
            m_HorizontalInput = h;
            h = 0.0f;
        }
  
        
        //this.transform.position = this.transform.position + new Vector3(0, 0, v);
        //if (Mathf.Abs(v) < 0.1)
        {
        //    GetComponent<Rigidbody>().velocity = Vector3.zero;

        }
      //  else
        {
            //GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, v), ForceMode.VelocityChange);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, v * vel);
        }


        if (Mathf.Abs(h) < 0.1)
        {
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            for(int i=0; i<players.Length; i++)
            {
                players[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }

        }
        else
        {

            GetComponent<Rigidbody>().AddTorque(new Vector3(0, 0, h), ForceMode.VelocityChange);

        }

    
       
    }
}
