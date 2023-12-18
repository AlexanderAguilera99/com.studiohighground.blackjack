
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


using UnityEngine;
using System.Collections.Generic;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class NodeRoot : MonoBehaviour
{
    [SerializeField]
    public int ammountOfNodes = -11310;

    [SerializeField]
    public ScriptNode[] scriptNodes;

    public NodeContent mainnode;

    [SerializeField]
    public bool optimizedMode;
    private bool optimizedModeWas;
    public void UpdateGizmo()
    {
        if (Application.isPlaying)
            return;
        scriptNodes = GetComponentsInChildren<ScriptNode>();
        int size = scriptNodes.Length;

        List<NodeContent> nodeContentsList = new();

        Debug.Log("addedNode" + "size " + ammountOfNodes);

        ammountOfNodes = size;
        //Grab first node for conversation
        if (size > 0)
        {

            Debug.Log("processing nested nodes");
            mainnode = NodeContent.FromNode(scriptNodes[0]);
        }

        Hashtable hashT = new Hashtable();
        for (int i = 0; i < size; i++)
        {
            var node = scriptNodes[i];
            node.optimizedMode = optimizedMode;

            // Node content 
            NodeContent content = new();
            content.character = node.character;
            content.contents = node.contents;
            nodeContentsList.Add(content);

            if (!hashT.Contains(node.hash))
            {
                hashT.Add(node.hash, i);
                continue;
            }
            else
            {
                if (node.hash == "")
                    continue;
                var newhash = node.CreateMiniSHA256();
                node.FixDuplicate(node.hash, newhash);
            }
        }

    }
    private void OnDrawGizmos()
    {
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;

        int size = scriptNodes.Length;

        if (size != ammountOfNodes || optimizedMode != optimizedModeWas)
        {
            optimizedModeWas = optimizedMode;
            UpdateGizmo();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(NodeRoot))]
public class NodeRootEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NodeRoot root = target as NodeRoot;
        base.OnInspectorGUI();
    }
}
#endif
