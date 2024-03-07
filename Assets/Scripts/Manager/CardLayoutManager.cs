using System.Collections.Generic;
using UnityEngine;

public class CardLayoutManager : MonoBehaviour
{
    public bool isHorizontal;
    public float maxWidth = 7f;
    public float cardSpacing = 2f;
    [Header("Curve Layout")]
    public float maxAngle = 40f;
    public float radius = 17f;
    public Vector3 centerPoint;
    [SerializeField] private List<Vector3> cardPositions = new();
    private List<Quaternion> cardRotations = new();
    private void Awake()
    {
        centerPoint = isHorizontal ? Vector3.up * -4.5f : Vector3.up * -21.5f;
    }
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
            Calculate_Horizontal_Layout(numberOfCards);
        }
        else
        {
            Calculate_Curve_Layout(numberOfCards);
        }
    }
    private void Calculate_Curve_Layout(int numberOfCards)
    {
        var angleBetweenCards = Mathf.Min(maxAngle / (numberOfCards - 1), 7f);
        float cardAngle = (numberOfCards - 1) * angleBetweenCards / 2;
        for (int i = 0; i < numberOfCards; i++)
        {
            var pos = FanCardPosition(cardAngle - i * angleBetweenCards);
            var rotation = Quaternion.Euler(
                0, 
                0, 
                cardAngle - i * angleBetweenCards
            );
            cardPositions.Add(pos);
            cardRotations.Add(rotation);
        }
    }
    private Vector3 FanCardPosition(float angle)
    {
        return new Vector3(
            centerPoint.x - radius * Mathf.Sin(angle * Mathf.Deg2Rad), centerPoint.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad),
            0
        );
    }
    private void Calculate_Horizontal_Layout(int numberOfCards)
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
