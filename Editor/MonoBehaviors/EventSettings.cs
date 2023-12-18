
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


using System.Runtime.CompilerServices;
using UnityEngine;


[SerializeField]
public class EventSettings : MonoBehaviour
{
    [SerializeField]
    public string BlackjackScene;

    [SerializeField]
    public NodeContent[] conversations;

    [SerializeField]
    public SaveManager saveManager;


    // DELETE THIS EXAMPLE

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    NodeContent GetConversation()
    {
        NodeContent currentDialog = conversations[0];

        string dialog = currentDialog.contents;

        if (currentDialog.nextNode != null)
        {
            currentDialog = currentDialog.nextNode;

            if (currentDialog.type == DialogType.OPTION_BRANCH)
            {
                //OPTIONS currentDialog.optionNodes
            }

            if (currentDialog.type == DialogType.CARD_GATE)
            {
                // Posible scenarios currentDialog.optionNodes
            }

            if (currentDialog.type == DialogType.CARD_ITEM)
            {
                // Posible uso de ITEMS currentDialog.optionNodes
            }

            if (currentDialog.type == DialogType.ACTION)
            {
                //GIVE DIALOG
            }

        }
        return currentDialog;
    }
}
