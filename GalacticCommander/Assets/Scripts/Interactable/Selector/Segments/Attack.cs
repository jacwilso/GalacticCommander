using UnityEngine.EventSystems;

namespace UIWheel
{
    public class Attack : Segment
    {
        public override void Select()
        {
            UISelectors.instance.AttackWheel();
        }
    }
}