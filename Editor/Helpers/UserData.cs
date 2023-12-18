
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
using UnityEngine;

[SerializeField]
public class UserData : MonoBehaviour
{
    [SerializeField]
    public Card[] cards;

    [SerializeField]
    public SaveSpot saveSpot;

    private static HashSet<string> deck;

}
