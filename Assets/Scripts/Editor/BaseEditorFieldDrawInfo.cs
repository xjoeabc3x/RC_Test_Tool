#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class BaseEditorFieldDrawInfo
{
    protected GUIContent gcLabel;
    public Rect rect;
    GUIStyle mStyle;
    public GUIStyle style
    {
        set
        {
            mStyle = value;
        }
        get
        {
            return mStyle;
        }
    }
    public string label
    {
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (gcLabel == null)
                    gcLabel = new GUIContent();
                gcLabel.text = value;
            }
        }
        get
        {
            return gcLabel.text;
        }
    }
    public BaseEditorFieldDrawInfo() {}
    public BaseEditorFieldDrawInfo(Rect rt)
    {
        rect = rt;
    }
    public BaseEditorFieldDrawInfo(Rect rt, string label) : this(rt)
    {
        this.label = label;
    }
    public BaseEditorFieldDrawInfo(Rect rt, string label, GUIStyle style) : this(rt, label)
    {
        this.style = style;
    }
    public abstract void Draw();

    public static float SumHeight(IList<BaseEditorFieldDrawInfo> list)
    {
        float sum = 0;
        foreach (BaseEditorFieldDrawInfo info in list)
            sum += info.rect.height;
        return sum;
    }
}
public abstract class EditableFieldDrawInfo : BaseEditorFieldDrawInfo
{
    public SerializedProperty property;
    public EditableFieldDrawInfo(Rect rt) : base(rt){ }
    public EditableFieldDrawInfo(Rect rt, SerializedProperty property) : base(rt) 
    {
        this.property = property;
    }
    public EditableFieldDrawInfo(Rect rt, SerializedProperty property, GUIStyle style) : this(rt, property)
    {
        this.style = style;
    }
    public EditableFieldDrawInfo(Rect rt, string label, SerializedProperty property) : base(rt, label)
    {
        this.property = property;
    }
    public EditableFieldDrawInfo(Rect rt, string label, SerializedProperty property, GUIStyle style) : this(rt, label, property)
    {
        this.style = style;
    }
}
public class LabelFieldDrawInfo : BaseEditorFieldDrawInfo
{
    GUIContent mGCContent;
    public GUIContent gcContent
    {
        set
        {
            mGCContent = value;
        }
        get
        {
            if (mGCContent == null)
            {
                mGCContent = new GUIContent();
                mGCContent.text = "Empty!!!";
            }
                
            return mGCContent;
        }
    }
    public string content
    {
        set
        {
            gcContent.text = value;
        }
        get
        {
            return gcContent.text;
        }
    }
    public string tooltip
    {
        set
        {
            gcContent.tooltip = value;
        }
        get
        {
            return gcContent.tooltip;
        }
    }
    public Color textColor
    {
        set
        {
            if(style == null)
            {
                style = new GUIStyle(GUI.skin.label);
            }
            style.normal.textColor = value;
        }
        get
        {
            return style == null ? Color.black : style.normal.textColor;
        }
    }
    public LabelFieldDrawInfo(Rect rt) : base(rt) {}
    public LabelFieldDrawInfo(Rect rt, string content) : base(rt)
    {
        this.content = content;
    }
    public LabelFieldDrawInfo(Rect rt, string content, GUIStyle style) : this(rt, content)
    {
        this.style = style;
    }
    public LabelFieldDrawInfo(Rect rt, string label, string content) : this(rt, content)
    {
        this.label = label;
    }
    public LabelFieldDrawInfo(Rect rt, string label, string content, GUIStyle style) : this(rt, label, content)
    {
        this.style = style;
    }
    public override void Draw()
    {
        if (style == null)
            style = new GUIStyle(EditorStyles.label);
        if(gcLabel == null && mGCContent == null)
            EditorGUI.LabelField(rect, gcContent, style);
        else if (gcLabel == null && mGCContent != null)
            EditorGUI.LabelField(rect, mGCContent, style);
        else if(gcLabel != null && mGCContent == null)
            EditorGUI.LabelField(rect, gcLabel, style);
        else 
            EditorGUI.LabelField(rect, gcLabel, gcContent, style);
    }
}
public class PropertyFieldDrawInfo : BaseEditorFieldDrawInfo
{
    public SerializedProperty sp;
    public bool includeChildren = true;
    public PropertyFieldDrawInfo(Rect rt) : base(rt) { }
    public string tooltip
    {
        set
        {
            if(gcLabel != null)
                gcLabel.tooltip = value;
        }
        get
        {
            return gcLabel != null ? gcLabel.tooltip : "";
        }
    }
    public override void Draw()
    {
        if (sp == null)
            EditorGUI.LabelField(rect, "Can't Find sp!!", EditorStyles.boldLabel);
        else
        {
            if (gcLabel == null)
                EditorGUI.PropertyField(rect, sp, includeChildren);
            else
                EditorGUI.PropertyField(rect, sp, gcLabel, includeChildren);
        }
    }
}

