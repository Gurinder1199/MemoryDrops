using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceRecorder : MonoBehaviour
{
    AudioClip currentClip;
    public AudioSource audioSource;
    public Image timerImage;
    public GameObject recordButton;
    public GameObject stopButton;
    public Button doneButton;
    public Button playButton;
    private int maxRecordingTime = 5;
    public int microphoneStartPlaytime;
    public float currentTime;
    bool isRecording;
    string recordingDevice;
    public delegate void OnCompleteListener(AudioClip audioClip);
    OnCompleteListener onCompleteListener;

    private static VoiceRecorder m_instance = null;
    public static VoiceRecorder Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = Instantiate(Resources.Load<VoiceRecorder>("VoiceRecorder"));

            return m_instance;
        }
    }


    public void Awake()
    {
        isRecording = false;
        recordingDevice = Microphone.devices[0];
    }

    public void Update()
    {
        if (isRecording)
        {
            currentTime = currentTime + Time.deltaTime;
            if (currentTime >= maxRecordingTime)
            {
                stopRecording();
            }
            float percentFilled = currentTime / maxRecordingTime;
            timerImage.fillAmount = (percentFilled <= 1) ? percentFilled : 1;
        }
    }

    public void getRecording(OnCompleteListener onCompleteListener,int maxRecordingTime=20)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        resetAllParameters();
        this.onCompleteListener = onCompleteListener;
        this.maxRecordingTime = maxRecordingTime;
    }

    public void onCancelButtonPressed()
    {
        beforeExitRecorder();
        if (onCompleteListener != null) onCompleteListener(null);
    }

    public void onDoneButtonPressed()
    {
        beforeExitRecorder();
        if(onCompleteListener!=null)onCompleteListener(currentClip);
    }

    private void resetAllParameters()
    {
        playButton.interactable = false;
        microphoneStartPlaytime = 0;
        currentTime = 0;
        isRecording = false;
        onCompleteListener = null;
        doneButton.interactable=false;
        recordButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
        timerImage.fillAmount = 1;
        audioSource.clip = null;
    }

    private void beforeExitRecorder()
    {
        stopRecording();
        gameObject.SetActive(false);
        doneButton.interactable = false;
        playButton.interactable = false;
        if (audioSource.isPlaying) audioSource.Stop();
    }

    public void startRecording()
    {
        if (Microphone.IsRecording(recordingDevice))
        {
            return;
        }
        if (audioSource.isPlaying) audioSource.Stop();
        recordButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
        microphoneStartPlaytime = maxRecordingTime + 3;
        doneButton.interactable = false;
        playButton.interactable = false;
        timerImage.fillAmount = 0;
        currentTime = 0;
        currentClip = Microphone.Start(Microphone.devices[0], false, microphoneStartPlaytime, 44100);
        isRecording = true;
    }

    public void stopRecording()
    {
        if (!Microphone.IsRecording(recordingDevice))
        {
            return;
        }
        recordButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
        timerImage.fillAmount = 1;
        doneButton.interactable = true;
        playButton.interactable = true;
        Microphone.End(Microphone.devices[0]);
        isRecording = false;
    }
    public void playRecording()
    {
        audioSource.clip = currentClip;
        audioSource.Play();
    }
}
