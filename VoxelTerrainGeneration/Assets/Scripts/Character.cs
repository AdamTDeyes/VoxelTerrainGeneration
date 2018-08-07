using UnityEngine;

public class Character : MonoBehaviour {

    public Transform Add;
    public Transform Delete;
	// Use this for initialization
	void Start () {
        Add = Transform.Instantiate(Resources.Load<Transform>("Add"), this.transform.position, Quaternion.identity) as Transform;
        Delete = Transform.Instantiate(Resources.Load<Transform>("Delete"), this.transform.position, Quaternion.identity) as Transform;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Add.transform.GetComponent<MeshRenderer>().enabled = true;

            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 30))
            {
                Vector3 rawposition = hit.point + hit.normal.Cap() - new Vector3(0.005f, 0.005f, 0.005f);
                Vector3 roundedposition = new Vector3(Mathf.RoundToInt(rawposition.x), Mathf.RoundToInt(rawposition.y), Mathf.RoundToInt(rawposition.z));
                Add.transform.position = roundedposition;
                MathHelper.AddBlock(roundedposition, Block.Stone);
            }
        }
        else
        {
            if (Add.transform.GetComponent<MeshRenderer>().enabled == true)
            {
                
            }
            Add.transform.GetComponent<MeshRenderer>().enabled = false;

        }
        if (Input.GetMouseButtonUp(1))
        {
            Delete.transform.GetComponent<MeshRenderer>().enabled = true;
             
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 30))
            {
                Vector3 rawposition = hit.point - hit.normal.Cap() + new Vector3(0.005f, 0.005f, 0.005f);
                Vector3 roundedposition = new Vector3(Mathf.RoundToInt(rawposition.x), Mathf.RoundToInt(rawposition.y), Mathf.RoundToInt(rawposition.z));
                Delete.transform.position = roundedposition;
                MathHelper.AddBlock(roundedposition, Block.Air);
            }
        }
        else
        {
            if (Delete.transform.GetComponent<MeshRenderer>().enabled == true)
            {
                
            }
            Delete.transform.GetComponent<MeshRenderer>().enabled = false; 
        }
    }
}
