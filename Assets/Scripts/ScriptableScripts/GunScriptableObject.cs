using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunScriptableObject : ScriptableObject
{
    public GunType Type;
    public string Name;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;
    public float weaponRotBob;
    public float weaponBob;
    public bool Auto;

    public ShootConfigScriptableObject ShootConfig;
    public TrailConfigScriptableObject TrailConfig;

    private PlayerManager playerManager;
    private MonoBehaviour ActiveMonoBehaviour;
    private GameObject Model;
    private float LastShootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;

    public void Shoot()
    {
        if (Time.time > ShootConfig.FireRate + LastShootTime)
        {
            playerManager.PlayGunShot();
            LastShootTime = Time.time;
            ShootSystem.Play();
            ActiveMonoBehaviour.StartCoroutine(AnimateWeapon(ShootConfig.FireRate));
            Vector3 shootDirection = ShootSystem.transform.forward
                + new Vector3(
                        Random.Range(
                            -ShootConfig.Spread.x,
                            ShootConfig.Spread.x
                            ),
                        Random.Range(
                            -ShootConfig.Spread.y,
                            ShootConfig.Spread.y
                            ),
                        Random.Range(
                            -ShootConfig.Spread.z,
                            ShootConfig.Spread.z
                            )
                    );
            shootDirection.Normalize();

            // this is when an object is hit
            if (Physics.Raycast(
                    ShootSystem.transform.position,
                    shootDirection, 
                    out RaycastHit hit,
                    float.MaxValue,
                    ShootConfig.HitMask
                ))
            {
                ActiveMonoBehaviour.StartCoroutine(
                        PlayTrail(
                            ShootSystem.transform.position,
                            hit.point,
                            hit
                        )
                );
                playerManager.handleShoot(hit, ShootConfig.Damage);
            }

            // this is when an object misses
            else
            {
                RaycastHit temp = new RaycastHit();
                ActiveMonoBehaviour.StartCoroutine(
                        PlayTrail(
                            ShootSystem.transform.position,
                            ShootSystem.transform.position + (shootDirection * TrailConfig.MissDistance),
                            temp
                        )
                );
            }
        }
    }

    private IEnumerator AnimateWeapon(float animationLength)
    {
        float firstFrameTime = Time.time;
        float currentDuration = 0;
        Transform model = Model.transform;
        while (currentDuration <= animationLength)
        {
            currentDuration = Time.time - firstFrameTime;
            float lerpPercentFirstHalf = Mathf.Clamp01(1 - (currentDuration / animationLength / 2));
            float lerpPercentSecondHalf = Mathf.Clamp01(1 - ((currentDuration - 1) / animationLength / 2));
            // first half of the animation
            if (Model != null)
            {
                if (currentDuration <= animationLength / 2)
                {
                    // position
                    Model.transform.localPosition = Vector3.Lerp(
                        model.localPosition + (model.localPosition - SpawnPoint),
                        new Vector3(
                            model.localPosition.x + (model.localPosition.x - SpawnPoint.x),
                            model.localPosition.y + (model.localPosition.y - SpawnPoint.y),
                            model.localPosition.z + (model.localPosition.z - SpawnPoint.z) - weaponBob),
                        lerpPercentFirstHalf
                    );

                    // rotation
                    Model.transform.localRotation = Quaternion.Lerp(
                        Quaternion.Euler(model.localRotation.eulerAngles + (model.localRotation.eulerAngles - SpawnRotation)),
                        Quaternion.Euler(new Vector3(
                            Model.transform.localRotation.x + (Model.transform.localRotation.x - SpawnPoint.x) - weaponRotBob,
                            Model.transform.localRotation.y + (Model.transform.localRotation.y - SpawnPoint.y),
                            Model.transform.localRotation.z + (Model.transform.localRotation.z - SpawnPoint.z)
                        )),
                        lerpPercentFirstHalf
                    );
                }
                // second half of the animation
                else
                {
                    // position
                    Model.transform.localPosition = Vector3.Lerp(
                        new Vector3(
                            model.localPosition.x + (model.localPosition.x - SpawnPoint.x),
                            model.localPosition.y + (model.localPosition.y - SpawnPoint.y),
                            model.localPosition.z + (model.localPosition.z - SpawnPoint.z) - weaponBob),
                        model.localPosition + (model.localPosition - SpawnPoint),
                        lerpPercentSecondHalf
                    );

                    // rotation
                    Model.transform.localRotation = Quaternion.Lerp(
                        Quaternion.Euler(new Vector3(
                            Model.transform.localRotation.x + (Model.transform.localRotation.x - SpawnPoint.x) - weaponRotBob,
                            Model.transform.localRotation.y + (Model.transform.localRotation.y - SpawnPoint.y),
                            Model.transform.localRotation.z + (Model.transform.localRotation.z - SpawnPoint.z)
                        )),
                        Quaternion.Euler(model.localRotation.eulerAngles + (model.localRotation.eulerAngles - SpawnRotation)),
                        lerpPercentSecondHalf
                    );
                }
            }


            yield return null;
        }
    }

    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit hit)
    {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null; // this is done to avoid the get startpoint function to be assigned with the previous particle value

        instance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
            );
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        // this is for impact (later)
        //if (hit.collider != null)
        //{
        //    SurfaceManager.instance.HandleImpact(
        //            hit.transform.gameObject,
        //            EndPoint,
        //            hit.normal,
        //            ImpactType,
        //            0
        //        );
        //}

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }

    public void Spawn(Transform Parent, MonoBehaviour ActiveMonoBehaviour, GameObject frontReference, PlayerManager script)
    {
        this.ActiveMonoBehaviour = ActiveMonoBehaviour;
        LastShootTime = 0;
        TrailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(Parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);
        Model.GetComponent<WeaponClippingPrevention>().setFrontReference(frontReference);
        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
        playerManager = script;
    }

    public void DestroyModel()
    {
        if (Model != null)
        {
            Destroy(Model);
        }
    }

    private TrailRenderer CreateTrail ()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        return trail;
    }
}
