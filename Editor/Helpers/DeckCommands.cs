
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// COMMANDS in AE's Engine are Static Function lists that affect the game holistically
/// </summary>
class DeckCommands {
    private static HashSet<string> deck;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool InitializeDeckAndReturnTrue()
    {
        if (deck != null)
            return false;

        deck = new();
        return true;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InsertCardToDeckFromNode(NodeContent card)
    {
        if (deck == null)
            deck = new();
        deck.Add(card.card.UDID);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InsertCardToDeckFromNode(ScriptNode card)
    {
        if (deck == null)
            deck = new();
        deck.Add(card.card.UDID);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InsertCardToDeck(Card card)
    {
        if (deck == null)
            deck = new();
        deck.Add(card.UDID);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InsertCardToDeck(String card)
    {
        if (deck == null)
            deck = new();
        deck.Add(card);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RemoveCardFromDeck(Card card, bool deleteCardGroup = true)
    {
        if (deck == null)
            return;

        deck.Remove(card.UDID);
        var cardsInCardSize = card.additionalCardsGiven.Count;

        if (deleteCardGroup)
        {
            for (int i = 0; i < cardsInCardSize; i++)
            {
                var currentCardInGroup = card.additionalCardsGiven[i];
                if (deck.Contains(currentCardInGroup.UDID))
                {
                    // contains
                    deck.Remove(currentCardInGroup.UDID);
                }
            }
        }
      
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ClearDeck()
    {
        deck.Clear();
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasCard(ScriptNode node)
    {
        if (deck == null)
            return false;

        bool hasAllCards = true;

        var cardsInCardSize = node.cardRequirements.Count;

        if (cardsInCardSize == 0)
            return hasAllCards;

        for (int i = 0; i < cardsInCardSize; i++)
        {

            var currentCardInGroup = node.cardRequirements[i];

            if (currentCardInGroup == null)
                continue;

            if (deck.Contains(currentCardInGroup.UDID))
            {
                // contains
            }
            else
            {
                return false;
            }
        }

        return hasAllCards;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasCard(NodeContent node)
    {
        if (deck == null)
            return false;

        bool hasAllCards = true;

        var cardsInCardSize = node.cardRequirements.Count;

        if (cardsInCardSize == 0)
            return hasAllCards;

        for (int i = 0; i < cardsInCardSize; i++)
        {

            var currentCardInGroup = node.cardRequirements[i];

            if (currentCardInGroup == null)
                continue;

            if (deck.Contains(currentCardInGroup.UDID))
            {
                // contains
            }
            else
            {
                return false;
            }
        }

        return hasAllCards;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasCard (Card card)
    {
        if (deck == null)
            return false;

        bool hasAllCards = true;

        // Elimination from non existence. (lighter). unless else is proven non lighter.

        var cardUDID = card.UDID;
        if (deck.Contains(cardUDID))
        {
            // contains
        }
        else
        {
            return false;
        }

        var cardsInCardSize = card.additionalCardsGiven.Count;

        if(cardsInCardSize == 0)
            return hasAllCards;

        for (int i = 0; i < cardsInCardSize; i++)
        {
            var currentCardInGroup = card.additionalCardsGiven[i];
            if (deck.Contains(currentCardInGroup.UDID))
            {
                // contains
            }
            else
            {
                return false;
            }
        }

        return hasAllCards;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool HasCardIgnoresGroup(Card card)
    {
        return deck.Contains(card.UDID);
    }
}