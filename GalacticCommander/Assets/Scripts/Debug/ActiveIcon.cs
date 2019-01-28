using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveIcon : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float distance;
    [SerializeField]
    Color emissive;

    MeshRenderer rend;
    float initialY;
    Ship ship;

    void Start() {
        ship = GetComponentInParent<Ship>();
        if (ship) {
            ship.StartTurnEvent += StartTurn;
            ship.EndTurnEvent += EndTurn;
        }
        rend = GetComponent<MeshRenderer>();
        rend.enabled = false;
        initialY = transform.localPosition.y;
    }

    void Update()
    {
        if (!rend.enabled) return;

        var t = Mathf.PingPong(speed * Time.time, 1);

        Vector3 position = transform.localPosition;
        position.y = Mathf.Lerp(0, distance, t) + initialY;
        transform.localPosition = position;

        rend.material.SetColor("_EmissionColor", emissive * Mathf.LinearToGammaSpace(t));
    }

    void StartTurn() {
        rend.enabled = true;
    }

    void EndTurn() {
        rend.enabled = false;
    }
}
