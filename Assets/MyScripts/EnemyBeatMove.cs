using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyDirection
{
    right,left
};


public class EnemyBeatMove : MonoBehaviour {
    private delegate void Movement();
    Movement Moveonbeat;
    public bool moving;
    public EnemyDirection mydirection;
    public BPMtimer selectedBeatDetection;
    public GameObject leftsensor, rightsensor, leftoutersensor, rightoutersensor;
    //Reset bools from previous action after some loops, to properly let enemies see them in time 
    private float resetpreviousaction;

    //Different Sprites    
    public Sprite[] movingsprites;
    public Sprite attackingSP;
    public Sprite[] dyingSP;

    public Color deathColor;
    public int dead;

    // Use this for initialization
    void Start () {
        selectedBeatDetection.OnBPMBeat += OnBeat;
        resetpreviousaction = 0;
        dead = 0;
    }

    // Update is called once per frame
    void Update() {
        if(dead <= 0)
        { 
            if (mydirection == EnemyDirection.left)
            {
                if (leftsensor.GetComponent<TriggerSensor>().isInside)
                {
                    if (!leftsensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<PlayerBeatMovement>().attacking
                     && !leftsensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<PlayerBeatMovement>().leftinput)
                    {
                        Moveonbeat = leftAttack;
                    }
                    else if (leftsensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<PlayerBeatMovement>().leftinput)
                    {
                        Moveonbeat = moveLeft;
                        moving = true;
                    }
                    else
                    {
                        Moveonbeat = null;
                    }
                }
                else
                {
                    if (leftoutersensor.GetComponent<TriggerSensor>().isInside &&
                        !leftoutersensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<EnemyBeatMove>().moving)
                    {
                        Moveonbeat = null;
                    }
                    else
                    {
                        Moveonbeat = moveLeft;
                        moving = true;
                    }
                }
            }
            else if (mydirection == EnemyDirection.right)
            {
                if (rightsensor.GetComponent<TriggerSensor>().isInside)
                {
                    if (!rightsensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<PlayerBeatMovement>().attacking
                     && !rightsensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<PlayerBeatMovement>().rightinput)
                    {
                        Moveonbeat = rightAttack;
                    }
                    else if (rightsensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<PlayerBeatMovement>().rightinput)
                    {
                        Moveonbeat = moveRight;
                        moving = true;
                    }
                    else
                    {
                        Moveonbeat = null;
                    }
                }
                else
                {
                    if (rightoutersensor.GetComponent<TriggerSensor>().isInside &&
                        !rightoutersensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<EnemyBeatMove>().moving)
                    {
                        Moveonbeat = null;
                    }
                    else
                    {
                        Moveonbeat = moveRight;
                        moving = true;
                    }
                }
            }
        }

        if (resetpreviousaction > 0)
        {
            resetpreviousaction--;
            if (resetpreviousaction <= 0)
            {
                moving = false;
                if(dead >= 3)
                {
                    Moveonbeat = null;
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void Die()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        if(mydirection == EnemyDirection.right)
        {
            this.transform.Translate(-1.5f, 1, 0);
        }
        else
        {
            this.transform.Translate(1.5f, 1, 0);
        }
        int randnumber = Random.Range(0, dyingSP.Length);
        this.GetComponent<SpriteRenderer>().sprite = dyingSP[randnumber];
        this.GetComponent<SpriteRenderer>().color = deathColor;
        dead = 1;
    }

    void moveRight()
    {
        this.transform.Translate(1, 0, 0);
        int randnumber = Random.Range(0, movingsprites.Length);
        this.GetComponent<SpriteRenderer>().sprite = movingsprites[randnumber];
    }

    void moveLeft()
    {
        this.transform.Translate(-1, 0, 0);
        int randnumber = Random.Range(0, movingsprites.Length);
        this.GetComponent<SpriteRenderer>().sprite = movingsprites[randnumber];
        this.GetComponent<SpriteRenderer>().flipX = true;
    }

    void rightAttack()
    {
        rightsensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<PlayerBeatMovement>().TakeDamage();
        this.GetComponent<SpriteRenderer>().sprite = attackingSP;
    }

    void leftAttack()
    {
        leftsensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<PlayerBeatMovement>().TakeDamage();
        this.GetComponent<SpriteRenderer>().sprite = attackingSP;
    }

    void OnBeat()
    {
        if (Moveonbeat != null)
            Moveonbeat();
        Moveonbeat = null;
        resetpreviousaction = 3;
        if(dead > 0 )
        {
            dead++;
        }
        
    }
}
