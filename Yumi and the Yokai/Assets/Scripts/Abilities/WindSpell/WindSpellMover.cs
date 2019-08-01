/*WindSpellMover.cs
********************************************************************************
*Creator(s)................................Jack Bruce && Evanito && Darrell Wong
*Created..................................................................5/08/18
*Last Modified..........................................................4/12/2019
*Last Modified by...................................................Darrell Wong
*
 * Description: Moves 'WindSpell' obj on desired path
 *  - WindSpellSpawner will populate targetarray
 *  - once activated WindSpellUse will pass the target LinkLis to WindSpellMover
 *  - then WindSpell obj will follow path declared by array
********************************************************************************
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpellMover : MonoBehaviour
{

    public float speed = 10;
    public ParticleSystem emit;

    private Vector3[] wsTargets;
    private Vector3 nextPos;
    private int current;
    private int wsTargetsSize;
    private string wsSpawnerName = "WindSpellSpawner(Clone)"; //had to make this dynamic for instantiated prefab names
    private GameObject spawnerObj;
    private Vector3 velocity = new Vector3(-1, -1, 0);
    private Rigidbody rb;
    private Vector3 lastPos;

    public GameObject wind_impact;

    // Use this for initialization
    void Start()
    {
        SetSpawner(GameObject.Find(wsSpawnerName));
        wsTargets = spawnerObj.GetComponent<WindSpellUse>().GetTargets();
        wsTargetsSize = wsTargets.Length;
        NoiseReduction(wsTargets);
        current = 0;

        rb = this.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("WindSpellTrigger") && collision.gameObject.name != "Yumi" && collision.gameObject.name != "PlayerDetection")
        {
            Instantiate(wind_impact, this.transform.position, Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, velocity.normalized, Vector3.forward)));
            spawnerObj.GetComponent<WindSpellUse>().CleanUp();
        }
    }

    public Vector3 getVelocity()
    {

        velocity.x = (rb.position.x - lastPos.x) * 10f;
        velocity.y = (rb.position.y - lastPos.y) * 10f;
        //print(velocity);
        return velocity;

    }

    private void SetSpawner(GameObject spawner)
    {
        spawnerObj = spawner;
    }

    public void DetachParticles()
    {
        // This splits the particle off so it doesn't get deleted with the parent
        emit.transform.parent = null;

        // this stops the particle from creating more bits
        emit.enableEmission = false;

        // This finds the particleAnimator associated with the emitter and then
        // sets it to automatically delete itself when it runs out of particles

        //Destroy(emit, 2); for some reason this didnt work so i made a script "autoDestroyParticleSystem" to delete the particle emitter.
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity.x = (rb.position.x - lastPos.x) * 10f;
        velocity.y = (rb.position.y - lastPos.y) * 10f;

        //print("(drawn) windspell velocity: " + getVelocity());
        lastPos = rb.position;


        
        if (transform.position == wsTargets[wsTargetsSize - 1])
        { // die at end of path (transform.position == wsTargets[wsTargets.Length - 1])
            try
            {
                Instantiate(wind_impact, this.transform.position, Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, velocity.normalized, Vector3.forward)));
                //print(velocity);
                //print(Vector3.Angle(Vector3.right, velocity));
                DetachParticles();
                spawnerObj.GetComponent<WindSpellUse>().CleanUp();
            }
            catch
            {
                Instantiate(wind_impact, this.transform.position, Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, velocity.normalized, Vector3.forward)));
                DetachParticles();
                Destroy(gameObject);
            }

        }

        nextPos = wsTargets[current];
        // move until you reach the current obj
        if (transform.position != nextPos)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
            transform.LookAt(nextPos);  //align spell for the animation to face direction
            GetComponent<Rigidbody>().MovePosition(pos);
        }
        else if (current < wsTargetsSize - 1)
        {
            current++; // obj reached, move to the next obj
        }
    }

    private bool isZero(Vector3 inpos)
    {
        if (inpos.x == 0 && inpos.y == 0)
        {
            return true;
        }
        else
            return false;
    }

    private void NoiseReduction(Vector3[] src, int severity = 1)
    {
        for (int i = 1; i < src.Length; i++)
        {
            //---------------------------------------------------------------avg
            var start = (i - severity > 0 ? i - severity : 0);
            var end = (i + severity < src.Length ? i + severity : src.Length);

            float sumx = 0, sumy = 0;

            for (int j = start; j < end; j++)
            {
                sumx += src[j].x;
                sumy += src[j].y;
            }

            var avgx = sumx / (end - start);
            var avgy = sumy / (end - start);
            src[i].x = avgx;
            src[i].y = avgy;

        }
    }
}

