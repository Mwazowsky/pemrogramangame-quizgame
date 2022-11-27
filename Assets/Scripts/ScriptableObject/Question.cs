using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable()]
public struct Answer
{
    [SerializeField] private string _info;
    public string Info { get { return _info; } }

    [SerializeField] private bool _isCorrect;
    public bool IsCorrect { get { return _isCorrect; } }
}
[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/new Question")]
public class Question : ScriptableObject {
    public enum                 AnswerType                  { Multi, Single }
    
    public enum                 QuestionType                { Tumbuhan, Hewan, Transportasi, KNW_Tumbuhan, KNW_Hewan, KNW_Transportasi }

    [SerializeField] private    String      _info           = String.Empty;
    public                      String      Info            { get { return _info; } }
    
    [SerializeField]    AudioClip           _narration            = null;
    public              AudioClip           Narration            { get { return _narration; } }
    
    [SerializeField]            Answer[]    _answers        = null;
    public                      Answer[]    Answers         { get { return _answers; } }

    [SerializeField] private    String      _knowledge           = String.Empty;
    public                      String      Knowledge            { get { return _knowledge; } }
    //Parameters

    [SerializeField] private    bool        _useTimer       = false;
    public                      bool        UseTimer        { get { return _useTimer; } }

	[SerializeField] private    bool        _useImage       = false;
    public                      bool        UseImage        { get { return _useImage; } }

    [SerializeField] private    int         _timer          = 0;
    public                      int         Timer           { get { return _timer; } }

	[SerializeField] private    Sprite          _questionImage          = null;
    public                      Sprite          QuestionImage           { get { return _questionImage; } }

    [SerializeField] private    AnswerType  _answerType     = AnswerType.Multi;
    public                      AnswerType  GetAnswerType   { get { return _answerType; } }
    
    [SerializeField] private    QuestionType  _questionType     = QuestionType.Tumbuhan;
    public                      QuestionType  GetQuestionType   { get { return _questionType; } }

    [SerializeField] private    int         _addScore       = 10;
    public                      int         AddScore        { get { return _addScore; } }

    /// <summary>
    /// Function that is called to collect and return correct answers indexes.
    /// </summary>
    public List<int> GetCorrectAnswers ()
    {
        List<int> CorrectAnswers = new List<int>();
        for (int i = 0; i < Answers.Length; i++)
        {
            if (Answers[i].IsCorrect)
            {
                CorrectAnswers.Add(i);
            }
        }
        return CorrectAnswers;
    }
}