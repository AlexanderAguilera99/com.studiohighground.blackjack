using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TextContainer {

    [SerializeField]
    public Text contentsText;

    [SerializeField]
    public Text characterText;

    [SerializeField]
    public Text adjectiveText;

    [SerializeField]
    public Text trackerInfoText;

    [SerializeField]
    public Text dialogType;

    [SerializeField]
    public Text hashText;

    [SerializeField]
    public RawImage icon;
}

[SerializeField]
public enum DialogType {
    ACTION = 0,
    OPTION_BRANCH = 1,
    START_NODE = 2,
    FINAL_NODE = 3,
    CARD_ITEM = 4,
    CARD_GATE = 5 //No definition yet
}


public class TextContainerName {
    public const string
         //
    CONTENTS = "Content",
    CHARACTER = "Character",
    ADJECTIVE = "Adjective",
    TRACKER_INFO = "EyeTarget",
    TYPE = "Type";
}
