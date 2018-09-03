using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIWheel
{
    public class ActionWheel : MonoBehaviour, IInteractable
    {
        public static ActionWheel Instance
        {
            get { return instance; }
        }

        [SerializeField, MinMaxValue(0.01f, 10f)]
        private Vector2 scaleBounds;

        private static ActionWheel instance;

        private Camera cam;
        private PlayerShip ship;
        private Segmenter segmenter;

        private void Awake()
        {
            if (instance != null)
                Debug.LogError("Multiple action selectors");
            instance = this;
        }

        private void Start()
        {
            gameObject.SetActive(false);
            cam = Camera.main;
            segmenter = GetComponentInChildren<Segmenter>();
        }

        private void Update()
        {
            float scale = Vector3.SqrMagnitude(cam.transform.position - transform.position);
            scale = Mathf.Min(Mathf.Max(scaleBounds.x, scale), scaleBounds.y);
            transform.localScale = scale * Vector3.one;
        }

        public void Activate(PlayerShip ship, ShipProperties properties)
        {
            this.ship = ship;
            gameObject.SetActive(true);
            segmenter.SetSegments(properties.GetIcons());
        }

        public void Select()
        {
            gameObject.SetActive(false);
            float angle = 2f * Mathf.PI / segmenter.Segments;
            Vector2 inputPos = ARCursor.Instance.InputPosition;
            inputPos -= 0.5f * new Vector2(Screen.width, Screen.height);
            float inputAngle = Mathf.Atan2(inputPos.y, inputPos.x) - Mathf.PI;
            int segment = (int)(-inputAngle / angle);
            ship.Action(segment);
        }

        public void Deselect()
        {
            gameObject.SetActive(false);
        }
    }
}
