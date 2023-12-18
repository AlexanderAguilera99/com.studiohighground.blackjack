
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

using UnityEngine;

public class Innard : MonoBehaviour
{

    public GameObject parent;

    [SerializeField]
    public Vector3 pos = new Vector3();

    [HideInInspector]
    [SerializeField]
    bool hasBeenPositioned;

    public void FixPos () {
        if (!hasBeenPositioned) {
            pos = transform.localPosition;
            hasBeenPositioned = true;
        } else {
            transform.localPosition = pos;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Innard))]
public class SelectParentInsteadEditor:Editor {
    void OnSceneGUI () {

        Innard innard = target as Innard;

        innard.FixPos();

    }
    private void OnEnable () {
        Innard innard = target as Innard;

        innard.FixPos();

         Innard targetGO = target as Innard;

        SceneView sceneView = EditorWindow.focusedWindow as SceneView;
        if(targetGO != null && sceneView != null)
        {
            Selection.activeGameObject = targetGO.parent;
            return;
        }
    }
}
#endif