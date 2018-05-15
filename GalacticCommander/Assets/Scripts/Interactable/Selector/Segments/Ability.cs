using UnityEngine.EventSystems;

namespace UIWheel
{
    public class Ability : Segment
    {
        public override void Select()
        {
            UISelectors.instance.AttackWheel();
        }
    }
}