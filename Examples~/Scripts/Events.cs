using UnityEngine;

namespace LuviKunG.Attributes.Example
{
    public delegate void BallFallDelegate(in Ball ball);
    public delegate void BallGoalReachDelegate(in Goal goal, in Ball ball);
}
