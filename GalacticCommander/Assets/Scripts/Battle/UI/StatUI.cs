using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameText, healthText, shieldText;
    [Header("Attacked Stats"), SerializeField]
    private TextMeshProUGUI accuracyText;
    [SerializeField]
    private TextMeshProUGUI damageText;

    private Ship ship;

    private void Start()
    {
        ship = GetComponentInParent<Ship>();
        ship.DamageEvent += Damaged;

        nameText.text = ship.properties.name;
        healthText.text = ship.properties.Health.value.ToString();
        shieldText.text = ship.properties.ShieldStrength.value.ToString();

        accuracyText.transform.parent.gameObject.SetActive(false);
    }

    public void Damaged()
    {
        healthText.text = ship.properties.Health.value.ToString();
        shieldText.text = ship.properties.ShieldStrength.value.ToString();
    }

    public void Display(float accuracy, Vector2 damage)
    {
        accuracyText.transform.parent.gameObject.SetActive(true);
        accuracyText.text = accuracy.ToString();
        damageText.text = damage.x + " - " + damage.y;
    }
}
