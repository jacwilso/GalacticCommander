namespace UIWheel
{
    public class Cancel : Segment
    {
        public override void Select()
        {
            UISelectors.instance.gameObject.SetActive(false);
            ARCursor.instance.Deselect();
        }
    }
}