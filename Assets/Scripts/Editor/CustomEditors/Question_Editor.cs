using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Question))]
public class Question_Editor : Editor {

    #region Variables

    #region Serialized Properties
    SerializedProperty  questionInfoProp        = null;
    SerializedProperty  answersProp             = null;
    SerializedProperty  questionKnowledgeProp   = null;
    SerializedProperty  narrationProp           = null;
    SerializedProperty  useTimerProp            = null;
    SerializedProperty  timerProp               = null;
	SerializedProperty  useImageProp            = null;
    SerializedProperty  imageProp               = null;
    SerializedProperty  answerTypeProp          = null;
    SerializedProperty  questionTypeProp        = null;
    SerializedProperty  addScoreProp            = null;

    SerializedProperty  arraySizeProp
    {
        get
        {
            return answersProp.FindPropertyRelative("Array.size");
        }
    }
    #endregion

    private bool        showParameters          = false;

    #endregion

    #region Default Unity methods

    void OnEnable ()
    {
        #region Fetch Properties
        questionInfoProp        = serializedObject.FindProperty("_info");
        answersProp             = serializedObject.FindProperty("_answers");
        questionKnowledgeProp   = serializedObject.FindProperty("_knowledge");
        narrationProp           = serializedObject.FindProperty("_narration");
        useTimerProp            = serializedObject.FindProperty("_useTimer");
        timerProp               = serializedObject.FindProperty("_timer");
		useImageProp            = serializedObject.FindProperty("_useImage");
        imageProp               = serializedObject.FindProperty("_questionImage");
        answerTypeProp          = serializedObject.FindProperty("_answerType");
        questionTypeProp        = serializedObject.FindProperty("_questionType");
        addScoreProp            = serializedObject.FindProperty("_addScore");
        #endregion

        #region Get Values
        showParameters = EditorPrefs.GetBool("Question_showParameters_State");
        #endregion
    }

    public override void OnInspectorGUI ()
    {
        GUILayout.Label("Question", EditorStyles.miniLabel);
        GUIStyle textAreaStyle = new GUIStyle(EditorStyles.textArea)
        {
            fontSize = 15,
            fixedHeight = 30,
            alignment = TextAnchor.MiddleLeft
        };
        questionInfoProp.stringValue = EditorGUILayout.TextArea(questionInfoProp.stringValue, textAreaStyle);
        
        GUILayout.Space(7.5f);
        GUILayout.Label("Knowledge", EditorStyles.miniLabel);
        GUIStyle AnswertextAreaStyle = new GUIStyle(EditorStyles.textArea)
        {
            fontSize = 11,
            fixedHeight = 100,
            alignment = TextAnchor.UpperLeft
        };
        questionKnowledgeProp.stringValue = EditorGUILayout.TextArea(questionKnowledgeProp.stringValue, AnswertextAreaStyle);
        
        GUILayout.Space(7.5f);
        //EditorGUILayout.ObjectField("\ sound", narrationProp, typeof(AudioClip), true);
        EditorGUILayout.PropertyField(narrationProp, new GUIContent("Knowledge Narration", "This question Knowledge Narration?"));
        
        GUILayout.Space(7.5f);
        GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
        {
            fontSize = 10
        };
        EditorGUI.BeginChangeCheck();
        showParameters = EditorGUILayout.Foldout(showParameters, "Parameters", foldoutStyle);
        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetBool("Question_showParameters_State", showParameters);
        }
        if (showParameters)
        {
            EditorGUILayout.PropertyField(useTimerProp, new GUIContent("Use Timer", "Should this question have a timer?"));
            if (useTimerProp.boolValue)
            {
                timerProp.intValue = EditorGUILayout.IntSlider(new GUIContent("Time"), timerProp.intValue, 1, 120);
            }
			GUILayout.Space(2);
            EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(useImageProp, new GUIContent("Use Image", "Should this question have an image?"));
            if (useImageProp.boolValue)
            {
                EditorGUILayout.PropertyField(imageProp, new GUIContent("Question Image", "This question Image?"));
            }
            GUILayout.Space(2);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(questionTypeProp, new GUIContent("Question Type", "Specify this question type."));
            EditorGUILayout.PropertyField(answerTypeProp, new GUIContent("Answer Type", "Specify this question answer type."));
            if (EditorGUI.EndChangeCheck())
            {
                if (answerTypeProp.enumValueIndex == (int)Question.AnswerType.Single)
                {
                    if (GetCorrectAnswersCount() > 1)
                    {
                        UncheckCorrectAnswers();
                    }
                }
            }
            addScoreProp.intValue = EditorGUILayout.IntSlider(new GUIContent("Add Score"), addScoreProp.intValue, 0, 100);
        }
        GUILayout.Space(7.5f);
        GUILayout.Label("Answers", EditorStyles.miniLabel);
        DrawAnswers();

        serializedObject.ApplyModifiedProperties();
    }

    #endregion

    void DrawAnswers ()
    {
        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(arraySizeProp);
        GUILayout.Space(5);

        EditorGUI.indentLevel++;
        for (int i = 0; i < arraySizeProp.intValue; i++)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(answersProp.GetArrayElementAtIndex(i));
            if (EditorGUI.EndChangeCheck())
            {
                if (answerTypeProp.enumValueIndex == (int)Question.AnswerType.Single)
                {
                    SerializedProperty isCorrectProp = answersProp.GetArrayElementAtIndex(i).FindPropertyRelative("_isCorrect");

                    if (isCorrectProp.boolValue)
                    {
                        UncheckCorrectAnswers();
                        answersProp.GetArrayElementAtIndex(i).FindPropertyRelative("_isCorrect").boolValue = true;

                        serializedObject.ApplyModifiedProperties();
                    }
                }
            }
            GUILayout.Space(5);
        }

        EditorGUILayout.EndVertical();
        EditorGUI.indentLevel--;
    }

    void UncheckCorrectAnswers ()
    {
        for (int i = 0; i < arraySizeProp.intValue; i++)
        {
            answersProp.GetArrayElementAtIndex(i).FindPropertyRelative("_isCorrect").boolValue = false;
        }
    }

    int GetCorrectAnswersCount ()
    {
        int count = 0;
        for (int i = 0; i < arraySizeProp.intValue; i++)
        {
            if (answersProp.GetArrayElementAtIndex(i).FindPropertyRelative("_isCorrect").boolValue)
            {
                count++;
            }
        }
        return count;
    }
}