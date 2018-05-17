namespace UIWheel
{
    public class Move : Segment
    {
        public override void Select()
        {
            Ship ship = (Ship)ARCursor.instance.Interact;
            if (ship != null)
            {
                ship.Movement();
            }
            UISelectors.instance.gameObject.SetActive(false);
        }
    }
}