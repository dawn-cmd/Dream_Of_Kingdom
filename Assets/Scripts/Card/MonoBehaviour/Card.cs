using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Component")]
    public SpriteRenderer cardSprite;
    public TextMeshPro costText, descriptionText, typeText, nameText;
    public CardDataSO cardData;
    private void Start()
    {
        Init(cardData);
    }
    public void Init(CardDataSO data)
    {
        cardData = data;
        cardSprite.sprite = data.cardImage;
        costText.text = data.cost.ToString();
        descriptionText.text = data.description;
        nameText.text = data.cardName;
        typeText.text = data.cardType switch
        {
            CardType.Attack => "攻击",
            CardType.Defense => "防御",
            CardType.Abilities => "能力",
            _ => "Unknown",
        };
    }
}
