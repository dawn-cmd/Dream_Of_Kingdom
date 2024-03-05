using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float cardSpacing = 2f;
    public Vector3 centerPoint;
    [SerializeField] private List<Vector3> cardPositions = new();
    private List<Quaternion> cardRotations = new();
    public CardTransform GetCardTransform(int index, int totalCards)
    {
        CalculatePosition(totalCards, isHorizontal);
        return new CardTransform(cardPositions[index], cardRotations[index]);
    }
    private void CalculatePosition(int numberOfCards, bool horizontal)
    {
        cardPositions.Clear();
        cardRotations.Clear();
        if (horizontal)
        {
            float currentWidth = cardSpacing * (numberOfCards - 1);
            float totalWidth = Mathf.Min(maxWidth, currentWidth);
            float currentSpacing = totalWidth > 0 ? totalWidth / (numberOfCards - 1) : 0;
            for (int i = 0; i < numberOfCards; i++)
            {
                cardPositions.Add(new Vector3(-totalWidth / 2 + i * currentSpacing, centerPoint.y, 0));
                cardRotations.Add(Quaternion.identity);
            }
        }
    }
}
