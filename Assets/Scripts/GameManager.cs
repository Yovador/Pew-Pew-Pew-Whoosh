using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void OnStrongTempo()
    {
        List<GameObject> players = new List<GameObject>( GameObject.FindGameObjectsWithTag("Player") );

        foreach (var player in players)
        {
            player.GetComponent<PlayerController>().Shoot();
        }
    }

    public void OnWeakTempo()
    {
        List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

        foreach (var player in players)
        {
            player.GetComponent<PlayerController>().ActivateDeath();
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
