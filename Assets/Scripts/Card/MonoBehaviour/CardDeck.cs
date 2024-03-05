using System.Collections.Generic;
using UnityEngine;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager layoutManager;
    private List<CardDataSO> drawDeck = new();
    private List<CardDataSO> discardDeck = new();
    private List<Card> handCardObjectList = new();
    private void Start()
    {
        InitializeDeck(); // For Testing
        DrawCard(4); // For Testing
    }
    public void InitializeDeck()
    {
        drawDeck.Clear();
        discardDeck.Clear();
        foreach (var entry in cardManager.currentLibrary.cardLibraryList)
        {
            for (int i = 0; i < entry.amount; i++)
            {
                drawDeck.Add(entry.cardData);
            }
        }
        //TODO: Shuffle the draw deck
    }
    [ContextMenu("TestDrawCard")]
    public void TestDrawCard()
    {
        DrawCard(1);
    }
    public void DrawCard(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (drawDeck.Count == 0)
            {
                //TODO: Shuffle the discard deck and move it to the draw deck
            }
            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            var card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardData);
            handCardObjectList.Add(card);
            SetCardLayout();
        }
    }
    private void SetCardLayout()
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard = handCardObjectList[i];
            CardTransform cardTransform = layoutManager.GetCardTransform(i, handCardObjectList.Count);
            currentCard.transform.SetPositionAndRotation(cardTransform.pos, cardTransform.rotation);
        }
    }
}
