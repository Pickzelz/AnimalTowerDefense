using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class DbWindow : EditorWindow
{
    bool _isNeedRefresh = false;

    List<string> _tableName;
    Dictionary<string, string> _tableField;
    Dictionary<string, object> _tableValue;

    public Rect TableWindowRect = new Rect(0, 0, 200, 200);


    [MenuItem("Window/Databases")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(DbWindow));
    }
    #region get set table
    List<string> tableName {
        get
        {
            if(_tableName == null)
            {
                _tableName = new List<string>();
            }
            return _tableName;
        }
        set
        {
            if (_tableName == null)
            {
                _tableName = new List<string>();
            }
            _tableName = value;
        }
    }

    Dictionary<string, object> tableValue
    {
        get
        {
            if (_tableValue == null)
            {
                _tableValue= new Dictionary<string, object>();
            }
            return _tableValue;
        }
        set
        {
            if (_tableValue == null)
            {
                _tableValue = new Dictionary<string, object>();
            }
            _tableValue = value;
        }
    }

    Dictionary<string,string> tableField
    {
        get
        {
            if(_tableField == null)
            {
                _tableField = new Dictionary<string, string>();
            }
            return _tableField;
        }
        set
        {
            if (_tableField == null)
            {
                _tableField = new Dictionary<string, string>();
            }

            _tableField = value;
        }
    }
    #endregion

    private void OnGUI()
    {
        TableWindowRect.width = Screen.width;
        TableWindowRect.height = Screen.height; 
        BeginWindows();
        {
            TableWindowRect = GUILayout.Window(1, TableWindowRect, TableWindow, "Hi There");
        }
        //if (GUILayout.Button("Generate C# File"))
        //{
        //    string dirPath = Application.dataPath + "/models";
        //    if (!Directory.Exists(dirPath))
        //    {
        //        Directory.CreateDirectory(dirPath);
        //    }
        //    string path = dirPath + "/database.cs";
        //    File.WriteAllText(path, NewClassTemplate());
        //    _isNeedRefresh = true;
        //}
        EndWindows();
        

        if (_isNeedRefresh)
        {
            AssetDatabase.Refresh();
            _isNeedRefresh = false;
        }
    }
    void TableWindow(int unusedID)
    {
        EditorGUILayout.BeginVertical();
        {
            CreateDatabaseTable();
        }
        EditorGUILayout.EndVertical();
    }
    private void CreateDatabaseTable()
    {
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Header 1");
                EditorGUILayout.LabelField("Body 1");
                EditorGUILayout.LabelField("Body 1");
                EditorGUILayout.LabelField("Body 1");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Header 2");
                EditorGUILayout.LabelField("Body 2");
                EditorGUILayout.LabelField("Body 2");
                EditorGUILayout.LabelField("Body 2");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Header 1");
                EditorGUILayout.LabelField("Body 1");
                EditorGUILayout.LabelField("Body 1");
                EditorGUILayout.LabelField("Body 1");
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("Header 2");
                EditorGUILayout.LabelField("Body 2");
                EditorGUILayout.LabelField("Body 2");
                EditorGUILayout.LabelField("Body 2");
            }
            EditorGUILayout.EndVertical();

        }
        EditorGUILayout.EndHorizontal();
    }

    private string NewClassTemplate()
    {
        string template = "public class NewClass {\n" +
            "\n" +
            "}";

        return template;
    }
}
