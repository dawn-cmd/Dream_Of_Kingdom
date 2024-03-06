using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// The CardManager class manages the card library for the game.
/// </summary>
public class CardManager : MonoBehaviour
{
    public PoolTool poolTool;
    public List<CardDataSO> cardDataList;

    [Header("Card Library")]
    public CardLibrarySO newGameCardLibrary;
    public CardLibrarySO currentLibrary;

    private void Awake()
    {
        InitializeCardDataList();
        foreach (var item in newGameCardLibrary.cardLibraryList)
        {
            currentLibrary.cardLibraryList.Add(item);
        }
    }
    private void OnDisable() {
        currentLibrary.cardLibraryList.Clear();
    }

    public GameObject GetCardObject()
    {
        var cardObj = poolTool.GetObjectFromPool();
        cardObj.transform.localScale = Vector3.zero;
        return cardObj;
    }

    private void InitializeCardDataList()
    {
        Addressables.LoadAssetsAsync<CardDataSO>("CardData", null).Completed += OnCardDataLoaded;
    }

    private void OnCardDataLoaded(AsyncOperationHandle<IList<CardDataSO>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            cardDataList = new List<CardDataSO>(handle.Result);
        }
        else
        {
            Debug.LogError("Failed to load card data");
        }
    }

    /// <summary>
    /// Releases a card object back to the object pool.
    /// </summary>
    /// <param name="card">The card object to be discarded.</param>
    public void DiscardCard(GameObject card)
    {
        poolTool.ReleaseObjectToPool(card);
    }
}
