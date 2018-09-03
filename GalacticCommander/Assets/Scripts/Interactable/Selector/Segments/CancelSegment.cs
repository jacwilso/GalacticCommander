namespace UIWheel
{
    public class CancelSegment : Segment
    {
        public override void Select()
        {
            ActionWheel.Instance.gameObject.SetActive(false);
            ARCursor.Instance.Deselect();
        }
    }
}