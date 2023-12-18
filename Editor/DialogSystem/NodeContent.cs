
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
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class NodeContent
{
    [SerializeField]
    [TextArea]
    public string contents;

    [SerializeField]
    public string hash;

    [SerializeField]
    public ScriptCharacter character;

    [SerializeField]
    public DialogType type;

    [SerializeField]
    public NodeContent parentNode;

    [SerializeField]
    public NodeContent nextNode;

    [SerializeField]
    public Card card;

    [SerializeField]
    public List<Card> cardRequirements;

    [SerializeField]
    public Emotion chosenEmotion;

    [SerializeField]
    public List<NodeContent> displayNodes;

    [SerializeField]
    public List<NodeContent> optionNodes;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NodeContent FromNode(ScriptNode node, NodeContent parentNode = null)
    {
        NodeContent content = new();
        if (node == null)
        {
            content.type = DialogType.FINAL_NODE;
            return content;
        }

        if (node.character != null)
        content.character = node.character;

        if (node.contents != null)
            content.contents = node.contents;

        if (node.hash != null)
            content.hash = node.hash;

        content.type = node.type;

        if (node.card != null)
            content.card = node.card;

        if (node.cardRequirements != null)
            content.cardRequirements = node.cardRequirements;

        if (node.chosenEmotion != null)
            content.chosenEmotion = node.chosenEmotion;

        // List ALL parents
        List<NodeContent> allParents = new();
        if (parentNode != null)
        {
            Debug.Log("Node has parent");
            allParents.Add(parentNode);
            content.parentNode = parentNode;
            NodeContent parentParent = parentNode;
            while (parentParent.parentNode != null)
            {
                parentParent = parentParent.parentNode;
                allParents.Add(parentParent.parentNode);
                if (parentNode.optionNodes != null)
                {
                    var size = parentNode.optionNodes.Count;

                    if (size > 0)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            var option = parentNode.optionNodes[i];
                            if (option == null)
                            {
                                continue;
                            }
                            allParents.Add(option);
                        }
                    }
                }
            }
        }

        if(node.nextNode != null)
        {

            // Check if parent is the same as next Node
            bool foundItself = false;
            NodeContent buckledParent = new();
            var size = allParents.Count;
            for (int i = 0; i < size; i++)
            {
                var parent = allParents[i];

                if (parent == null)
                    continue;
                if (parent.hash == null)
                    continue;
                if (node.nextNode.hash == null)
                    continue;

                if (parent.hash == node.nextNode.hash)
                {
                    buckledParent = parent;
                    foundItself = true;
                    break;
                }

            }
            if (foundItself)
            {
                content.nextNode = buckledParent;
            }
            else
            {
                content.nextNode = FromNode(node.nextNode, content);
            }
        }

        if (node.optionNodes != null)
        {
            var size = node.optionNodes.Count;
            content.optionNodes = new();

            for (int i = 0; i < size; i++)
            {
                var optionNode = node.optionNodes[i];
                if(optionNode == null)
                {
                    continue;
                }
                // Check if parent is the same as next Node
                bool foundItself = false;
                NodeContent buckledParent = new();
                var size2 = allParents.Count;
                for (int a = 0; a < size2; a++)
                {
                    var parent = allParents[a];

                    if (parent == null)
                        continue;
                    if (parent.hash == null)
                        continue;

                    if (node.nextNode == null)
                        Debug.Log("Broken golden path!!");

                    if (node.nextNode.hash == null)
                        continue;

                    if (parent.hash == optionNode.hash)
                    {
                        buckledParent = parent;
                        foundItself = true;
                        break;
                    }

                }
                if (foundItself)
                {
                    Debug.Log("Node has option BUCKLE");
                    content.optionNodes.Add(buckledParent);
                }
                else
                {
                    Debug.Log("Node has option");
                    content.optionNodes.Add(FromNode(optionNode,content));
                }

            }
        }

        content.displayNodes = new();
        if(content.nextNode != null)
        {
            content.displayNodes.Add(content.nextNode);
        }

        return content;
    }
}
