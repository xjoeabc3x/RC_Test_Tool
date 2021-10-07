using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
[CustomEditor(typeof(RectTransform)), CanEditMultipleObjects]
public class RectTransformAnchorInspector : Editor
{
    private const string kExpandAnchorQuickSetPrefName = "RectTransformAnchorInspector.ExpandAnchorQuickSet";
    private static Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
    private System.Type decoratedEditorType;
    Editor m_EditorInstance;
    SerializedProperty m_SPAnchorMin;
    SerializedProperty m_SPAnchorMax;
    SerializedProperty m_SPAnchoredPosition;
    private bool m_ShowAnchorTool = false;
    private static Dictionary<string, MethodInfo> decoratedMethods = new Dictionary<string, MethodInfo>();
    // empty array for invoking methods using reflection
    private static readonly object[] EMPTY_ARRAY = new object[0];
    protected Editor EditorInstance
    {
        get
        {
            if (m_EditorInstance == null && targets != null && targets.Length > 0)
            {
                m_EditorInstance = CreateEditor(targets, decoratedEditorType);
            }
            if (m_EditorInstance == null)
            {
                Debug.LogError("Could not create editor !");
            }
            return m_EditorInstance;
        }
    }
    public RectTransformAnchorInspector()
    {
        decoratedEditorType = editorAssembly.GetTypes().Where(t => t.Name == "RectTransformEditor").FirstOrDefault();
    }
    /// <summary>
    /// Delegates a method call with the given name to the decorated editor instance.
    /// </summary>
    protected void CallInspectorMethod(string methodName, bool single)
    {
        MethodInfo method = null;

        // Add MethodInfo to cache
        if (!decoratedMethods.ContainsKey(methodName))
        {
            var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

            method = decoratedEditorType.GetMethod(methodName, flags);

            if (method != null)
            {
                decoratedMethods[methodName] = method;
            }
            else
            {
                Debug.LogError(string.Format("Could not find method {0}", methodName));
            }
        }
        else
        {
            method = decoratedMethods[methodName];
        }

        if (method == null)
            return;
        method.Invoke(EditorInstance, EMPTY_ARRAY);
    }
	#region Unity override or default
	private void OnSceneGUI()
	{
		CallInspectorMethod("OnSceneGUI", true);
	}
	protected override void OnHeaderGUI()
    {
        CallInspectorMethod("OnHeaderGUI", false);
    }
    
    public override void OnInspectorGUI()
    {
        EditorInstance.OnInspectorGUI();
        DrawDecorate();
    }
    public override void DrawPreview(Rect previewArea)
    {
        EditorInstance.DrawPreview(previewArea);
    }

    public override string GetInfoString()
    {
        return EditorInstance.GetInfoString();
    }

    public override GUIContent GetPreviewTitle()
    {
        return EditorInstance.GetPreviewTitle();
    }

    public override bool HasPreviewGUI()
    {
        return EditorInstance.HasPreviewGUI();
    }

    public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
    {
        EditorInstance.OnInteractivePreviewGUI(r, background);
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        EditorInstance.OnPreviewGUI(r, background);
    }

    public override void OnPreviewSettings()
    {
        EditorInstance.OnPreviewSettings();
    }

