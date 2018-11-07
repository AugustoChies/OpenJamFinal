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


    //How close player can get to spawnpoint before it stops working
    public float playerlimitproximity;
    [SerializeField]
    bool active;
    public GameObject player;

	// Use this for initialization
	void Start () {
        selectedBeatDetection.OnBPMBeat += OnBeat;
        active = false;
    }
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(player.transform.position,this.transform.position) <= playerlimitproximity)
        {
            active = false;
        }
        else
        {
            active = true;
        }
	}

    void OnBeat()
    {
        if (active)
        {
            int randnumber = (int)(Random.Range(0,int.MaxValue) % AverageBeatsToSpawn);

            if (randnumber == 0)
            {
                GameObject newenemy = Instantiate(enemyPrefab,this.transform.position,Quaternion.identity);
                newenemy.GetComponent<EnemyBeatMove>().selectedBeatDetection = selectedBeatDetection;
                newenemy.GetComponent<ColorBeatAlternation>().selectedBeatDetection = selectedBeatDetection;

                newenemy.GetComponent<EnemyBeatMove>().mydirection = spawndirection;

                randnumber = Mathf.Clamp(Random.Range(0, possiblecolors.Length), 0, possiblecolors.Length);

                newenemy.GetComponent<ColorBeatAlternation>().beatcolors[0] = possiblecolors[randnumber];
            }
        }
    }
}
