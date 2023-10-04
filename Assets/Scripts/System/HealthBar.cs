using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image current_health;
    [SerializeField] private Health health;
    private float full_health;

    private void Start()
    {
		full_health = health.current_health;
    }


	private void Update()
	{
		current_health.fillAmount = health.current_health / full_health;
	}

}
