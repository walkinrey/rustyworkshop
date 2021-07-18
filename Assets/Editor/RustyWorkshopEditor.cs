using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(RustyWorkshop))]
public class RustyWorkshopEditor : Editor
{
    public static GUIStyle RichStyle(bool middleCenter) 
    {
        var style = new GUIStyle();
        style.richText = true;
        if(middleCenter) style.alignment = TextAnchor.MiddleCenter;
        return style;
    }
    public override void OnInspectorGUI()
    {
        var workshop = (RustyWorkshop)target;

        GUILayout.Space(10f);

        GUILayout.Label("<b><size=16><color=white>Start Interface Fields</color></size></b>", RichStyle(true));

        GUILayout.Space(10f);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Panel Object</color>", RichStyle(false));
        workshop._panelStart = (GameObject)EditorGUILayout.ObjectField(workshop._panelStart, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Status Text</color>", RichStyle(false));
        workshop._statusTextStart = (Text)EditorGUILayout.ObjectField(workshop._statusTextStart, typeof(Text), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Language Text</color>", RichStyle(false));
        workshop._languageText = (Text)EditorGUILayout.ObjectField(workshop._languageText, typeof(Text), true);
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10f);

        GUILayout.Label("<b><size=16><color=white>New Item Interface Fields</color></size></b>", RichStyle(true));

        GUILayout.Space(10f);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Panel Object</color>", RichStyle(false));
        workshop._interfaceNewItem = (GameObject)EditorGUILayout.ObjectField(workshop._interfaceNewItem, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Status Text</color>", RichStyle(false));
        workshop._statusText = (Text)EditorGUILayout.ObjectField(workshop._statusText, typeof(Text), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Item Name Input Field</color>", RichStyle(false));
        workshop._itemNameInput = (InputField)EditorGUILayout.ObjectField(workshop._itemNameInput, typeof(InputField), true);
        GUILayout.EndHorizontal();
        
        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Item Icon Input Field</color>", RichStyle(false));
        workshop._itemIconInput = (InputField)EditorGUILayout.ObjectField(workshop._itemIconInput, typeof(InputField), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(10f);

        GUILayout.Label("<b><size=16><color=white>Update Item Interface Fields</color></size></b>", RichStyle(true));

        GUILayout.Space(10f);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Panel Object</color>", RichStyle(false));
        workshop._interfaceUpdateItem = (GameObject)EditorGUILayout.ObjectField(workshop._interfaceUpdateItem, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Status Text</color>", RichStyle(false));
        workshop._statusTextUpdate = (Text)EditorGUILayout.ObjectField(workshop._statusTextUpdate, typeof(Text), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Header Item Icon Text</color>", RichStyle(false));
        workshop._headerItemIconText = (Text)EditorGUILayout.ObjectField(workshop._headerItemIconText, typeof(Text), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Item ID Input Field</color>", RichStyle(false));
        workshop._itemUpdateIDinput = (InputField)EditorGUILayout.ObjectField(workshop._itemUpdateIDinput, typeof(InputField), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Item Name Input Field</color>", RichStyle(false));
        workshop._itemUpdateNameInput = (InputField)EditorGUILayout.ObjectField(workshop._itemUpdateNameInput, typeof(InputField), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Item Icon Input Field</color>", RichStyle(false));
        workshop._itemUpdateIconInput = (InputField)EditorGUILayout.ObjectField(workshop._itemUpdateIconInput, typeof(InputField), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Item Changelog Input Field</color>", RichStyle(false));
        workshop._itemUpdateChangelogInput = (InputField)EditorGUILayout.ObjectField(workshop._itemUpdateChangelogInput, typeof(InputField), true);
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10f);

        GUILayout.Label("<b><size=16><color=white>Notice Fields</color></size></b>", RichStyle(true));

        GUILayout.Space(10f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Notice Panel Object</color>", RichStyle(false));
        workshop._notice = (GameObject)EditorGUILayout.ObjectField(workshop._notice, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Notice Footer Text Object</color>", RichStyle(false));
        workshop._noticeFooterText = (GameObject)EditorGUILayout.ObjectField(workshop._noticeFooterText, typeof(GameObject), true);
        GUILayout.EndHorizontal();
    
        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Notice Progress Bar Object</color>", RichStyle(false));
        workshop._progressBar = (GameObject)EditorGUILayout.ObjectField(workshop._progressBar, typeof(GameObject), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Notice Text</color>", RichStyle(false));
        workshop._noticeText = (Text)EditorGUILayout.ObjectField(workshop._noticeText, typeof(Text), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Notice Header Text</color>", RichStyle(false));
        workshop._noticeHeaderText = (Text)EditorGUILayout.ObjectField(workshop._noticeHeaderText, typeof(Text), true);
        GUILayout.EndHorizontal();

        GUILayout.Space(5f);

        GUILayout.BeginHorizontal();
        GUILayout.Label("<color=white>Progress Bar Image</color>", RichStyle(false));
        workshop._progressBarImage = (Image)EditorGUILayout.ObjectField(workshop._progressBarImage, typeof(Image), true);
        GUILayout.EndHorizontal();
    }
}
