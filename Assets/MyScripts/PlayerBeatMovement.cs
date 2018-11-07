using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeatMovement : MonoBehaviour {
    public int life;
    public BPMtimer selectedBeatDetection;
    public KeyCode left, right;
    public bool leftinput, rightinput,attacking;
    public GameObject leftsensor, rightsensor;
    //Reset bools from previous action after some loops, to properly let enemies see them in time 
    private float resetpreviousaction;


    private delegate void Movement();
    Movement Moveonbeat;

	// Use this for initialization
	void Start () {
        selectedBeatDetection.OnBPMBeat += OnBeat;
        life = 3;
        resetpreviousaction = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(left))
        {
            Moveonbeat = moveLeft;
            if (leftsensor.GetComponent<TriggerSensor>().isInside)
            {
                Moveonbeat += leftAttack;
                attacking = true;
            }
            leftinput = true;
            rightinput = false;
        }
        else if (Input.GetKeyDown(right))
        {
            Moveonbeat = moveRight;
            if (rightsensor.GetComponent<TriggerSensor>().isInside)
            {
                Moveonbeat += rightAttack;
                attacking = true;
            }
            leftinput = false;
            rightinput = true;
        }

        if(life <= 0)
        {
            Destroy(this.gameObject);
        }

        if(resetpreviousaction > 0)
        {
            resetpreviousaction--;
            if(resetpreviousaction <= 0)
            {
                leftinput = false;
                rightinput = false;
                attacking = false;
            }
        }
    }


    void moveRight()
    {
        this.transform.Translate(1, 0, 0);        
    }

    void moveLeft()
    {
        this.transform.Translate(-1, 0, 0);
    }

    void rightAttack()
    {
        Destroy(rightsensor.GetComponent<TriggerSensor>().colidedObject);
    }

    void leftAttack()
    {
        Destroy(leftsensor.GetComponent<TriggerSensor>().colidedObject);
    }

    void OnBeat()
    {
        if(Moveonbeat != null)
            Moveonbeat();
        Moveonbeat = null;
        resetpreviousaction = 3;
    }
}
