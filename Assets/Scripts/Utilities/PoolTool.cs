using UnityEngine;
using UnityEngine.Pool;
public class PoolTool : MonoBehaviour
{
    public GameObject objPrefab;
    private ObjectPool<GameObject> pool;
    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(objPrefab, transform),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );
        PreFillPool(10);
    }
    private void PreFillPool(int count)
    {
        var preFillList = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            preFillList[i] = pool.Get();
        }
        for (int i = 0; i < count; i++)
        {
            pool.Release(preFillList[i]);
        }
    }
    public GameObject GetObjectFromPool()
    {
        return pool.Get();
    }
    public void ReleaseObjectToPool(GameObject obj)
    {
        pool.Release(obj);
    }
}
