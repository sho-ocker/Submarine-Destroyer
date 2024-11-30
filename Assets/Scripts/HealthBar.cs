using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarImage;
    [SerializeField] private int healthBarSize = 180;
    private Health healthComponent;
    private RectTransform healthBarRectTransform;

    private void Start()
    {
        GameObject playerShip = GameObject.FindWithTag("Player");
        if (playerShip != null)
        {
            healthComponent = playerShip.GetComponent<Health>();
        }

        if (healthBarImage != null)
        {
            healthBarRectTransform = healthBarImage.GetComponent<RectTransform>();
        }
    }

    private void Update()
    {
        if (healthComponent != null && healthBarRectTransform != null)
        {
            float healthPercentage = (float)healthComponent.GetCurrentHealth() / healthComponent.GetMaxHealth();
            Vector2 sizeDelta = healthBarRectTransform.sizeDelta;
            sizeDelta.x = healthPercentage * healthBarSize;
            healthBarRectTransform.sizeDelta = sizeDelta;
        }
    }
}
