
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

#if UNITY_EDITOR
using UnityEditor;
#endif

public class HideObject: MonoBehaviour {

    private void OnDrawGizmos () {
       // print("lol");
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(HideObject))]
public class lockEdition:Editor {
    public override void OnInspectorGUI () {

        //Selection.activeGameObject.hideFlags = HideFlags.HideInHierarchy;

        base.OnInspectorGUI();

        // TO UNLOCK
        //Selection.activeGameObject.hideFlags = ( HideFlags )0;
    }
}
#endif