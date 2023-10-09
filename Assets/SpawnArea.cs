using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] private GameObject Wall;
    [SerializeField] private List<GameObject> Enemies;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;

    // Update is called once per frame
    void Update()
    {
        if(Enemies.Count <= 0) {
            OpenWall();
        }
    }

	private void OpenWall()
	{
		Wall.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D trig)
	{
		if(trig.tag == "Player")
        {
            Wall.SetActive(true);
        }
        if(trig.tag == "Monster")
        {
            var behavior = trig.GetComponent<EnemyBehavior>();

			behavior.leftLimit = leftLimit;
			behavior.rightLimit = rightLimit;
			behavior.SelectTarget();
			Enemies.Add(trig.gameObject);
        }
	}

	private void OnTriggerExit2D(Collider2D trig)
	{
		if (trig.tag == "Monster")
		{
			Enemies.Remove(trig.gameObject);
		}
	}
}
