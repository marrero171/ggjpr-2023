using UnityEngine;

[CreateAssetMenu(fileName = "Throwable", menuName = "General/Throwable Info")]
public class ThrowableInfo : ScriptableObject
{
    public int damage = 1;
    public int knockback = 10;
    public int speed = 5;
}
