using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothDampTime = 0.2f;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private Rigidbody2D playerRb;
	private Vector3 smoothDampVelocity;
	
	
	void Awake()
	{
		playerRb = target.GetComponent<Rigidbody2D>();
	}
	
	
	void LateUpdate()
	{
		if( !useFixedUpdate )
			updateCameraPosition();
	}


	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}


	void updateCameraPosition()
	{
		if( playerRb == null )
		{
			return;
		}
		
		if( playerRb.velocity.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref smoothDampVelocity, smoothDampTime );
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, target.position - leftOffset, ref smoothDampVelocity, smoothDampTime );
		}
	}
	
}
