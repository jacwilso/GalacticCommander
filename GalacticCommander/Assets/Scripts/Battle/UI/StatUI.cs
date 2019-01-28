using UnityEngine;
using TMPro;

public class StatUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText = null,
        hullText = null,
        shieldText = null;
    [Header("Attacked Stats")]
    [SerializeField]
    TextMeshProUGUI accuracyText = null;
    [SerializeField]
    TextMeshProUGUI shieldRangeText = null, hullRangeText = null;

    Ship ship;

    void Start()
    {
        ship = GetComponentInParent<Ship>();
        ship.DamageEvent += UpdateDisplay;

        nameText.text = ship.properties.name;
        hullText.text = ship.properties.Hull.Value.ToString("F2");
        shieldText.text = ship.properties.Shield.Value.ToString("F2");

        // gameObject.SetActive(false);
        accuracyText.transform.parent.gameObject.SetActive(false);
        shieldRangeText.transform.parent.gameObject.SetActive(false);
        hullRangeText.transform.parent.gameObject.SetActive(false);
    }

    public void DisplayAttack(float accuracy, WeaponDamageRange range)
    {
        gameObject.SetActive(true);
        accuracyText.transform.parent.gameObject.SetActive(true);
        shieldRangeText.transform.parent.gameObject.SetActive(true);
        hullRangeText.transform.parent.gameObject.SetActive(true);
        accuracyText.text = accuracy.ToString("F2");
        shieldRangeText.text = range.shieldRange.x.ToString("F2") + " - " + range.shieldRange.y.ToString("F2");
        hullRangeText.text = range.hullRange.x.ToString("F2") + " - " + range.hullRange.y.ToString("F2");
    }

    public void HideAttack()
    {
        accuracyText.transform.parent.gameObject.SetActive(false);
        shieldRangeText.transform.parent.gameObject.SetActive(false);
        hullRangeText.transform.parent.gameObject.SetActive(false);
    }

    public void UpdateDisplay()
    {
        hullText.text = ship.properties.Hull.Value.ToString("F2");
        shieldText.text = ship.properties.Shield.Value.ToString("F2");
    }
}
