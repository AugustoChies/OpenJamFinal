using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeatMovement : MonoBehaviour {
    public int life;   
    public BPMtimer selectedBeatDetection;
    public KeyCode left, right;
    public bool leftinput, rightinput, attacking;
    public ScriptableBool dead;
    public GameObject leftsensor, rightsensor;
    //Reset bools from previous action after some loops, to properly let enemies see them in time 
    private float resetpreviousaction;
    //value leaving player vulnerable for 1 beat when he misses an attack   
    public int fumble;
    public Color fumbleColor;
    private Color previousColorMemory;

    public GameObject lifebar1, lifebar2, lifebar3;

    public Sprite standingSp, damageSp, MissSP;
    public Sprite[] attackingSp;

    public AudioClip[] hitsounds;
    public AudioClip hurtsound;

    public ScriptableInt score;
    public ScriptableInt comboMultiplier;


    private delegate void Movement();
    Movement Moveonbeat;

	// Use this for initialization
	void Start () {
        selectedBeatDetection.OnBPMBeat += OnBeat;
        life = 3;
        resetpreviousaction = 0;
        fumble = 0;
        score.value = 0;
        comboMultiplier.value = 1;
        dead.value = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (fumble <= 0 && dead.value == false)
        {
            if (Input.GetKeyDown(left))
            {
                if (leftsensor.GetComponent<TriggerSensor>().isInside)
                {
                    Moveonbeat = moveLeft;
                    Moveonbeat += leftAttack;
                    attacking = true;
                }
                else
                {
                    previousColorMemory = this.gameObject.GetComponent<ColorBeatAlternation>().beatcolors[0];
                    this.gameObject.GetComponent<ColorBeatAlternation>().beatcolors[0] = fumbleColor;
                    this.GetComponent<SpriteRenderer>().sprite = MissSP;
                    fumble = 2;
                }
                leftinput = true;
                rightinput = false;
            }
            else if (Input.GetKeyDown(right))
            {
                if (rightsensor.GetComponent<TriggerSensor>().isInside)
                {
                    Moveonbeat = moveRight;
                    Moveonbeat += rightAttack;
                    attacking = true;
                }
                else
                {
                    previousColorMemory = this.gameObject.GetComponent<ColorBeatAlternation>().beatcolors[0];
                    this.gameObject.GetComponent<ColorBeatAlternation>().beatcolors[0] = fumbleColor;
                    this.GetComponent<SpriteRenderer>().sprite = MissSP;
                    fumble = 2;
                }
                leftinput = false;
                rightinput = true;
            }
        }

        lifebar1.SetActive(life > 0);
        lifebar2.SetActive(life > 1);
        lifebar3.SetActive(life > 2);

        if (life <= 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            dead.value = true;
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
        rightsensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<EnemyBeatMove>().Die();
        int randnumber = Random.Range(0, attackingSp.Length);
        this.GetComponent<SpriteRenderer>().sprite = attackingSp[randnumber];
        this.GetComponent<SpriteRenderer>().flipX = false;

        score.value += 10 * comboMultiplier.value;
        comboMultiplier.value++;

        randnumber = Random.Range(0, hitsounds.Length);
        this.gameObject.GetComponent<AudioSource>().clip = hitsounds[randnumber];
        this.gameObject.GetComponent<AudioSource>().Play();
    }

    void leftAttack()
    {
        leftsensor.GetComponent<TriggerSensor>().colidedObject.GetComponent<EnemyBeatMove>().Die();
        int randnumber = Random.Range(0, attackingSp.Length);
        this.GetComponent<SpriteRenderer>().sprite = attackingSp[randnumber];
        this.GetComponent<SpriteRenderer>().flipX = true;

        score.value += 10 * comboMultiplier.value;
        comboMultiplier.value++;

        randnumber = Random.Range(0, hitsounds.Length);
        this.gameObject.GetComponent<AudioSource>().clip = hitsounds[randnumber];
        this.gameObject.GetComponent<AudioSource>().Play();
    }

    public void TakeDamage()
    {
        life--;
        this.gameObject.GetComponent<AudioSource>().clip = hurtsound;
        this.gameObject.GetComponent<AudioSource>().Play();
        this.gameObject.GetComponent<SpriteRenderer>().sprite = damageSp;
    }

    void OnBeat()
    {
        if(Moveonbeat != null)
            Moveonbeat();
        else if(this.gameObject != null && Moveonbeat == null && fumble <= 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = standingSp;
            comboMultiplier.value = 1;
        }
        Moveonbeat = null;
        resetpreviousaction = 3;
        fumble--;

        if(fumble == 0)
        {           
            this.gameObject.GetComponent<ColorBeatAlternation>().beatcolors[0] = previousColorMemory;
        }
    }
}
