using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//modified form CubeSound. Checks beats adding a bpm limit to checks.

public class BPMtimer : MonoBehaviour {
    //bpm limiter
    public int BPMLimiter;

    public SubbandBeatDetection beatProcessor;
    public int[] subbandsToEar;

    //event
    public delegate void OnLimitedBeatHandler();
    public event OnLimitedBeatHandler OnBPMBeat;


    [SerializeField]
    private float limittimer, timer;

    private void Start()
    {
        for (int i = 0; i < subbandsToEar.Length; i++)
        {
            beatProcessor.subBands[subbandsToEar[i]].OnBeat += OnBeat;
        }
        limittimer = 60 / (float)BPMLimiter;
        timer = limittimer;
    }

    // Update is called once per frame
    void Update () {
        
        timer += Time.deltaTime;
	}

    void OnBeat()
    {
        if (timer >= limittimer)
        {
            if (OnBPMBeat != null)
                OnBPMBeat();
            timer = 0;
        }
    }
}
