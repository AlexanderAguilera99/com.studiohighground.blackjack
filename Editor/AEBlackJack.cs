
/**
 * Copyright (C) Fernando Holguin Weber, and Studio High Ground(tm) - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential.
 * 
 * 
 * UNDER NO CIRCUMSTANCES IS Fernando Holguin Weber, OR Studio High Ground, ITS PROGRAM
 * DEVELOPERS OR SUPPLIERS LIABLE FOR ANY OF THE FOLLOWING, EVEN IF INFORMED OF THEIR
 * POSSIBILITY: LOSS OF, OR DAMAGE TO, DATA; DIRECT, SPECIAL, INCIDENTAL, OR INDIRECT
 * DAMAGES, OR FOR ANY ECONOMIC CONSEQUENTIAL DAMAGES; OR  LOST PROFITS, BUSINESS,
 * REVENUE, GOODWILL, OR ANTICIPATED SAVINGS.
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class AEBlackJack:EditorWindow
{
    public static string report = "";

    private static string FILE_EXTENSION = ".unity";

    public static EventSettings eventSettings;

    public static BlackJackLinker blackJackLinker;

    [MenuItem("Window/Amy's Escape/BlackJack Manager")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ShowWindow()
    {
        GetWindow<AEBlackJack>("AEBlackJack");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnGUI()
    {
        // Buttons Style
        GUIStyle buttonStyleBold = new GUIStyle(GUI.skin.button);
        buttonStyleBold.fixedHeight = 34;
        buttonStyleBold.fontStyle = FontStyle.Bold;

        // Title label style
        GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontStyle = FontStyle.Bold;
        labelStyle.fixedHeight = 22;
        labelStyle.fontSize = 14;

        //Label 'Modes'
        GUILayout.Space(1);
        GUILayout.BeginHorizontal();
        GUILayout.Label("BLACKJACK WORKSPACE", labelStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Sync BlackJack", buttonStyleBold))
        {
            GUILayout.Space(5);

            report = "Export started on: " + System.DateTime.Now;

            OpenBlackJack();
        }

        GUILayout.Space(10);
        EditorGUI.BeginDisabledGroup(true); 
        EditorGUILayout.LabelField(report, GUILayout.MinHeight(150));
        EditorGUI.EndDisabledGroup();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [MenuItem("AENavigator/EditorTest")]
    public static void OpenBlackJack()
    {
        eventSettings = Tools.FindObjectOfType<EventSettings>();

        blackJackLinker = Tools.FindObjectOfType<BlackJackLinker>();

        var blackJackScene = "";

        try
        {
            blackJackScene = eventSettings.BlackjackScene;
        }
        catch (NullReferenceException exception)
        {
            report += "\nNo event-settings found in the scene";
            report += "\n[Create a [event-settings] GameObject and put the script EventSettings.cs in it]";
            report += "\n[Make sure to save scene beforehand]";
            return;
        }

        if (!blackJackScene.Contains(FILE_EXTENSION))
        {
            blackJackScene += FILE_EXTENSION;
        }

        var pathOrigin = EditorApplication.currentScene;

        //SPLIT scene path

        var pathSplit = EditorApplication.currentScene.Split(char.Parse("/"));

        var currentPath = "";

        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            report += "\nSaved!";
        }
        else
        {
            report += "\nAborted.";
            return;
        }

        for (int i = 0; i< pathSplit.Length-1; i++)
        {
            currentPath += pathSplit[i]+"/";
        }
        currentPath += blackJackScene;

        try
        {
            EditorSceneManager.OpenScene(currentPath);
        } catch (ArgumentException ex)
        {
            report += "\nBlackJack scene Not Found " + eventSettings.BlackjackScene;
            report += "\n[Make sure file is in the same directory or see if you wrote the name of the scene]";
            return;
        }

        report += "\nGoing to Blackjack scene [" + eventSettings.BlackjackScene + "]";

        //Geting the NODES

        List<NodeContent> contents = new();
        NodeRoot[] conversations = Tools.FindObjectsOfType<NodeRoot>();

        if(conversations.Length == 0)
        {
            report += "\nNo conversations found";
            report += "\n[Make sure the Blackjack file is valid]";
            EditorSceneManager.OpenScene(pathOrigin);
            return;
        }

        var size = conversations.Length;

        for(int i = 0; i < size; i++)
        {
            var conversation = conversations[i];

            conversation.UpdateGizmo();

            if(conversation.mainnode != null)
            {
                contents.Add(conversation.mainnode);
            }
        }

        report += "\nGetting mainNode in Blackjack scene";

        var contentCarried = contents.ToArray<NodeContent>();

        PreviousScene(contentCarried, pathOrigin);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void PreviousScene(NodeContent[] nodes, string previousScene)
    {
        EditorSceneManager.OpenScene(previousScene);

        EventSettings[] eventSettings = Tools.FindObjectsOfType<EventSettings>();

        if(eventSettings.Length > 0 && nodes.Length > 0)
        {
            eventSettings[0].conversations = nodes;
        }

        report += "\nBurned dialog in current scene";
    }
}
