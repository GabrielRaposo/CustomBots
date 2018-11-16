using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {

    public GameObject bulletPrefab;
    public int size;

    List<GameObject> pool;
    int index;
    public static BulletPool instance;

    private void Awake()
    {
        if(instance == null) 
            instance = this;
        else 
            Destroy(gameObject);
    }

    void Start ()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity, transform);
            pool.Add(bullet);
            bullet.SetActive(false);
            bullet.GetComponent<Bullet>().Init(this);
        }
	}
	
	public GameObject GetFromPool()
    {
        GameObject bullet = pool[index];
        index = (index + 1) % size;

        return bullet;
    }

    public void Return(GameObject bullet)
    {
        bullet.transform.position = transform.position;
        bullet.SetActive(false);
        bullet.transform.parent = transform;
    }
}
