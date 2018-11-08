using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour {
    public BPMtimer selectedBeatDetection;

    //enemy spawn information
    public GameObject enemyPrefab;
    public EnemyDirection spawndirection;
    public Color[] possiblecolors;
    //the number of beats it takes, on average, to spawn 1 enemy
    public int AverageBeatsToSpawn;    
    private int originalbeatsperSpawn;
    public ScriptableInt score;


    //How close player can get to spawnpoint before it stops working
    public float playerlimitproximity;
    [SerializeField]
    bool active;
    public GameObject player;

	// Use this for initialization
	void Start () {
        selectedBeatDetection.OnBPMBeat += OnBeat;
        active = false;
        originalbeatsperSpawn = AverageBeatsToSpawn;
    }
	
	// Update is called once per frame
	void Update () {
        if (player != null)
        {
            if (Vector3.Distance(player.transform.position, this.transform.position) <= playerlimitproximity)
            {
                active = false;
            }
            else
            {
                active = true;
            }
        }
        //increasing difficulty
        if (AverageBeatsToSpawn > 2)
        {
            int dificultyincrease = originalbeatsperSpawn - score.value / (50 * ((originalbeatsperSpawn+1) - AverageBeatsToSpawn));
            if(dificultyincrease < AverageBeatsToSpawn)
            {
                AverageBeatsToSpawn = dificultyincrease;
            }
        }
	}

    void OnBeat()
    {
        if (active)
        {
            int randnumber = (Random.Range(0,AverageBeatsToSpawn));
            
            if (randnumber == 0)
            {
                GameObject newenemy = Instantiate(enemyPrefab,this.transform.position,Quaternion.identity);
                newenemy.GetComponent<EnemyBeatMove>().selectedBeatDetection = selectedBeatDetection;
                newenemy.GetComponent<ColorBeatAlternation>().selectedBeatDetection = selectedBeatDetection;

                newenemy.GetComponent<EnemyBeatMove>().mydirection = spawndirection;

                randnumber = Random.Range(0, possiblecolors.Length);

                newenemy.GetComponent<ColorBeatAlternation>().beatcolors[0] = possiblecolors[randnumber];
            }
        }
    }
}
