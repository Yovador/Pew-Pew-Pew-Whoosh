using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool gameStarted = false;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartGame()
    {
        GameObject.Find("Canvas").SetActive(false);
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
                                GameOver();
                            }

                            break;
                        case 1:

                            if (playerController.beatToDie == currentBeat)
                            {
                                GameOver();
                            }

                            break;
                        case 2:

                            if (playerController.beatToDie == currentBeat)
                            {
                                GameOver();
                            }

                            break;
                        case 3:

                            if (playerController.beatToDie == currentBeat)
                            {
                                GameOver();
                            }

                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public void GameOver()
    {
        gameStarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
