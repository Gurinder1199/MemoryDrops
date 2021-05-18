using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestionControllerScript : MonoBehaviour
{
    public float speed=1.0f;
    public UnityAction<int> OnAnswerChosen;
    public UnityAction OnQuestionDestroyed;
    public int currentSequence;

    public Image questionImage;
    public Image answerImage1;
    public Image answerImage2;
    public Text questionText;
    public Text answerText1;
    public Text answerText2;
    public Button audioButton;

    public RectTransform parentRect;
    public RectTransform rect;

    public ParticleSystem rightParticleSystem;
    public ParticleSystem wrongParticleSystem;
    public GameObject UI_container;

    bool answerChosen;
    public void Start()
    {
        parentRect = transform.parent.gameObject.GetComponent<RectTransform>();
        rect=gameObject.GetComponent<RectTransform>();
        float sizeRatio = PlayerPrefs.GetFloat("QuestionSize")+1;
        rect.sizeDelta = new Vector2(100 * sizeRatio, 300 * sizeRatio);
        answerChosen = false;
        currentSequence = 0;
        Vector3 newPosition = new Vector3(Random.Range(-parentRect.rect.width / 2 + rect.rect.width / 2, parentRect.rect.width / 2 - rect.rect.width / 2), parentRect.rect.height/ 2 + rect.rect.height / 2, 0);
        transform.localPosition = newPosition;
        if (audioButton.gameObject.activeInHierarchy)
        {
            audioButton.GetComponent<AudioSource>().Play();
        }
    }

    public void Update()
    {
        if (currentSequence == 0) RunNormalSequence();
        else if (currentSequence == 1) RunWrongAnswerChosenSequence();
        else if (currentSequence == 2) RunRightAnswerChosenSequence();
    }

    public void AnswerChosen(int index)
    {
        if (answerChosen) return;
        answerChosen = true;
        OnAnswerChosen(index);
    }

    public void RunNormalSequence()
    {
        transform.Translate(0, -(speed * Time.deltaTime), 0);
        if (transform.localPosition.y <= (-parentRect.rect.height/2 + rect.rect.height/2))
        {
            AnswerChosen(0);
        }
    }

    public void RunWrongAnswerChosenSequence()
    {
        transform.Translate(0, -(speed * Time.deltaTime * 3), 0);
        if (transform.localPosition.y <= (-parentRect.rect.height / 2 + rect.rect.height / 2))
        {
            currentSequence = -1;
            UI_container.SetActive(false);
            wrongParticleSystem.gameObject.SetActive(true);
            StartCoroutine(delayedDestruction(wrongParticleSystem.main.duration));
        }
    }

    public void RunRightAnswerChosenSequence()
    {
        currentSequence = -1;
        UI_container.SetActive(false);
        rightParticleSystem.gameObject.SetActive(true);
        StartCoroutine(delayedDestruction(rightParticleSystem.main.duration));
    }

    public void CreateQuestion(string text)
    {
        questionText.gameObject.SetActive(true);
        questionText.text = text;
    }

    public void CreateQuestion(AudioClip audioClip)
    {
        audioButton.gameObject.SetActive(true);
        audioButton.GetComponent<AudioSource>().clip = audioClip;
    }

    public void CreateQuestion(Sprite image)
    {
        questionImage.color = new Color(1, 1, 1, 1);
        questionImage.sprite = image;
    }

    public void CreateAnswers(string text1,string text2)
    {
        answerText1.gameObject.SetActive(true);
        answerText2.gameObject.SetActive(true);
        answerText1.text = text1;
        answerText2.text = text2;
    }

    public void CreateAnswers(Sprite image1, Sprite image2)
    {
        answerImage1.color = new Color(1, 1, 1, 1);
        answerImage2.color = new Color(1, 1, 1, 1);
        answerImage1.sprite = image1;
        answerImage2.sprite = image2;
    }

    IEnumerator delayedDestruction(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnQuestionDestroyed();
        Destroy(gameObject);
    }

}
