using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldManager : MonoBehaviour 
{

	public GameObject Panel;
	public GameObject BlockBuilder;


	// Use this for initialization
	void Start () 
	{
		Debug.Log ("WorldManager Start");

		
		BuildBlock (new Vector3 (0, 0, 0));
		BuildBlock (new Vector3 (80, 0, 0));

	}

	public void BuildBlock(Vector3 offset) {

		GameObject block1 = Instantiate(BlockBuilder);
		BlockBuilder blockBuilder = block1.GetComponent<BlockBuilder>();
		blockBuilder.test ();

		block1.transform.SetParent (Panel.transform);
		
		block1.transform.position = Panel.transform.position + offset;

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
