using UnityEngine;

namespace UIWheel
{
    public class MoveSegment : Segment
    {
        [SerializeField]
        private GameObject confirmCancelUI;

        private PlayerShip player;

        public override void Select()
        {
            player = (PlayerShip)ARCursor.Instance.Selected;
            player?.SelectMovement();
            ARCursor.Instance.DeselectEvent += Display;
            UISelectors.Instance.gameObject.SetActive(false);
        }

        public void ConfirmMove()
        {
            player?.ConfirmMovement();
            Hide();
        }

        public void CancelMove()
        {
            player?.CancelMovement();
            Hide();
        }

        public void Display()
        {
            confirmCancelUI.SetActive(true);
        }

        public void Hide()
        {
            player = null;
            confirmCancelUI.SetActive(false);
            ARCursor.Instance.DeselectEvent -= Display;
        }
    }
}