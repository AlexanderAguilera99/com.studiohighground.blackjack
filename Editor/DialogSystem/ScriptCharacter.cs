
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScriptCharacter : MonoBehaviour
{

    [SerializeField]
    public string ScriptName;

    [SerializeField]
    public string DialogBoxName;

    [SerializeField]
    public List<Emotion> emotions;
}

[System.Serializable]
public class Emotion {
    [SerializeField]
    public string emotionID;

    [SerializeField]
    public Texture Texture;
}