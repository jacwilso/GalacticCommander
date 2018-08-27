using UnityEngine;

namespace UIWheel
{
    public class MoveSegment : Segment
    {
        [SerializeField]
        private GameObject confirmCancelUI;

        public override void Select()
        {
            PlayerShip ship = (PlayerShip)ARCursor.Instance.Selected;
            if (ship != null)
            {
                ship.Movement();
            }
            confirmCancelUI.SetActive(true);
            UISelectors.Instance.gameObject.SetActive(false);
        }
    }
}