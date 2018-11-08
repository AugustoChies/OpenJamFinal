using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour {
    public Text scoreText, comboText, finalscoreTxt;
    public GameObject gameOver;
    public ScriptableBool dead;
    public ScriptableInt score, combo;
	// Use this for initialization
	void Start () {
        dead.value = false;
	}
	
	// Update is called once per frame
	void Update () {
        scoreText.text = "" + score.value;
        finalscoreTxt.text = "Final Score: " + score.value;
        if (combo.value > 2)
        {
            comboText.enabled = true;
            comboText.text = "x" + (combo.value - 1);
        }
        else
        {
            comboText.enabled = false;
        }

        if(dead.value)
        {
            scoreText.enabled = false;
            comboText.enabled = false;
            gameOver.SetActive(true);
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SceneManager.LoadScene("SampleScene");
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SceneManager.LoadScene("Menu");
            }
        }
	}
}
