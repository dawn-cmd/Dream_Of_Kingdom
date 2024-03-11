using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class CardDeck : MonoBehaviour
{
    public CardManager cardManager;
    public CardLayoutManager layoutManager;
    public Vector3 deckPosition;
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
        ShuffleDeck();
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
                foreach (var item in discardDeck)
                {
                    drawDeck.Add(item);
                }
                ShuffleDeck();
            }
            CardDataSO currentCardData = drawDeck[0];
            drawDeck.RemoveAt(0);
            var card = cardManager.GetCardObject().GetComponent<Card>();
            card.Init(currentCardData);
            card.transform.position = deckPosition;
            handCardObjectList.Add(card);
            var delay = i * 0.2f;
            SetCardLayout(delay);
        }
    }
    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handCardObjectList.Count; i++)
        {
            Card currentCard = handCardObjectList[i];
            CardTransform cardTransform =
                layoutManager.GetCardTransform(i, handCardObjectList.Count);

            currentCard.isAnimating = true;
            currentCard.transform.DOScale(Vector3.one, 0.5f)
                .SetDelay(delay)
                .onComplete = () =>
                {
                    currentCard.transform.DOMove(cardTransform.pos, 0.5f).onComplete = () => currentCard.isAnimating = false;
                    currentCard.transform.DORotateQuaternion(cardTransform.rotation, 0.5f);
                };
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.UpdatePositionRotation(cardTransform.pos, cardTransform.rotation);
        }
    }
    private void ShuffleDeck()
    {
        discardDeck.Clear();

        for (int i = 0; i < drawDeck.Count; i++)
        {
            CardDataSO temp = drawDeck[i];
            int randomIndex = Random.Range(i, drawDeck.Count);
            drawDeck[i] = drawDeck[randomIndex];
            drawDeck[randomIndex] = temp;
        }
    }
    private void DiscardCard(Card card)
    {
        handCardObjectList.Remove(card);
        discardDeck.Add(card.cardData);
        cardManager.DiscardCard(card.gameObject);
        SetCardLayout(0);
    }
}
