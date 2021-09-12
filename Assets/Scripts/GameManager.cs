using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameStarted = false;
    private List<int> score = new List<int>();

    private void Awake()
    {
        Debug.LogWarning("GAMEMANAGERAWAKE");
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void StartGame()
    {
        GameObject.Find("PRESSTOSTART").SetActive(false);
        gameStarted = true;




    }

    public void OnStrongTempo()
    {
        if (gameStarted)
        {
            List<GameObject> players = new List<GameObject>( GameObject.FindGameObjectsWithTag("Player") );

            foreach (var player in players)
            {
                player.GetComponent<PlayerController>().Shoot();
            }
        }
    }



    public void OnWeakTempo()
    {
        if (gameStarted)
        {
            int currentBeat = AudioEngine.instance.currentBPMcount % AudioEngine.instance.signature;
            Debug.Log("Beat number : " + currentBeat);
            List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

            foreach (var player in players)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();

                if(playerController.beatToDie != -1)
                {

                    Debug.LogWarning("Dying on beat : " + playerController.beatToDie);

                    switch (currentBeat)
                    {
                        case 0:

                            if(playerController.beatToDie == currentBeat)
                            {
                                GameOver(playerController);
                            }

                            break;
                        case 1:

                            if (playerController.beatToDie == currentBeat)
                            {
                                GameOver(playerController);
                            }

                            break;
                        case 2:

                            if (playerController.beatToDie == currentBeat)
                            {
                                GameOver(playerController);
                            }

                            break;
                        case 3:

                            if (playerController.beatToDie == currentBeat)
                            {
                                GameOver(playerController);
                            }

                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    void UpdateScoreDisplay(int playerNumber, int score)
    {

        GameObject.Find("PLAYER" + playerNumber + "SCORE").GetComponent<Text>().text = score.ToString();

    }

    private void ChangeScore(PlayerController playerController)
    {
        List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        foreach (var player in players)
        {
            PlayerController currentPlayerController= player.GetComponent<PlayerController>();
            if(currentPlayerController != playerController)
            {

                score[currentPlayerController.playerNumber-1]++;
                UpdateScoreDisplay(playerController.playerNumber, score[currentPlayerController.playerNumber-1]);
                Debug.LogWarning("IN ADD SCORE : " + score[0] + " / " + score[1]);
            }
        }

    }

    public void GameOver(PlayerController playerController)
    {
        gameStarted = false;
        ChangeScore(playerController);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        if (score.Count == 0)
        {
            Debug.LogWarning("NO SCORE");
            foreach (var player in players)
            {
                score.Add(0);
            }
        }

        foreach (var player in players)
        {
     
            UpdateScoreDisplay(player.GetComponent<PlayerController>().playerNumber, score[player.GetComponent<PlayerController>().playerNumber - 1]);
        }
   
    }
}
