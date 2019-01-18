using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText = null,
        healthText = null,
        shieldText = null;
    [Header("Attacked Stats")]
    [SerializeField]
    TextMeshProUGUI accuracyText = null;
    [SerializeField]
    TextMeshProUGUI damageText = null;

    Ship ship;

    void Start()
    {
        ship = GetComponentInParent<Ship>();
        ship.DamageEvent += UpdateDisplay;

        nameText.text = ship.properties.name;
        healthText.text = ship.properties.Hull.Value.ToString("F2");
        shieldText.text = ship.properties.Shield.Value.ToString("F2");

        // gameObject.SetActive(false);
        accuracyText.transform.parent.gameObject.SetActive(false);
        damageText.transform.parent.gameObject.SetActive(false);
    }

    public void AttackDisplay(float accuracy, Vector2 damageMinMax)
    {
        gameObject.SetActive(true);
        accuracyText.transform.parent.gameObject.SetActive(true);
        damageText.transform.parent.gameObject.SetActive(true);
        accuracyText.text = accuracy.ToString("F2");
        damageText.text = damageMinMax.x.ToString("F2") + " - " + damageMinMax.y.ToString("F2");
    }

    public void AttackHide()
    {
        accuracyText.transform.parent.gameObject.SetActive(false);
        damageText.transform.parent.gameObject.SetActive(false);
    }

    public void UpdateDisplay()
    {
        healthText.text = ship.properties.Hull.Value.ToString("F2");
        shieldText.text = ship.properties.Shield.Value.ToString("F2");
    }
}
