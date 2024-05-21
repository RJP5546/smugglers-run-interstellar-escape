using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private GameManager gm;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreTextEnd;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [SerializeField] private float scoreVal;
    private float speed;
    private int scoreAsInt;
    private float highScore;
    private float originalCamFOV;

    private void Start()
    {
        scoreVal = 0;
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = "High Score: " + highScore;
        originalCamFOV = gameCamera.fieldOfView;
        
    }

    void FixedUpdate()
    {
        //get speed value from gm
        speed = gm.getCurrSpeed();

        //calculate score increase and cast to int
        scoreVal += speed;
        scoreAsInt = (int)scoreVal;

        //display scoreAsInt in UI
        scoreText.text = scoreAsInt.ToString();
        scoreTextEnd.text = "Score: " + scoreAsInt.ToString();
        
        if (scoreVal > highScore)
        {
            PlayerPrefs.SetInt("HighScore", scoreAsInt);
            highScoreText.text = "High Score: " + scoreAsInt;
        }
    }

    public IEnumerator CameraFOVIncrease()
    {
        float timeElapsed = 0;

        while (timeElapsed < 3f)
        {
            gameCamera.fieldOfView = Mathf.Lerp(originalCamFOV, 170, timeElapsed / 3f);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        gameCamera.fieldOfView = 170;
    }

    public IEnumerator CameraFOVDecrease()
    {
        float timeElapsed = 0;

        while (timeElapsed < 3f)
        {
            gameCamera.fieldOfView = Mathf.Lerp(170, originalCamFOV, timeElapsed / 3f);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        gameCamera.fieldOfView = originalCamFOV;
    }
}
