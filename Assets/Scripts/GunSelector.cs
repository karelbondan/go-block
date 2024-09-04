using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelector : MonoBehaviour
{
    [SerializeField]
    private GunType Gun;
    [SerializeField]
    private Transform GunParent;
    [SerializeField]
    private List<GunScriptableObject> Guns;
    [SerializeField]
    private GameObject frontReference;
    [SerializeField]
    private PlayerManager playerManager;

    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject ActiveGun;

    [HideInInspector]
    public bool Auto;


    // Start is called before the first frame update
    void Start()
    {
        GunScriptableObject gun = Guns.Find(gun => gun.Type == Gun);
        if (gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {Gun}");
            return;
        }
        ActiveGun = gun;
        Auto = ActiveGun.Auto;
        gun.Spawn(GunParent, this, frontReference, playerManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeGun(GunType gunType)
    {
        ActiveGun.DestroyModel();
        GunScriptableObject gun = Guns.Find(gun => gun.Type == gunType);
        if (gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {gunType}");
            return;
        }
        ActiveGun = gun;
        Auto = ActiveGun.Auto;
        gun.Spawn(GunParent, this, frontReference, playerManager);
    }
}
