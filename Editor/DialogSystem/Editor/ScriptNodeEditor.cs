
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


#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
[CustomEditor(typeof(ScriptNode))]
public class DropDownEditor:Editor {

    void OnSceneGUI () {
        ScriptNode node = ( ScriptNode )target;
        node.UpdateInternal();
    }

    public override void OnInspectorGUI () {
        ScriptNode node = ( ScriptNode )target;

        if (node.character != null) {
            // Emotion
            int size = node.character.emotions.Count;
            if (size > 0) {
                List<string> emotionsStringList = new List<string>();
                for (int i = 0; i < size; i++) {
                    Emotion currentEmotion = node.character.emotions[i]; emotionsStringList.Add(currentEmotion.emotionID);
                }
                GUIContent arrayList = new GUIContent("Emotion");
                node.chosenEmotionIndex = EditorGUILayout.Popup(arrayList, node.chosenEmotionIndex, emotionsStringList.ToArray());
                if (node.chosenEmotionIndex >= node.character.emotions.Count) {
                    node.chosenEmotionIndex = 0;
                }

                node.chosenEmotion = node.character.emotions[node.chosenEmotionIndex];
            }
        }

        base.OnInspectorGUI();


        if (node.hash != "" && node.character == null) {
            if (GUILayout.Button("-Restart Hash")) {
                node.hash = "";
            }
        }
        if (node.nextNode == null) {
            if (GUILayout.Button("-Attempt link to next node")) {
                node.NextNode();
            }
        }
        if (GUILayout.Button("-Connect to previous node")) {
            node.PrevNode();
        }

   
        if (Application.isPlaying)
            return;

        if (GUILayout.Button("Clear Hash duplicates"))
        {
            var activeObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var scriptNodes = activeObjects.GetRootGameObjects();

            List<ScriptNode> all = new();

            for(int i = 0; i< scriptNodes.Length; i++)
            {
                var isParticles = scriptNodes[i].GetComponent(typeof(ScriptNode)) as ScriptNode;
                if (isParticles != null)
                {
                    all.Add(isParticles);
                    continue;
                }
                var allInners = scriptNodes[i].GetComponentsInChildren<ScriptNode>();
                for (int a = 0; a< allInners.Length; a++)
                {
                        all.Add(allInners[a]);
                        continue;
                }

            }

            for (int i = 0; i < all.Count; i++)
            {
                var nodeIn = all[i];
                for (int a = i; a < all.Count; a++)
                {
                    var nodeOut = all[a];
                    if (nodeIn != nodeOut)
                    {
                        if (nodeIn.hash == nodeOut.hash)
                        {
                            nodeOut.restartHash();
                        }
                    }

                }
            }
        }


        node.more = GUILayout.Toggle(node.more, "More..");
        if (node.more) {
            GUILayout.Space(10);
            GUILayout.Label("More Options:");
            node.hideInnards = GUILayout.Toggle(node.hideInnards, "Innards hidden");
        }
        if (node.hideInnards) {
            var objects = node.GetComponentsInChildren<Transform>();
            int size = objects.Length;
            for (int i = 0; i < size; i++) {
                var obj = objects[i];
                if (obj != node.transform)
                    obj.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable;
            }
            EditorApplication.DirtyHierarchyWindowSorting();
        } else {
            var objects = node.GetComponentsInChildren<Transform>();
            int size = objects.Length;
            for (int i = 0; i < size; i++) {
                var obj = objects[i];
                if (obj != node.transform)
                    obj.hideFlags = HideFlags.None;
            }
            EditorApplication.DirtyHierarchyWindowSorting();
        }
    }
    private void PostDuplicateMethod () {
        Debug.Log("Post Duplication");

        // detect the new duplicate
        //ScriptNode node = ( ScriptNode )target;
        // node.FixDuplicate();
    }
}
#endif