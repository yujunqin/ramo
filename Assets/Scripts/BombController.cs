using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    
    public float initSpeed = 15.0f;
    public GameObject TrajactoryPoint;
    public GameObject ExplosionPrefab;
    public int TrajectoryPointNum = 70;
    public float timeIntvl = 0.05f;
    public Vector3 direction = Vector2.up;
    public Vector3 offset = new Vector3(0.05f, 0.0f, 0.0f);
    public float swellFactor = 5.0f;
    public int PlayerID = 1;
    private GameObject[] TrajactoryPoints;
    private GameObject explosion;
    private bool isThrown = false;
    private bool isExploded = false;
    private Rigidbody rb;
    private SphereCollider scollider;
    private float exploseRadius;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        scollider = GetComponent<SphereCollider>();
        rb.isKinematic = true;
        exploseRadius = scollider.radius * swellFactor;
        TrajactoryPoints = new GameObject[TrajectoryPointNum];
        for (int i = 0; i < TrajectoryPointNum; i++) {
            TrajactoryPoints[i] = Instantiate(TrajactoryPoint, transform.position, Quaternion.identity);
            TrajactoryPoints[i].transform.SetParent(transform);
        }
        EventBus.Publish<BombEvent>(new BombEvent(false));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isThrown) { // before throw
            // if (RightPressed())
            //     direction.x += 0.02f;
            // else if (LeftPressed())
            //     direction.x -= 0.02f;
            // if (ShootPressed()) {
            //     ThrowBomb();
            // }
            for (int i = 0; i < TrajectoryPointNum; i++)
                TrajactoryPoints[i].transform.position = Trajectory(timeIntvl*i);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (isThrown) {
            if (other.gameObject.CompareTag("Branch")) {
                if (other.GetComponent<BranchController>().GetPlayerID() != PlayerID) {
                    other.gameObject.GetComponent<BranchController>().Damage(1000);
                    if (!isExploded)
                        StartCoroutine(Explode());
                }
            }
            if (other.gameObject.CompareTag("Wall") && !isExploded)
                StartCoroutine(Explode());
        }
    }


    public void ThrowBomb()
    {
        rb.isKinematic = false;
        isThrown = true;
        for (int i = 0; i < TrajectoryPointNum; i++)
            Destroy(TrajactoryPoints[i]);
        rb.velocity = initSpeed * direction;
    }

    Vector3 Trajectory(float time)
    {
        // the position after time t
        return initSpeed * direction * time + 0.5f * Physics.gravity * time * time + transform.position + offset;
    }

    IEnumerator Explode()
    {
        isExploded = true;
        scollider.radius = exploseRadius;
        //rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        explosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        explosion.transform.SetParent(transform);
        explosion.transform.localScale = new Vector3(7.0f, 7.0f, 1.0f);
        yield return new WaitForSeconds(1.0f);
        EventBus.Publish<BombEvent>(new BombEvent(true));
        Destroy(gameObject);
    }


    bool LeftPressed() 
    {
        return (PlayerID == 1 && Input.GetKeyDown(KeyCode.B)) || (PlayerID == 2 && Input.GetKeyDown(KeyCode.Y));
    }

    bool RightPressed() 
    {
        return (PlayerID == 1 && Input.GetKeyDown(KeyCode.N)) || (PlayerID == 2 && Input.GetKeyDown(KeyCode.U));
    }

    bool ShootPressed() 
    {
        return (PlayerID == 1 && Input.GetKeyDown(KeyCode.V)) || (PlayerID == 2 && Input.GetKeyDown(KeyCode.I));
    }
}

class BombEvent {
    public bool isExplode;
    public BombEvent(bool explode) {
        isExplode = explode;
    }
}
