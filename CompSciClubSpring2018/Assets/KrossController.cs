using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrossController : MonoBehaviour
{

    Collider root_collider;
    Bounds root_collider_bounds;
    Bounds skin_root_collider_bounds;
    Rigidbody rb;

    public float ray_distance = .5f;
    public LayerMask platforms;
    public bool isGrounded = false;
    public bool movingRight = false;

    private Vector3 BR_corner;
    private Vector3 BL_corner;
    private Vector3 TR_corner;
    private Vector3 TL_corner;

    private Vector3 velocity;
    private Vector3 grav_a = Vector3.down * 0.05f;
    private Vector3 grav_force;

    private Vector3 forces_sum;

    public int num_vertical_rays = 4;
    public int num_horizontal_rays = 5;

    // Use this for initialization
    void Start()
    {

        root_collider = this.GetComponent<BoxCollider>();

        rb = this.GetComponent<Rigidbody>();
        grav_force = grav_a * rb.mass;

        forces_sum = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame

    private Vector3 previous_velocity;

    private void cast_check_ground_raycasts() {
        for (int i = 0; i < num_vertical_rays; i++)
        {
            Vector3 ray_origin = BL_corner + Vector3.right * i * ((BR_corner - BL_corner).magnitude / (num_vertical_rays - 1));
            Ray ray = new Ray(ray_origin, Vector3.down);
            Debug.DrawRay(ray.origin, ray.direction * ray_distance, Color.red);

            RaycastHit hit;
            bool raycasthit = Physics.Raycast(ray.origin, Vector3.down, out hit, ray_distance, platforms);
            if (raycasthit)
            {
                isGrounded = true;
                velocity = Vector3.zero;
            }
            else
            {
                Debug.Log("No floor hit");
            }
        }
    }

    private void cast_check_side_raycasts()
    {
        for(int i = 0; i < num_horizontal_rays; i++)
        {

            Vector3 corner_origin = velocity.x > 0 ? BR_corner : BL_corner;
            Vector3 ray_origin = corner_origin + Vector3.up * i * ((TR_corner - BR_corner).magnitude / (num_horizontal_rays - 1));
            Ray ray = new Ray(ray_origin, Vector3.right);

            if (velocity.x != 0)
            {
                Debug.DrawRay(ray.origin, Mathf.Sign(velocity.x) * ray.direction * ray_distance, Color.red);

                RaycastHit hit;
                bool raycasthit = Physics.Raycast(ray.origin, Vector3.right * Mathf.Sign(velocity.x), out hit, ray_distance, platforms);
                if (raycasthit)
                {
                    movingRight = true;
                    velocity.x = 0f;
                }
                else
                {
                    Debug.Log("No right hit");
                }
            }
            else {
                Debug.Log("Velocity.x == 0");
            }


        }
    }

    private void OnDrawGizmos()
    {

    }

    void Update()
    {
        forces_sum = Vector3.zero;
        
        root_collider_bounds = root_collider.bounds;

        velocity.x = 1f;
        skin_root_collider_bounds = root_collider_bounds;

        BL_corner = new Vector3(skin_root_collider_bounds.min.x, skin_root_collider_bounds.min.y);
        BR_corner = new Vector3(skin_root_collider_bounds.max.x, skin_root_collider_bounds.min.y);
        TL_corner = new Vector3(skin_root_collider_bounds.min.x, skin_root_collider_bounds.max.y);
        TR_corner = new Vector3(skin_root_collider_bounds.max.x, skin_root_collider_bounds.max.y);

        cast_check_ground_raycasts();
        
        if (!isGrounded)
        {
            forces_sum = forces_sum + grav_force;            
        }
        
        velocity += (forces_sum / rb.mass);
        velocity.x = -1f;
        cast_check_side_raycasts();
        //velocity.x = 1;

        this.transform.Translate(velocity * Time.deltaTime);
        previous_velocity = velocity;
    }
}