public class TextFieldDrawInfo : EditableFieldDrawInfo
{
    public TextFieldDrawInfo(Rect rt) : base(rt) {}
    public TextFieldDrawInfo(Rect rt, SerializedProperty property) : base(rt, property) { }
    public TextFieldDrawInfo(Rect rt, string label, SerializedProperty property) : base(rt, label, property) { }
    public TextFieldDrawInfo(Rect rt, string label, SerializedProperty property, GUIStyle style) : base(rt, label, property, style) { }
    public override void Draw()
    {
        if (style == null)
            style = new GUIStyle(EditorStyles.textField);
        if (gcLabel != null)
        {
            EditorGUIUtility.labelWidth = 80;
            property.stringValue = EditorGUI.TextField(rect, gcLabel, property.stringValue, style);
            EditorGUIUtility.labelWidth = 0;
        }

        else
            property.stringValue = EditorGUI.TextField(rect, property.stringValue, style);
    }
}
public class ToggleDrawInfo : EditableFieldDrawInfo
{
    public ToggleDrawInfo(Rect rt) : base(rt) { }
    public ToggleDrawInfo(Rect rt, SerializedProperty property) : base(rt, property) { }
    public ToggleDrawInfo(Rect rt, string label, SerializedProperty property) : base(rt, label, property) { }
    public ToggleDrawInfo(Rect rt, string label, SerializedProperty property, GUIStyle style) : base(rt, label, property, style) { }
    public override void Draw()
    {
        if (style == null)
            style = new GUIStyle(EditorStyles.toggle);
        if (gcLabel != null)
            property.boolValue = EditorGUI.Toggle(rect, gcLabel, property.boolValue, style);
        else
            property.boolValue = EditorGUI.Toggle(rect, property.boolValue, style);
    }
}
public class FloatFieldDrawInfo : EditableFieldDrawInfo
{
    public FloatFieldDrawInfo(Rect rt) : base(rt) { }
    public FloatFieldDrawInfo(Rect rt, SerializedProperty property) : base(rt, property) { }
    public FloatFieldDrawInfo(Rect rt, string label, SerializedProperty property) : base(rt, label, property) { }
    public FloatFieldDrawInfo(Rect rt, string label, SerializedProperty property, GUIStyle style) : base(rt, label, property, style) { }
    public override void Draw()
    {
        if (style == null)
            style = new GUIStyle(EditorStyles.numberField);
        if (gcLabel != null)
            property.floatValue = EditorGUI.FloatField(rect, gcLabel, property.floatValue, style);
        else
            property.floatValue = EditorGUI.FloatField(rect, property.floatValue, style);
    }
}
public class IntFieldDrawInfo : EditableFieldDrawInfo
{
    public IntFieldDrawInfo(Rect rt) : base(rt) { }
    public IntFieldDrawInfo(Rect rt, SerializedProperty property) : base(rt, property) { }
    public IntFieldDrawInfo(Rect rt, string label, SerializedProperty property) : base(rt, label, property) { }
    public IntFieldDrawInfo(Rect rt, string label, SerializedProperty property, GUIStyle style) : base(rt, label, property, style) { }
    public override void Draw()
    {
        if (style == null)
            style = new GUIStyle(EditorStyles.numberField);
        if (gcLabel != null)
            property.intValue = EditorGUI.IntField(rect, gcLabel, property.intValue, style);
        else
            property.intValue = EditorGUI.IntField(rect, property.intValue, style);
    }
}
public class GUIHorizontalSliderDrawInfo : EditableFieldDrawInfo
{
    float leftValue = 0, rightValue = 0;
    public GUIHorizontalSliderDrawInfo(Rect rt) : base(rt) { }
    public GUIHorizontalSliderDrawInfo(Rect rt, SerializedProperty property) : base(rt, property) { }
    public GUIHorizontalSliderDrawInfo(Rect rt, string label, SerializedProperty property) : base(rt, label, property) { }
    public GUIHorizontalSliderDrawInfo(Rect rt, string label, SerializedProperty property, float leftValue, float rightValue) : base(rt, label, property) 
    {
        this.leftValue = leftValue;
        this.rightValue = rightValue;
    }

    public override void Draw()
    {
        property.floatValue = GUI.HorizontalSlider(rect, property.floatValue, leftValue, rightValue);
    }
}
public class MinMaxSliderDrawInfo : EditableFieldDrawInfo
{
    float minLimit = 0, maxLimit = 0;
    SerializedProperty maxProperty;
    public MinMaxSliderDrawInfo(Rect rt) : base(rt) { }
    public MinMaxSliderDrawInfo(Rect rt, string label, SerializedProperty minProperty, SerializedProperty maxProperty, float minLimit, float maxLimit) 
        : base(rt, label, minProperty) 
    {
        this.maxLimit = maxLimit;
        this.minLimit = minLimit;
        this.maxProperty = maxProperty;
    }
    public override void Draw()
    {
        float min = property.floatValue, max = maxProperty.floatValue;
        if (gcLabel != null)
            EditorGUI.MinMaxSlider(rect, gcLabel, ref min, ref max, minLimit, maxLimit);
        else
            EditorGUI.MinMaxSlider(rect, ref min, ref max, minLimit, maxLimit);
        property.floatValue = min;
        maxProperty.floatValue = max;
    }
}
public class ObjectFieldDrawInfo : EditableFieldDrawInfo
{
    public Object obj { private set; get; }
    System.Type mType;
    public ObjectFieldDrawInfo(Rect rt, Object obj, System.Type type) : base(rt) 
    {
        this.obj = obj;
        mType = type;
    }
    public ObjectFieldDrawInfo(Rect rt, string label, Object obj, System.Type type) : base(rt, null, label)
    {
        this.obj = obj;
        mType = type;
    }
    public override void Draw()
    {
        if (gcLabel != null)
            obj = EditorGUI.ObjectField(rect, gcLabel, obj, mType, false);
        else 
            obj = EditorGUI.ObjectField(rect, obj, mType, false);
    }
}
#endif
