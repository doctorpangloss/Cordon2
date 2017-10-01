using UnityEngine;
using System.Collections;

public class SimpleBirdController : MonoBehaviour {

    public GameObject simpleBirdPrefab;
    public float birdSpeed;
    //public float birdDistance; //From Camera;
    public float birdHeight; // Above Ground;
    public float randomPositionFactor;

    public float spawnCheckInterval; //how often to check for spawning;
    public float spawnCheckProbablity; //probablity of spawning;
    private float timer;

    public int minFlockSize;
    public int maxFlockSize;
    
    private Camera mainCamera;

	// Use this for initialization
	void Start () {
        timer = 0;
        mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > spawnCheckInterval)
        {
            timer = 0;
            if (Random.value < spawnCheckProbablity)
            {
                int flockSize = (int)Random.Range(minFlockSize, maxFlockSize);
                float x = 0;
                float y = 0;
                //Select a random point on the side of the camera
                if (Random.value > 0.5)
                {
                    x = (Random.value > 0.5)? 1.5f : -0.5f;
                    y = Random.value*3 - 1f;
                } else
                {
                    x = Random.value* 3 - 1f;
                    y = (Random.value > 0.5) ? 1.5f : -0.5f;
                }
                Vector3 startPoint = new Vector3(0, 0, 0);
                Vector3 endPoint = new Vector3(1, 1, 1);
                Ray startRay = mainCamera.ViewportPointToRay(new Vector3(x, y, 0));
                RaycastHit hit;
                if(Physics.Raycast(startRay, out hit))
                {
                    startPoint = hit.point;
                    startPoint.y = birdHeight;
                }


                Ray endRay = mainCamera.ViewportPointToRay(new Vector3(1-x, 1-y, 0));
                if (Physics.Raycast(endRay, out hit))
                {
                    endPoint = hit.point;
                    endPoint.y = birdHeight;
                }


                for (int i = 0; i < flockSize; i++)
                {
                    spawnBird(startPoint, endPoint);
                }
            }
        }

	}


    void spawnBird(Vector3 startPoint, Vector3 endPoint)
    {
        startPoint.x += (Random.value - 0.5f) * randomPositionFactor;
        startPoint.y += (Random.value - 0.5f) * randomPositionFactor;
        startPoint.z += (Random.value - 0.5f) * randomPositionFactor;

        endPoint.x += (Random.value - 0.5f) * randomPositionFactor;
        endPoint.y += (Random.value - 0.5f) * randomPositionFactor;
        endPoint.z += (Random.value - 0.5f) * randomPositionFactor;


        GameObject bird = Instantiate(simpleBirdPrefab, startPoint, Quaternion.identity) as GameObject;
        Vector3 vel = (endPoint - startPoint).normalized * birdSpeed;
        SimpleBirdScript sbs = bird.GetComponent<SimpleBirdScript>();
        sbs.velocity = vel;
        sbs.startPoint = startPoint;
        sbs.endPoint = endPoint;
        bird.transform.rotation = Quaternion.FromToRotation(Vector3.forward, (endPoint - startPoint));

    }
}
