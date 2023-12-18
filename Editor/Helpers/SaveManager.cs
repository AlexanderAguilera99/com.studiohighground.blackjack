
/* Copyright (C) Fernando Holguin Weber, and Studio High Ground - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential.
 * Written by Fernando Holguin <fernando@studiohighground.com>
 * 
 * UNDER NO CIRCUMSTANCES IS Fernando Holguin Weber, OR Studio High Ground, ITS PROGRAM DEVELOPERS OR SUPPLIERS LIABLE FOR ANY OF THE FOLLOWING, EVEN IF INFORMED OF THEIR POSSIBILITY: 
 * LOSS OF, OR DAMAGE TO, DATA;
 * DIRECT, SPECIAL, INCIDENTAL, OR INDIRECT DAMAGES, OR FOR ANY ECONOMIC CONSEQUENTIAL DAMAGES; OR
 * LOST PROFITS, BUSINESS, REVENUE, GOODWILL, OR ANTICIPATED SAVINGS.
 */



using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[SerializeField]
public class SaveManager : MonoBehaviour
{

    [SerializeField]
    public bool useFakeUser = true;

    [SerializeField]
    public UserData userStart;

    [SerializeField]
    public UserData fakeUser;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InitializeBlackJackSaveData(SaveManager currentSave)
    {
        bool justInitialized = DeckCommands.InitializeDeckAndReturnTrue();
        if (!justInitialized)
            return;
        UserData data;
        if (currentSave.useFakeUser)
        {
            data = currentSave.fakeUser;
        }
        else
        {
            data = currentSave.userStart;
        }
        var cardsRaw = data.cards;
        var size = data.cards.Length;
        for(int i = 0;i< size; i++)
        {
            var currentCard = cardsRaw[i];
            var id = currentCard.UDID;
            DeckCommands.InsertCardToDeck(id);
        }
    }


}

