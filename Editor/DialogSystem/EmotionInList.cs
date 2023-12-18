
/**
* Copyright (C) Fernando Holguin Weber, and Studio High Ground - All Rights Reserved
* Unauthorized copying of this file, via any medium is strictly prohibited
* Proprietary and confidential.
* Written by Fernando Holguin <fernando@studiohighground.com>
* 
* UNDER NO CIRCUMSTANCES IS FERNANDO HOLGUIN WEBER, OR Studio High Ground, ITS PROGRAM
* DEVELOPERS OR SUPPLIERS LIABLE FOR ANY OF THE FOLLOWING, EVEN IF INFORMED OF THEIR
* POSSIBILITY: LOSS OF, OR DAMAGE TO, DATA; DIRECT, SPECIAL, INCIDENTAL, OR INDIRECT
* DAMAGES, OR FOR ANY ECONOMIC CONSEQUENTIAL DAMAGES; OR  LOST PROFITS, BUSINESS,
* REVENUE, GOODWILL, OR ANTICIPATED SAVINGS.
* 
*/


using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EmotionInList : PropertyAttribute
{

    public delegate string[] GetStringList();

    public EmotionInList(object character)
    {
        List<string> newStringList = new List<string>();
        newStringList.Add("asdsafasf");
        newStringList.Add("asdsafasfsaasf");

        // int size = character.emotion.count;
        // for (int i = 0; i < size; i++) {
        //      newStringList.Add (character.emotion[i].emotionID);
        //   }
        List = newStringList.ToArray();
    }

    public EmotionInList(Type type, string methodName)
    {
        var method = type.GetMethod(methodName);
        if (method != null)
        {
            List = method.Invoke(null, null) as string[];
        }
        else
        {
            Debug.LogError("NO SUCH METHOD " + methodName + " FOR " + type);
        }
    }

    public string[] List
    {
        get;
        private set;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EmotionInList))]
public class EmotionInListDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var emotionInList = attribute as EmotionInList;
        var list = emotionInList.List;
        if (property.propertyType == SerializedPropertyType.String)
        {
            int index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
            index = EditorGUI.Popup(position, property.displayName, index, list);

            property.stringValue = list[index];
        }
        else if (property.propertyType == SerializedPropertyType.Integer)
        {
            property.intValue = EditorGUI.Popup(position, property.displayName, property.intValue, list);
        }
        else
        {
            base.OnGUI(position, property, label);
        }
    }
}
#endif