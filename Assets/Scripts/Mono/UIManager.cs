using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable()]
public struct UIManagerParameters
{
    [Header("Answers Options")]
    [SerializeField] float margins;
    public float Margins { get { return margins; } }

    [Header("Resolution Screen Options")]
    [SerializeField] Color correctBGColor;
    public Color CorrectBGColor { get { return correctBGColor; } }
    [SerializeField] Color incorrectBGColor;
    public Color IncorrectBGColor { get { return incorrectBGColor; } }
    [SerializeField] Color finalBGColor;
    public Color FinalBGColor { get { return finalBGColor; } }
}
[Serializable()]
public struct UIElements
{
    [SerializeField] RectTransform answersContentArea;
    public RectTransform AnswersContentArea { get { return answersContentArea; } }

    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject { get { return questionInfoTextObject; } }
    
    [Space]
    [SerializeField] TextMeshProUGUI answerTextObject;
    public TextMeshProUGUI AnswerTextObject { get { return answerTextObject; } }

    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }

    [Space]
    
    [SerializeField] GameObject displayImage;
    public GameObject DisplayImage { get { return displayImage; } }
    
    [SerializeField] GameObject displayImageContainer;
    public GameObject DisplayImageContainer { get { return displayImageContainer; } }
    
    [Space]
    
    [SerializeField] GameObject explanationUI;
    public GameObject ExplanationUI { get { return explanationUI; } }
    
    [SerializeField] TextMeshProUGUI explanationUIText;
    public TextMeshProUGUI ExplanationUIText { get { return explanationUIText; } }
    
    [SerializeField] AudioSource narrationSource;
    public AudioSource NarrationSource { get { return narrationSource; } }

    [Space]

    [SerializeField] Animator resolutionScreenAnimator;
    public Animator ResolutionScreenAnimator { get { return resolutionScreenAnimator; } }

    [SerializeField] Image resolutionBG;
    public Image ResolutionBG { get { return resolutionBG; } }

    [SerializeField] TextMeshProUGUI resolutionStateInfoText;
    public TextMeshProUGUI ResolutionStateInfoText { get { return resolutionStateInfoText; } }

    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI ResolutionScoreText { get { return resolutionScoreText; } }

    [Space]

    [SerializeField] TextMeshProUGUI highScoreText;
    public TextMeshProUGUI HighScoreText { get { return highScoreText; } }

    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }

    [SerializeField] RectTransform finishUIElements;
    public RectTransform FinishUIElements { get { return finishUIElements; } }
}
public class UIManager : MonoBehaviour {

    #region Variables

    public enum         ResolutionScreenType   { Correct, Incorrect, Finish }

    [Header("References")]
    [SerializeField]    GameEvents             events                       = null;

    [Header("UI Elements (Prefabs)")]
    [SerializeField]    AnswerData             answerPrefab                 = null;

    [SerializeField]    UIElements             uIElements                   = new UIElements();

    [Space]
    [SerializeField]    UIManagerParameters    parameters                   = new UIManagerParameters();

    private             List<AnswerData>       currentAnswers               = new List<AnswerData>();
    private             int                    resStateParaHash             = 0;

    private Image m_Image;
    
    private             IEnumerator            IE_DisplayTimedResolution    = null;

    #endregion

    #region Default Unity methods

    /// <summary>
    /// Function that is called when the object becomes enabled and active
    /// </summary>
    void OnEnable()
    {
        events.UpdateQuestionUI         += UpdateQuestionUI;
        events.UpdateExplanationUI      += UpdateExplanationUI;
        events.UpdateNarration          += UpdateNarration;
        events.PlayNarration            += PlayNarration;
        events.DisplayResolutionScreen  += DisplayResolution;
        events.ScoreUpdated             += UpdateScoreUI;
    }
    /// <summary>
    /// Function that is called when the behaviour becomes disabled
    /// </summary>
    void OnDisable()
    {
        events.UpdateQuestionUI         -= UpdateQuestionUI;
        events.UpdateExplanationUI      -= UpdateExplanationUI;
        events.UpdateNarration          -= UpdateNarration;
        events.PlayNarration            -= PlayNarration;
        events.DisplayResolutionScreen  -= DisplayResolution;
        events.ScoreUpdated             -= UpdateScoreUI;
    }

