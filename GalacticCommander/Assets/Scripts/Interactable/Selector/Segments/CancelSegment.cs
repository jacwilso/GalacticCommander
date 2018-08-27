namespace UIWheel
{
    public class CancelSegment : Segment
    {
        public override void Select()
        {
            UISelectors.Instance.gameObject.SetActive(false);
            ARCursor.Instance.Deselect();
        }
    }
}