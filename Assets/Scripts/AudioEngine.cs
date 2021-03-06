using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioEngine : MonoBehaviour
{
    public static AudioEngine instance = null;
    [SerializeField]
    private AudioSource musicStream;
    [SerializeField]
    private AudioSource soundStream;

    [SerializeField]
    private int bpm = 70;
    [SerializeField]
    public int signature = 4;
    private int currentMesure = 0;

    public UnityEvent weakTempoEvent;
    public UnityEvent strongTempoEvent;

    public static float timeBetweenTwoBeat;

    public int currentBPMcount = 0;
    float nextBPMTimeCode ;

    void Awake()
    {
        Application.targetFrameRate = 60;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        nextBPMTimeCode = 0;
        timeBetweenTwoBeat = 60f / bpm;


    }

    private void Start()
    {
        musicStream.Play();
        soundStream.Play();
    }

    private void Update()
    {

        if (musicStream.time > nextBPMTimeCode)
        {

            nextBPMTimeCode += timeBetweenTwoBeat;


            weakTempoEvent.Invoke();

            if (currentBPMcount % signature == 0)
                {
                    currentMesure++;
                    if (currentMesure % 2 == 0)
                    {
                        strongTempoEvent.Invoke();
                    }
                }
                currentBPMcount++;

        }

        if(nextBPMTimeCode > musicStream.clip.length)
        {
            nextBPMTimeCode = timeBetweenTwoBeat;
        }
    }

    public void PlaySound(AudioClip soundClipToPlay)
    {
        soundStream.clip = soundClipToPlay;
        soundStream.Play();
    }
}
