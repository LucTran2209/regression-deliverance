using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image fillBar;
    public TextMeshProUGUI valueText;
    public void UpdateBar(int currentBlood, int maxBlood)
    {
        fillBar.fillAmount = (float)currentBlood / (float)maxBlood;
        valueText.text = $"{currentBlood.ToString()}/{maxBlood.ToString()}";
    }

}
