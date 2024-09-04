using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Config", order = 1)]
public class ShootConfigScriptableObject : ScriptableObject
{
    public LayerMask HitMask;
    public Vector3 Spread = new Vector3(0.1f, 0.1f, 0.1f);
    public float FireRate = 0.25f;
    public float Damage;
}