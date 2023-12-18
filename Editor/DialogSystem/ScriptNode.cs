
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


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

[System.Serializable]
public class ScriptNode : MonoBehaviour, INode
{

    //convertir en modelo

    [HideInInspector]
    public int clicks = 0;
    [HideInInspector]
    public bool newOb = true;
    [HideInInspector]
    public bool optimizedMode = false;

    [SerializeField]
    public DialogType type;

    /// <summary>
    /// Given via the previous node. takes time.
    /// </summary>
    [HideInInspector]
    [SerializeField]
    public DialogType prevNodeType;

    [SerializeField]
    public string hash = "";

    public ScriptNode nextNode;

    public List<ScriptNode> optionNodes = new List<ScriptNode>();

    [SerializeField]
    public ScriptCharacter character = new ScriptCharacter();

    [SerializeField]
    public Card card;

    [SerializeField]
    public bool repeatDialog = false;

    /// <summary>
    /// Which cards are required to access, automatic dialog, takes priority in options
    /// </summary>
    [SerializeField]
    public List<Card> cardRequirements;


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
    private Color mainColor;
    //

    string oldStatus = "   ";
    string updatedContent = "";



    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    void Start()
    {
        UpdateInternal();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;
        if (!optimizedMode)
            UpdateInternal();
        ColorStuff();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
            return;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    public void restartHash()
    {
        hash = CreateMiniSHA256();
    }

    public void ColorStuff()
    {
        Vector3 here = transform.localPosition;
        float s = transform.localScale.x;
        Gizmos.color = mainColor;
        Gizmos.DrawCube(here, ScriptNodeTexts.BASE_SIZE * s);
        Gizmos.color = new Color(1f, 1f, 1f, 1f);
        if (nextNode != null)
        {
            Vector3 start = here + ScriptNodeTexts.NODE_START * s;
            Vector3 dt = nextNode.GetGameObject().transform.localPosition + ScriptNodeTexts.NODE_CONTINUE * nextNode.transform.localScale.x;

            if (nextNode.hash == this.hash)
            {
                nextNode.restartHash();
            }

            if (type == DialogType.ACTION)
            {
                DrawArrow(start, dt, ScriptNodeColors.EXTRA_ARROW);
            }
            else
            {
                DrawArrow(start, dt, ScriptNodeColors.EXTRA_ARROW);
            }

            if (type == DialogType.CARD_ITEM)
            {
                nextNode.prevNodeType = DialogType.ACTION;
            }
            else
            {
                nextNode.prevNodeType = type;
            }

            nextNode.UpdateInternal(true);
        }
        int size = optionNodes.Count;
        for (int i = 0; i < size; i++)
        {

            if (optionNodes[i] == null)
                continue;

            var start = here + ScriptNodeTexts.NODE_START * s - optionNodes[i].transform.localScale.x * Vector3.right * (-i * 5 + 20) * .03f;
            var dt = optionNodes[i].GetGameObject().transform.localPosition + ScriptNodeTexts.NODE_CONTINUE * optionNodes[i].transform.localScale.x;
            DrawArrow(start, dt, ScriptNodeColors.DEFAULT_ARROW);

            optionNodes[i].prevNodeType = type;
            optionNodes[i].UpdateInternal(true);


            if (optionNodes[i].prevNodeType == DialogType.CARD_ITEM)
            {
                optionNodes[i].mainColor = ScriptNodeColors.P_COLOR;
                optionNodes[i].prevNodeType = type;
            }

        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    void DrawArrow(Vector3 op, Vector3 ed, Color arrowColor)
    {
        Gizmos.color = arrowColor;

        Gizmos.DrawLine(op, ed);
        float xDif = op.x - ed.x;
        float yDif = op.y - ed.y;

        float angle = -Mathf.Atan2(xDif, yDif);
        float an1 = angle - Mathf.PI * .35f;
        float an2 = angle - Mathf.PI * .65f;

        float size = .0365f;
        Vector3 ar1 = new Vector3(ed.x - Mathf.Cos(an1) * size, ed.y - Mathf.Sin(an1) * size, ed.z);
        Vector3 ar2 = new Vector3(ed.x - Mathf.Cos(an2) * size, ed.y - Mathf.Sin(an2) * size, ed.z);

        Gizmos.DrawLine(ar1, ed);
        Gizmos.DrawLine(ar2, ed);
        Gizmos.DrawLine(ar1, ar2);

    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UpdateInternal(bool force = false)
    {
        string charname = "null";

        if (character != null)
        {
            if (chosenEmotionIndex >= character.emotions.Count)
            {
                chosenEmotionIndex = 0;
            }
            charname = character.name + chosenEmotion.emotionID;
        }
        string newStatus = "" + chosenEmotionIndex + eyeTracker + contents + charname + nextNode + hash + type.ToString();

        if (oldStatus == newStatus && contents == updatedContent && !force)
        {
            return;
        }

        if (textInfo == null)
        {
            textInfo = new TextContainer();
            var yourScripts = GetComponentsInChildren<Text>();
            int size = yourScripts.Length;
            for (int i = 0; i < size; i++)
            {
                Text yourScript = yourScripts[i];

                if (yourScript == null)
                    continue;

                if (yourScript.name == "EyeTarget")
                {
                    textInfo.trackerInfoText = yourScript;
                    continue;
                }
                if (yourScript.name == "Character")
                {
                    textInfo.characterText = yourScript;
                    continue;
                }
                if (yourScript.name == "Adjective")
                {
                    textInfo.adjectiveText = yourScript;
                    continue;
                }
                if (yourScript.name == "Content")
                {
                    textInfo.contentsText = yourScript;
                    continue;
                }
                if (yourScript.name == "Hash")
                {
                    textInfo.hashText = yourScript;
                    continue;
                }
                if (yourScript.name == "Type")
                {
                    textInfo.dialogType = yourScript;
                    continue;
                }
            }


        }
        var images = GetComponentsInChildren<RawImage>();
        int imgSize = images.Length;
        for (int i = 0; i < imgSize; i++)
        {
            RawImage image = images[i];
            if (image.name == "icon")
            {
                textInfo.icon = image;
            }
        }
        if (hash == "" && character != null)
        {
            print("hash was empty!");
            hash = CreateMiniSHA256();
        }

        if (textInfo.icon != null)
            textInfo.icon.gameObject.SetActive(true);

        textInfo.hashText.text = hash;
        if (textInfo.dialogType != null)
        {
            switch (type)
            {
                case DialogType.ACTION:
                    mainColor = ScriptNodeColors.O_COLOR;
                    textInfo.dialogType.text = ScriptNodeTexts.DIALOG;

                    if (textInfo != null)
                    {
                        if (textInfo.icon != null)
                        {
                            textInfo.icon.gameObject.SetActive(true);
                        }
                    }
                    if (prevNodeType == DialogType.OPTION_BRANCH)
                    {
                        mainColor = ScriptNodeColors.C_COLOR;
                        textInfo.dialogType.text = ScriptNodeTexts.DIALOG_BRANCH;
                    }

                    break;

                case DialogType.OPTION_BRANCH:
                    mainColor = ScriptNodeColors.D_COLOR;
                    textInfo.dialogType.text = ScriptNodeTexts.OPTION;
                    break;

                case DialogType.CARD_ITEM:
                    mainColor = ScriptNodeColors.G_COLOR;
                    textInfo.dialogType.text = ScriptNodeTexts.CARD_BRANCH;
                    break;

                case DialogType.FINAL_NODE:
                    mainColor = ScriptNodeColors.E_COLOR;
                    textInfo.dialogType.text = ScriptNodeTexts.FINAL;

                    textInfo.characterText.text = ScriptNodeTexts.FINALE_CHAR;
                    break;

                case DialogType.START_NODE:
                    mainColor = ScriptNodeColors.G_COLOR;
                    textInfo.dialogType.text = ScriptNodeTexts.START_NODE;

                    textInfo.characterText.text = "LINKER ID:";

                    break;
            }

            if (type == DialogType.START_NODE)
            {
                textInfo.adjectiveText.text = "";
                textInfo.trackerInfoText.text = "";

                return;
            }

            if (type == DialogType.FINAL_NODE)
            {
                textInfo.adjectiveText.text = "";
                textInfo.contentsText.text = "";
                textInfo.trackerInfoText.text = "";

                character = null;
                if (textInfo.icon != null)
                {
                    textInfo.icon.texture = null;

                }
                return;
            }


        }
        if (prevNodeType == DialogType.CARD_ITEM)
        {
            mainColor = ScriptNodeColors.P_COLOR;
            textInfo.dialogType.text = ScriptNodeTexts.CARD_ITEM;
            textInfo.characterText.text = card.name;

            textInfo.contentsText.text = card.information;
            textInfo.adjectiveText.text = "Card";

            if (textInfo.icon != null)
            {
                if (card != null)
                    if (card != null)
                        textInfo.icon.texture = card.refferencetexture;
            }
            return;
        }
        else
        if (prevNodeType == DialogType.OPTION_BRANCH)
        {

            textInfo.characterText.text = "";
            textInfo.adjectiveText.text = ScriptNodeTexts.DIALOG_BRANCH;
            character = null;

            if (textInfo.icon != null)
            {
                textInfo.icon.texture = null;
                textInfo.icon.gameObject.SetActive(false);
            }
        }
        else if (character == null)
        {
            textInfo.characterText.text = ScriptNodeTexts.NEED_CHARACTER;
            textInfo.adjectiveText.text = ScriptNodeTexts.NEED_CHOOSEN_EMOTION;
            name = ScriptNodeTexts.NEED_CHARACTER_NAME;

            if (textInfo.icon != null)
            {
                textInfo.icon.gameObject.SetActive(false);
            }
        }
        else
        {
            if (textInfo.icon != null)
                textInfo.icon.texture = chosenEmotion.Texture;
            string contentForTitle = textInfo.contentsText.text;
            int lengthMax = textInfo.contentsText.text.Length;
            if (lengthMax > 13)
            {
                contentForTitle = contentForTitle.Substring(0, 10) + "..";
            }
            name = textInfo.characterText.text + ": " + contentForTitle;


            textInfo.characterText.text = character.ScriptName;


            if (cardRequirements != null)
            {
                var size = cardRequirements.Count;
                if (size > 0)
                {

                    textInfo.characterText.text = character.ScriptName + " [Lock]";
                    string allCards = "";
                    for (int i = 0; i < size; i++)
                    {
                        if(i == 0)
                        {
                            allCards += cardRequirements[i].UDID;
                        }
                        else
                        {
                            allCards += ", "+cardRequirements[i].UDID;
                        }

                    }
                    textInfo.adjectiveText.text = allCards;
                }
                else
                {
                    textInfo.characterText.text = character.ScriptName;
                    textInfo.adjectiveText.text = ScriptNodeTexts.NEED_EMOTION_CHARACTER;
                }
            }
        
        }

        if (card == null)
        {
            textInfo.trackerInfoText.text = ScriptNodeTexts.NULL_TARGET;
        }
        else
        {
            textInfo.trackerInfoText.text = card.UDID;
        }

        if (contents == "")
        {
            textInfo.contentsText.text = ScriptNodeTexts.NEED_DIALOG;
            updatedContent = "";
        }
        else
        {
            textInfo.contentsText.text = contents;
            updatedContent = textInfo.contentsText.text;
        }

        if (oldStatus == newStatus)
        {
        }
        else
        {
            oldStatus = newStatus;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]

    public string CreateMiniSHA256()
    {
        string data = GetHashCode() + "" + UnityEngine.Random.Range(0, 1000000000) + transform.GetHashCode();
        string rawHash = CryptoService.Sha256(data.GetHashCode().ToString());
        return rawHash.Substring(0, 10);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void NextNode()
    {
        var nodes = this.transform.parent.gameObject.GetComponentsInChildren<INode>();
        int size = nodes.Length;
        for (int i = 0; i < size; i++)
        {
            var node = nodes[i];
            if (node == this && i + 1 < size)
            {
                nextNode = nodes[i + 1] as ScriptNode;
                break;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void PrevNode()
    {
        var nodes = this.transform.parent.gameObject.GetComponentsInChildren<INode>();
        int size = nodes.Length;
        print("grabbing nodes" + size);
        for (int i = 0; i < size; i++)
        {
            var node = nodes[i];
            if (node == this && i > 0)
            {
                nodes[i - 1].SetNextNode(this);
                break;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void FixDuplicate(string hash, string newHash)
    {
        var nodes = this.transform.parent.gameObject.GetComponentsInChildren<ScriptNode>();
        int size = nodes.Length;
        ScriptNode duplicate = new ScriptNode();
        for (int i = 0; i < size; i++)
        {
            var node = nodes[i];
            if (node != this)
                if (node.hash == hash && node != this)
                {
                    print("DUPLICATE FOUND " + i);
                    duplicate = node;
                    duplicate.nextNode = this;
                    break;
                }
        }

        if (duplicate == null)
            return;

        this.hash = newHash;
        this.transform.localPosition = duplicate.transform.localPosition - ScriptNodeTexts.NODE_CONTINUE * transform.localScale.x + ScriptNodeTexts.NODE_START * transform.localScale.x + Vector3.down * .05f * transform.localScale.x;
        UpdateInternal(true);
        if (character == null)
        {
            textInfo.characterText.text = ScriptNodeTexts.NEED_CHARACTER;
            textInfo.adjectiveText.text = ScriptNodeTexts.NEED_CHOOSEN_EMOTION;
            name = ScriptNodeTexts.NEED_CHARACTER_NAME;
        }
        else
        {
            textInfo.icon.texture = chosenEmotion.Texture;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public INode GetNextNode()
    {
        return this.nextNode;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetNextNode(INode node)
    {
        nextNode = node as ScriptNode;
    }
}