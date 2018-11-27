using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    TextMeshProUGUI maxScoreText;
    [SerializeField] public GameObject Player;
    public int playerScore;
    public int maxScore;
    Scene curScene;

    void Start()
    {
        curScene = SceneManager.GetActiveScene();
        //Show score
        if(curScene.buildIndex == 1)
        {
            scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        }
        else if(curScene.buildIndex == 2)
        {
            scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
            maxScoreText = GameObject.Find("High Score Text").GetComponent<TextMeshProUGUI>();
            playerScore = PlayerPrefs.GetInt("Score");
            maxScore = PlayerPrefs.GetInt("High Score");
            scoreText.SetText(playerScore.ToString());
            maxScoreText.SetText(maxScore.ToString());
            CompareScore();
            
        }
    }
    //TODO: maxscore not saving

	// Update is called once per frame
	void Update ()
    {

        //TODO: change to update only when value is changed
        if(curScene.buildIndex == 1)
        {
            scoreText.SetText(playerScore.ToString());
        }

    }

    public void GameOver()
    {
        PlayerPrefs.SetInt("Score",playerScore);
        SceneManager.LoadScene(2);
    }

    void CompareScore()
    {
        if(playerScore >= maxScore)
        {
            PlayerPrefs.SetInt("High Score",playerScore);
            maxScore = playerScore;
            maxScoreText.SetText(playerScore.ToString());
            Debug.Log("new high scroe");
        }
    }
}
