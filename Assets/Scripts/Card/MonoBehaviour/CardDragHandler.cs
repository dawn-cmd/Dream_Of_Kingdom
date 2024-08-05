using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject arrowPrefab;
    private GameObject currentArrow;
    private Card currentCard;
    private bool canMove;
    private bool canExecute;
    private CharacterBase targetCharacter;
    private void Awake()
    {
        currentCard = GetComponent<Card>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        switch (currentCard.cardData.cardType)
        {
            case CardType.Attack:
                currentArrow = Instantiate(
                    arrowPrefab,
                    transform.position,
                    Quaternion.identity
                );
                break;
            case CardType.Defense:
            case CardType.Abilities:
                canMove = true;
                break;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canMove)
        {
            if (eventData.pointerEnter == null) return;
            if (eventData.pointerEnter.tag == "Enemy")
            {
                canExecute = true;
                targetCharacter = eventData.pointerEnter.GetComponent<CharacterBase>();
                return;
            }
            canExecute = false;
            targetCharacter = null;
            return;
        }
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
        if (currentArrow != null)
        {
            Destroy(currentArrow);
        }
        if (canExecute)
        {
            //TODO: Card take effect
            return;
        }
        currentCard.ResetCardTransform();
        currentCard.isAnimating = false;
    }
}