    /// <summary>
    /// Function that is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        UpdateScoreUI();
        resStateParaHash = Animator.StringToHash("ScreenState");
    }

    #endregion

    /// <summary>
    /// Function that is used to update new question UI information.
    /// </summary>
    void UpdateQuestionUI(Question question)
    {
        uIElements.QuestionInfoTextObject.text = question.Info;
        m_Image = uIElements.DisplayImage.GetComponent<Image>();
        m_Image.sprite = question.QuestionImage;
        if (question.UseImage == true)
        {
            uIElements.DisplayImageContainer.SetActive(true);
        }
        // Debug.Log(question.GetQuestionType);
        CreateAnswers(question);
    }

    void UpdateNarration(Question question)
    {
        if(uIElements.NarrationSource != null)
        {
            uIElements.NarrationSource.clip = question.Narration;
        }
    }

    void PlayNarration(Question question)
    {
        uIElements.NarrationSource.Play();
    }
    void UpdateExplanationUI(Question question)
    {
        if (uIElements.ExplanationUIText != null)
        {
            uIElements.ExplanationUIText.text = question.Knowledge;
            uIElements.ExplanationUI.SetActive(true);
        }
    }
    /// <summary>
    /// Function that is used to display resolution screen.
    /// </summary>
    void DisplayResolution(ResolutionScreenType type, int score)
    {
        UpdateResUI(type, score);
        uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 2);
        uIElements.MainCanvasGroup.blocksRaycasts = false;

        if (type != ResolutionScreenType.Finish)
        {
            if (IE_DisplayTimedResolution != null)
            {
                StopCoroutine(IE_DisplayTimedResolution);
            }
            IE_DisplayTimedResolution = DisplayTimedResolution();
            StartCoroutine(IE_DisplayTimedResolution);
        }
    }
    IEnumerator DisplayTimedResolution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        uIElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 1);
        uIElements.MainCanvasGroup.blocksRaycasts = true;
    }
    
    /// <summary>
    /// Function that is used to display resolution UI information.
    /// </summary>
    void UpdateResUI(ResolutionScreenType type, int score)
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);

        switch (type)
        {
            case ResolutionScreenType.Correct:
                uIElements.ResolutionBG.color = parameters.CorrectBGColor;
                uIElements.ResolutionStateInfoText.text = "BENAR!";
                uIElements.ResolutionScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.Incorrect:
                uIElements.ResolutionBG.color = parameters.IncorrectBGColor;
                uIElements.ResolutionStateInfoText.text = "SALAH!";
                uIElements.ResolutionScoreText.text = "-" + score;
                break;
            case ResolutionScreenType.Finish:
                uIElements.ResolutionBG.color = parameters.FinalBGColor;
                uIElements.ResolutionStateInfoText.text = "SKOR AKHIR";

                StartCoroutine(CalculateScore());
                uIElements.FinishUIElements.gameObject.SetActive(true);
                uIElements.HighScoreText.gameObject.SetActive(true);
                uIElements.HighScoreText.text = ((highscore > events.StartupHighscore) ? "<color=yellow>new </color>" : string.Empty) + "Skor Tertinggi: " + highscore;
                break;
        }
    }

    /// <summary>
    /// Function that is used to calculate and display the score.
    /// </summary>
    IEnumerator CalculateScore()
    {
        var scoreValue = 0;
        while (scoreValue < events.CurrentFinalScore)
        {
            scoreValue++;
            uIElements.ResolutionScoreText.text = scoreValue.ToString();

            yield return null;
        }
    }

    /// <summary>
    /// Function that is used to create new question answers.
    /// </summary>
    void CreateAnswers(Question question)
    {
        EraseAnswers();

        float offset = 0 - parameters.Margins;
        for (int i = 0; i < question.Answers.Length; i++)
        {
            AnswerData newAnswer = (AnswerData)Instantiate(answerPrefab, uIElements.AnswersContentArea);
            newAnswer.UpdateData(question.Answers[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            uIElements.AnswersContentArea.sizeDelta = new Vector2(uIElements.AnswersContentArea.sizeDelta.x, offset * -1);

            currentAnswers.Add(newAnswer);
        }
    }
    /// <summary>
    /// Function that is used to erase current created answers.
    /// </summary>
    void EraseAnswers()
    {
        foreach (var answer in currentAnswers)
        {
            Destroy(answer.gameObject);
        }
        currentAnswers.Clear();
    }

    /// <summary>
    /// Function that is used to update score text UI.
    /// </summary>
    void UpdateScoreUI()
    {
        uIElements.ScoreText.text = events.CurrentFinalScore.ToString();
    }
}