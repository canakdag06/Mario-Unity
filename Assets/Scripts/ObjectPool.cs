using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int size = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            //obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetFromPool(Vector3 pos)
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.position = pos;
            obj.SetActive(true);
            return obj;
        }

        GameObject newObj = Instantiate(prefab, pos, Quaternion.identity);
        newObj.SetActive(true);
        return newObj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}