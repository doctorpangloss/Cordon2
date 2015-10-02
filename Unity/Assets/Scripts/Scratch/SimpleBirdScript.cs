using UnityEngine;
using System.Collections;

public class SimpleBirdScript : MonoBehaviour {

    public Vector3 velocity;
    public Vector3 startPoint;
    public Vector3 endPoint;
    private Camera mainCamera;
    private float minLife;
    private float life;
    //bird will be automatically removed if life > minLife and not in main Camera
    //in Seconds
	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
        minLife = 5;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += (velocity * Time.deltaTime);
        life += Time.deltaTime;
        if (life > minLife && !isInCamera())
        {
            Destroy(this.gameObject);
        }
	}

    bool isInCamera()
    {
        Vector3 viewPos = mainCamera.WorldToViewportPoint(this.transform.position);
        return (viewPos.x > 0f && viewPos.x < 1f && viewPos.y > 0f && viewPos.y < 1f && viewPos.z > 0f);
    }
}
