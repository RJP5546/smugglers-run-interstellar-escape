using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private GameManager gm;
    [SerializeField] private GameObject WarpTunnel;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreTextEnd;
    [SerializeField] private TextMeshProUGUI highScoreText;

    [SerializeField] private float scoreVal;
    private float speed;
    private int scoreAsInt;
    private float highScore;
    private float originalCamFOV;

    private Material WarpTunnelMaterial;
    private Color WarpTunnelAlpha;

    private void Start()
    {
        scoreVal = 0;
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreText.text = "High Score: " + highScore;
        originalCamFOV = gameCamera.fieldOfView;

        WarpTunnelMaterial = WarpTunnel.GetComponent<Renderer>().material;
        WarpTunnelAlpha = WarpTunnelMaterial.color;
        WarpTunnelAlpha.a = 0f;
        WarpTunnelMaterial.color = WarpTunnelAlpha;

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
            gameCamera.fieldOfView = Mathf.Lerp(originalCamFOV, 168, timeElapsed / 3f);
            WarpTunnelAlpha.a = Mathf.Lerp(0, 1, timeElapsed / 3f);
            WarpTunnelMaterial.color = WarpTunnelAlpha;
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
            gameCamera.fieldOfView = Mathf.Lerp(168, originalCamFOV, timeElapsed / 3f);
            WarpTunnelAlpha.a = Mathf.Lerp(1, 0, timeElapsed / 3f);
            WarpTunnelMaterial.color = WarpTunnelAlpha;
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        gameCamera.fieldOfView = originalCamFOV;
    }
}
