using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    private float width;
    private float height;
    private void Awake() {
        width=healthBar.rectTransform.rect.width;
        height=healthBar.rectTransform.rect.height;
    }

    public void SetHealth(int health, int maxHealth) {
        healthBar.rectTransform.sizeDelta = new Vector2(width*health/maxHealth,height);
        healthText.text = health.ToString();
    }
}