    public override void ReloadPreviewInstances()
    {
        EditorInstance.ReloadPreviewInstances();
    }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        return EditorInstance.RenderStaticPreview(assetPath, subAssets, width, height);
    }

    public override bool RequiresConstantRepaint()
    {
        return EditorInstance.RequiresConstantRepaint();
    }

    public override bool UseDefaultMargins()
    {
        return EditorInstance.UseDefaultMargins();
    }
    #endregion
    #region Anchor Set
    int m_OptionX = 2;
    int m_OptionY = 2;
    string[] m_Options = { "Pivot", "Stretch", "None" };
    bool m_IsExpandAnchorQuickSet;
	void DrawDecorate()
    {
        GUI.backgroundColor = Color.yellow;
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Anchor Quick Set : ", "label", GUILayout.MaxWidth(150)))
            m_IsExpandAnchorQuickSet = !m_IsExpandAnchorQuickSet;
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Apply PivotXY", GUILayout.MaxWidth(150)))
            AnchoredToPivot(true, true);
        if (GUILayout.Button("Apply StretchXY", GUILayout.MaxWidth(150)))
            StretchAnchor(true, true);
        EditorGUILayout.EndHorizontal();
        GUI.backgroundColor = Color.yellow;
        if (m_IsExpandAnchorQuickSet)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("X : ", GUILayout.MaxWidth(40));
            GUI.backgroundColor = Color.red;
            m_OptionX = GUILayout.Toolbar(m_OptionX, m_Options);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Y : ", GUILayout.MaxWidth(40));
            m_OptionY = GUILayout.Toolbar(m_OptionY, m_Options);
            EditorGUILayout.EndHorizontal();
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Apply"))
            {
                AnchoredToPivot(m_OptionX == 0, m_OptionY == 0);
                StretchAnchor(m_OptionX == 1, m_OptionY == 1);
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndVertical();
        GUI.backgroundColor = Color.white;
    }

    void AnchoredToPivot(bool forX, bool forY)
    {
        if (!forX && !forY)
            return;
        foreach (Object tmp in targets)
        {
            RectTransform rt = tmp as RectTransform;
            RectTransform rtParent = rt.parent as RectTransform;
            Vector2 min = rt.anchorMin;
            Vector2 max = rt.anchorMax;
            Vector2 size = rt.rect.size;
            Vector2 sizeDelta = rt.sizeDelta;
            Vector2 anchoredPosition = rt.anchoredPosition;
            if (forX)
            {
                // 當 min.x == max.x
                if (Mathf.Approximately(min.x, max.x))
                {
                    // 計算點的位置
                    min.x += (anchoredPosition.x / rtParent.rect.width);
                }
                else
                {
                    min.x = (rtParent.rect.width * min.x) + rt.offsetMin.x;
                    min.x = (min.x + (size.x * rt.pivot.x)) / rtParent.rect.width;
                }
                anchoredPosition.x = 0;
                sizeDelta.x = (int)(size.x + 0.5f);
                max.x = min.x;
            }
            if (forY)
            {
                // 當 min.y == max.y
                if (Mathf.Approximately(min.y, max.y))
                {
                    // 計算點的位置
                    min.y += (anchoredPosition.y / rtParent.rect.height);
                }
                else
                {
                    min.y = (rtParent.rect.height * min.y) + rt.offsetMin.y;
                    min.y = (min.y + (size.y * rt.pivot.y)) / rtParent.rect.height;
                }
                anchoredPosition.y = 0;
                sizeDelta.y = (int)(size.y + 0.5f);
                max.y = min.y;
            }
            rt.anchorMin = min;
            rt.anchorMax = max;
            rt.sizeDelta = sizeDelta;
            rt.anchoredPosition = anchoredPosition;
        }
    }
    void StretchAnchor(bool forX, bool forY)
    {
        if (!forX && !forY)
            return;
        foreach (Object tmp in targets)
        {
            RectTransform rt = tmp as RectTransform;
            RectTransform rtParent = rt.parent as RectTransform;
            Vector2 min = rt.anchorMin;
            Vector2 max = rt.anchorMax;
            Vector2 size = rt.rect.size;
            Vector2 sizeDelta = rt.sizeDelta;
            Vector2 leftSize = size * rt.pivot;
            Vector2 anchoredPosition = rt.anchoredPosition;
            if (forX)
            {
                // 當 min.x == max.x
                if (Mathf.Approximately(min.x, max.x))
                {
                    // 計算點的位置
                    float pos = (rtParent.rect.width * min.x) + anchoredPosition.x;
                    min.x = (pos - leftSize.x) / rtParent.rect.width;
                    max.x = (pos + (size.x - leftSize.x)) / rtParent.rect.width;
                }
                else
                {
                    min.x += rt.offsetMin.x / rtParent.rect.width;
                    max.x += rt.offsetMax.x / rtParent.rect.width;
                }
                sizeDelta.x = 0;
                anchoredPosition.x = 0;
            }
            if (forY)
            {
                // 當 min.y == max.y
                if (Mathf.Approximately(min.y, max.y))
                {
                    // 計算點的位置
                    float pos = (rtParent.rect.height * min.y) + anchoredPosition.y;
                    min.y = (pos - leftSize.y) / rtParent.rect.height;
                    max.y = (pos + (size.y - leftSize.y)) / rtParent.rect.height;
                }
                else
                {
                    min.y += rt.offsetMin.y / rtParent.rect.height;
                    max.y += rt.offsetMax.y / rtParent.rect.height;
                }
                sizeDelta.y = 0;
                anchoredPosition.y = 0;
            }
            rt.anchorMin = min;
            rt.anchorMax = max;
            rt.sizeDelta = sizeDelta;
            rt.anchoredPosition = anchoredPosition;
        }
    }
    #endregion
    private void OnEnable()
    {
        m_SPAnchorMin = serializedObject.FindProperty("m_AnchorMin");
        m_SPAnchorMax = serializedObject.FindProperty("m_AnchorMax");
        m_SPAnchoredPosition = serializedObject.FindProperty("m_AnchoredPosition");
        m_IsExpandAnchorQuickSet = EditorPrefs.GetBool(kExpandAnchorQuickSetPrefName, false);
        SetEditorInstance();
    }
    void SetEditorInstance()
    {
        if (m_EditorInstance == null && targets != null && targets.Length > 0)
        {
            m_EditorInstance = CreateEditor(targets, decoratedEditorType);
        }
        if (m_EditorInstance == null)
        {
            Debug.LogError("Could not create editor !");
        }
    }
}
