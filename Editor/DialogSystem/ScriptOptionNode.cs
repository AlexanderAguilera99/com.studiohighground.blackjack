
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
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScriptOptionNode:MonoBehaviour, INode {



    [HideInInspector]
    public int clicks = 0;
    [HideInInspector]
    public bool newOb = true;
    [HideInInspector]
    public bool optimizedMode = false;


    [SerializeField]
    //[HideInInspector]
    public string hash = "";
    //

    public ScriptNode nextNode;

    [SerializeField]
    public ScriptCharacter character = new ScriptCharacter();

    // Emotion

    [SerializeField]
    [HideInInspector]
    public int chosenEmotionIndex;

    [SerializeField]
    [HideInInspector]
    public List<string> emotionList = new List<string>() { };

    [SerializeField]
    [HideInInspector]
    public Emotion chosenEmotion;


    [SerializeField]
    public EyeTracker eyeTracker;
    [SerializeField]
    [TextArea]
    public string contents;

    [HideInInspector]
    [SerializeField]
    public bool more;
    [HideInInspector]
    [SerializeField]
    public bool hideInnards = true;

    //

    private TextContainer textInfo;
    //

    string oldStatus = "   ";

    string updatedContent = "";


    void Start () {
        UpdateInternal();
    }

    void OnDrawGizmos () {
        if (Application.isPlaying)
            return;
        if (!optimizedMode)
            ColorStuff();
        UpdateInternal();
    }

    void OnDrawGizmosSelected () {
        if (Application.isPlaying)
            return;
    }

    public void ColorStuff () {

        Vector3 here = transform.localPosition;
        float s = transform.localScale.x;

        Gizmos.color = new Color(1f, 1f, .3f, .1f);

        Gizmos.DrawCube(here, ScriptNodeTexts.BASE_SIZE * s);

        //Gizmos.DrawSphere(here + ScriptNodeTexts.NODE_START, ScriptNodeTexts.NODE_SIZE);
        //Gizmos.DrawSphere(here + ScriptNodeTexts.NODE_CONTINUE, ScriptNodeTexts.NODE_SIZE);

        Gizmos.color = new Color(1f, 1f, 1f, 1f);
        if (nextNode != null)
            Gizmos.DrawLine(here + ScriptNodeTexts.NODE_START * s, nextNode.GetGameObject().transform.localPosition + ScriptNodeTexts.NODE_CONTINUE * nextNode.transform.localScale.x);

        //Gizmos.DrawCube(here + Vector3.right * .165f, ScriptNodeTexts.BASE_SIZE + Vector3.left * .325f);

    }
    public void UpdateInternal (bool force = false) {


        string charname = "null";
        if (character != null) {

            if (chosenEmotionIndex >= character.emotions.Count) {
                chosenEmotionIndex = 0;
            }
            charname = character.name + chosenEmotion.emotionID;
        }
        string newStatus = "" + chosenEmotionIndex + eyeTracker + contents + charname + nextNode + hash;

        if (oldStatus == newStatus && contents == updatedContent && !force) {
            return;
        } else {
        }

        if (textInfo == null) {
            textInfo = new TextContainer();
            var yourScripts = GetComponentsInChildren<Text>();
            int size = yourScripts.Length;
            for (int i = 0; i < size; i++) {

                Text yourScript = yourScripts[i];

                if (yourScript == null)
                    continue;

                if (yourScript.name == "EyeTarget") {
                    textInfo.trackerInfoText = yourScript;
                    continue;
                }
                if (yourScript.name == "Character") {
                    textInfo.characterText = yourScript;
                    continue;
                }
                if (yourScript.name == "Adjective") {
                    textInfo.adjectiveText = yourScript;
                    continue;
                }
                if (yourScript.name == "Content") {
                    textInfo.contentsText = yourScript;
                    continue;
                }
                if (yourScript.name == "Hash") {
                    textInfo.hashText = yourScript;
                    continue;
                }
            }



            var images = GetComponentsInChildren<RawImage>();
            int imgSize = images.Length;
            print("size of images" + " " + imgSize);
            for (int i = 0; i < imgSize; i++) {
                RawImage image = images[i];
                if (image.name == "icon") {
                    textInfo.icon = image;
                }
            }
        }

        if (hash == "" && character != null) {
            print("hash was empty!");
            hash = CreateMiniSHA256();
        }

        textInfo.hashText.text = hash;

        if (character == null) {
            textInfo.characterText.text = ScriptNodeTexts.NEED_CHARACTER;
            textInfo.adjectiveText.text = ScriptNodeTexts.NEED_CHOOSEN_EMOTION;
            name = ScriptNodeTexts.NEED_CHARACTER_NAME;
        } else {
            textInfo.icon.texture = chosenEmotion.Texture;
            string contentForTitle = textInfo.contentsText.text;
            int lengthMax = textInfo.contentsText.text.Length;
            if (lengthMax > 13) {
                contentForTitle = contentForTitle.Substring(0, 10) + "..";
            }
            name = textInfo.characterText.text + ": " + contentForTitle;

            textInfo.characterText.text = character.ScriptName;
            if (character.emotions.Count > 0) {
                textInfo.adjectiveText.text = chosenEmotion.emotionID;
            } else {
                textInfo.adjectiveText.text = ScriptNodeTexts.NEED_EMOTION_CHARACTER;
            }
        }

        if (eyeTracker == null) {
            textInfo.trackerInfoText.text = ScriptNodeTexts.NULL_TARGET;
        } else {
            textInfo.trackerInfoText.text = eyeTracker.name;
        }
        if (contents == "") {
            textInfo.contentsText.text = ScriptNodeTexts.NEED_DIALOG;
            updatedContent = "";
        } else {
            textInfo.contentsText.text = contents;
            updatedContent = textInfo.contentsText.text;
        }

        if (oldStatus == newStatus) {
        } else {
            oldStatus = newStatus;
        }
    }

    public string CreateMiniSHA256 () {
        string data = GetHashCode() + "" + UnityEngine.Random.Range(0, 1000000000) + transform.GetHashCode();
        string rawHash = CryptoService.Sha256(data.GetHashCode().ToString());
        return rawHash.Substring(0, 10);
    }

    public void NextNode () {
        var nodes = this.transform.parent.gameObject.GetComponentsInChildren<INode>();
        int size = nodes.Length;
        for (int i = 0; i < size; i++) {
            var node = nodes[i];
            if (node == this && i + 1 < size) {
                nextNode = nodes[i + 1] as ScriptNode;
                break;
            }
        }
    }

    public void PrevNode () {
        var nodes = this.transform.parent.gameObject.GetComponentsInChildren<INode>();
        int size = nodes.Length;
        print("grabbing nodes" + size);
        for (int i = 0; i < size; i++) {
            var node = nodes[i];
            if (node == this && i > 0) {
                nodes[i - 1].SetNextNode(this);
                break;
            }
        }
    }

    public void FixDuplicate (string hash, string newHash) {
        var nodes = this.transform.parent.gameObject.GetComponentsInChildren<ScriptNode>();
        int size = nodes.Length;
        ScriptNode duplicate = new ScriptNode();
        for (int i = 0; i < size; i++) {
            var node = nodes[i];
            if (node != this)
                if (node.hash == hash && node != this) {
                    print("DUPLICATE FOUND " + i);
                    duplicate = node;
                   // duplicate.nextNode = this;
                    break;
                }
        }

        if (duplicate == null)
            return;

        this.hash = newHash;
        this.transform.localPosition = duplicate.transform.localPosition - ScriptNodeTexts.NODE_CONTINUE * transform.localScale.x + ScriptNodeTexts.NODE_START * transform.localScale.x + Vector3.down * .05f * transform.localScale.x;
        UpdateInternal(true);
        if (character == null) {
            textInfo.characterText.text = ScriptNodeTexts.NEED_CHARACTER;
            textInfo.adjectiveText.text = ScriptNodeTexts.NEED_CHOOSEN_EMOTION;
            name = ScriptNodeTexts.NEED_CHARACTER_NAME;
        } else {
            textInfo.icon.texture = chosenEmotion.Texture;
        }
    }

    public GameObject GetGameObject () {
        return this.gameObject;
    }

    public INode GetNextNode () {
        return this.nextNode;
    }

    public void SetNextNode (INode node) {
        nextNode = node as ScriptNode;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(ScriptNode))]
public class ScriptOptionNodeEditor:Editor {

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
                GUIContent arrayList = new GUIContent("EMOTION");
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

        if (GUILayout.Button("restart hash"))
        {
            node.restartHash();
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