
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

public static class ScriptNodeTexts {

    public const string NEED_CHARACTER = "<TEXT>";
    public const string FINALE_CHAR = "(END)";

    public const string NEED_CHARACTER_NAME = "[NODE: ADD CHAR]";
    public const string NEED_DIALOG = "(...)";
    public const string NULL_TARGET = "-";
    public const string NEED_CHOOSEN_EMOTION = "";
    public const string NEED_EMOTION_CHARACTER = "-";

    public const string
        DIALOG = "ACTION",
        OPTION = "OPTION BRANCH",
        START_NODE = "START NODE",
        DIALOG_BRANCH = "OPTION",
        CARD_ITEM = "ITEM CARD",
        FINAL = "FINALE",
        START = "STARTED",
        CARD_BRANCH = "CARD BRANCH";


    public const float NODE_SIZE = .01f;
    public static readonly Vector3
    /**/    BASE_SIZE = new Vector3(1.3f, .4f, .02f),
        NODE_START = new Vector3(.06f, -.214f, 0),
        NODE_CONTINUE = new Vector3(-.16f, .214f, 0);

}


public static class ScriptNodeColors {
    public static Color G_COLOR = new Color(.31f, .8f, .2f, .15f);
    
    public static Color L_COLOR = new Color(.31f, .31f, .31f, .15f);

    public static Color C_COLOR = new Color(.3f, .3f, .9f, .15f);


    public static Color O_COLOR = new Color(1f, 1f, .3f, .3f);
    public static Color D_COLOR = new Color(.7f, .4f, .2f, .3f);

    public static Color P_COLOR = new Color(.8f, .2f, .8f, .15f);
    public static Color E_COLOR = new Color(1.0f, .0f, 0f, .3f);

    public static Color DEFAULT_ARROW = new Color(1f, 1f, 1f, 1f);
    
    public static Color EXTRA_ARROW = new Color(1f, 1f, 0f, 1f);

}