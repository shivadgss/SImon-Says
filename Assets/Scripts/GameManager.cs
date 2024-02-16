using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public SpriteRenderer[] colors;
    private int colorSelect;
    public float stayLit;
    private float stayitCounter;
    public float waitBetweenLight;
    private float waitBetweenLightTimer;
    private bool shouldBeLit;
    private bool shouldBeDark;
    public List<int> activeSequence;
    private int positionInSequence;
    private bool gameActive;
    private int inputInSequence;
    public AudioSource correct;
    public AudioSource incorrect;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("HiScore"))
        {
            PlayerPrefs.SetInt("HiScore", 0);
        }
        scoreText.text = "Score : 0 -  HighScore : " + PlayerPrefs.GetInt("HiScore");   
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldBeLit)
        {
            stayitCounter-=Time.deltaTime;
            if(stayitCounter < 0 ) {
                colors[activeSequence[positionInSequence]].color = new Color(colors[activeSequence[positionInSequence]].color.r, colors[activeSequence[positionInSequence]].color.g, colors[activeSequence[positionInSequence]].color.b, 0.5f);
                shouldBeLit = false;
                shouldBeDark = true;
                waitBetweenLightTimer = waitBetweenLight;
                positionInSequence++;
            }
        }
        if(shouldBeDark)
        {
            waitBetweenLightTimer -= Time.deltaTime;
            if (positionInSequence >= activeSequence.Count)
            {
                shouldBeDark=false;
                gameActive = true;
            }
            else
            {
                if(waitBetweenLightTimer < 0)
                {
                    colors[activeSequence[positionInSequence]].color = new Color(colors[activeSequence[positionInSequence]].color.r, colors[activeSequence[positionInSequence]].color.g, colors[activeSequence[positionInSequence]].color.b, 1f);
                    stayitCounter = stayLit;
                    shouldBeLit = true;
                    shouldBeDark = false;
                }
            }
        }
    }
    public void StartGame()
    {
        activeSequence.Clear();
        positionInSequence = 0;
        inputInSequence = 0;
        colorSelect = Random.Range(0, colors.Length);
        activeSequence.Add(colorSelect);
        colors[activeSequence[positionInSequence]].color = new Color(colors[activeSequence[positionInSequence]].color.r, colors[activeSequence[positionInSequence]].color.g, colors[activeSequence[positionInSequence]].color.b, 1f);
        stayitCounter = stayLit;
        shouldBeLit=true;
        scoreText.text = "Score : 0 -  HighScore : " + PlayerPrefs.GetInt("HiScore");



    }
    public void ColorPressed(int whichButton)
    {
        if (gameActive){
            if (activeSequence[inputInSequence] == whichButton)
            {
                Debug.Log("Correct");
                inputInSequence++;
                if(inputInSequence >= activeSequence.Count) 
                {
                    if (activeSequence.Count > PlayerPrefs.GetInt("HiScore")){
                        PlayerPrefs.SetInt("HiScore", activeSequence.Count);
                    }
                    scoreText.text = "Score : " + activeSequence.Count+ "- High Score : "+PlayerPrefs.GetInt("HiScore");
                    positionInSequence = 0;
                    inputInSequence = 0;
                    colorSelect = Random.Range(0, colors.Length);
                    activeSequence.Add(colorSelect);
                    colors[activeSequence[positionInSequence]].color = new Color(colors[activeSequence[positionInSequence]].color.r, colors[activeSequence[positionInSequence]].color.g, colors[activeSequence[positionInSequence]].color.b, 1f);
                    stayitCounter = stayLit;
                    shouldBeLit = true;
                    gameActive = false;
                    correct.Play();
                    
                }
            }
            else
            {
                Debug.Log("Incorrect");
                incorrect.Play();
                gameActive = false;
            }
        }

    }
}
