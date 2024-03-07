using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Card currentCard;
    private bool canMove;
    private bool canExecute;
    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        switch (currentCard.cardData.cardType)
        {
            case CardType.Attack:
                break;
            case CardType.Defense:
            case CardType.Abilities:
                canMove = true;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canMove) return;
        currentCard.isAnimating = true;
        Vector3 screenPos = new(
            Input.mousePosition.x,
            Input.mousePosition.y,
            10
        );
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        currentCard.transform.position = worldPos;
        canExecute = worldPos.y > 1f;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canExecute)
        {
            //TODO: Card take effect
            return;
        }
        currentCard.ResetCardTransform();
        currentCard.isAnimating = false;
    }
}
