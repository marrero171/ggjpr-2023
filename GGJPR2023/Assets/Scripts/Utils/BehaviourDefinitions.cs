using UnityEngine;
namespace Utils.AIHelpers
{
    public enum NormalBehaviour { LazeAround, WalkAround, GoToSomethngInteresting }
    public enum HungryBehaviour { ConsiderHealth, LookAround, SeekClosestMeal }
    public enum ThirstyBehaviour { ConsiderHealth, LookAround, SeekClosestMeal }
    public enum LowHealthBehavior { Continue, RunAway, Hide, CallForHelp }


    public enum OpponentSpotted { ConsiderHealth, Avoid, ApproachWithCaution, BargeInto }
    public enum OpponentCloseEnough { Act, Approach, DoNothing }
    public enum OpponentLowHealth { Attack, Approach, Leave, NotifyOthers, NotifyOpponents }


    public static class Functions
    {

        public static Vector3 RandomDirection()
        {
            int d = Random.Range(0, 4);
            Vector3 fDir = Vector3.zero;
            switch (d)
            {
                case 0:
                default:
                    break;
                case 1: //X
                    fDir = Vector3.right;
                    break;
                case 2: //Y
                    fDir = Vector3.up;
                    break;
                case 3: //Z
                    fDir = Vector3.forward;
                    break;
            }
            return fDir;

        }

        public static void PrintMessage(string msg) => MonoBehaviour.print(msg);
    }



}