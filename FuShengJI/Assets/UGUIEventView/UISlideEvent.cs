using UnityEngine;

public class UISlideEvent : MonoBehaviour
{
	
	public delegate void OnSlideEvent (GameObject obj,object sender = null);

	public delegate void OnSlidingEvent (GameObject obj,Vector2 delta,object sender = null);
	
	//When press down, call this callback
	public OnSlideEvent SlideStart { get; set; }
	//When press down, and no move, call this callback
	public OnSlideEvent Click {get; set;}
	
	//When press up and slide direction is left, call this callback
	public OnSlideEvent SlideLeftOver { get; set; }
	//When press up and slide direction is right, call this callback
	public OnSlideEvent SlideRightOver { get; set; }
	
	//When sliding, call this callback
	public OnSlidingEvent Sliding { get; set; }
	//When sliding and slide direction is left, call this callback
	public OnSlidingEvent SlidingLeft { get; set; }
	//When sliding and slide direction is right, call this callback
	public OnSlidingEvent SlidingRight { get; set; }
	
	public object SenderParam { get; set; }
	
	private Vector3 m_startMousePos = new Vector3 (0, 0, 0);
	
	void OnPress (bool isDown)
	{
		if (isDown) {
			m_startMousePos.x = Input.mousePosition.x;
			m_startMousePos.y = Input.mousePosition.y;
			m_startMousePos.z = Input.mousePosition.z;
			
			if (null != SlideStart) {
				SlideStart (this.gameObject, SenderParam);
			}
			
			//BH.Logger.Log("slide press down");
		} else {
			//BH.Logger.Log("slide press up");
			//left
			if (Input.mousePosition.x < m_startMousePos.x) {
				//BH.Logger.Log("slide left");
				if (null != SlideLeftOver) {
					SlideLeftOver (this.gameObject, SenderParam);
				}
			} else if (Input.mousePosition.x > m_startMousePos.x) { //right
				//BH.Logger.Log("slide right");
				if (null != SlideRightOver) {
					SlideRightOver (this.gameObject, SenderParam);
				}
			} 
			else
			{
				if (null != Click) {
					Click (this.gameObject, SenderParam);
				}
			}
		}
	}
	
	void OnDrag (Vector2 delta)
	{
		if (null != Sliding)
		{
			Sliding(this.gameObject, delta, SenderParam);		
		}
		
		//left
		if (Input.mousePosition.x < m_startMousePos.x) {
			//BH.Logger.Log("slide left");
			if (null != SlidingLeft) {
				SlidingLeft (this.gameObject, delta, SenderParam);
			}
		} else { //right
			//BH.Logger.Log("slide right");
			if (null != SlidingRight) {
				SlidingRight (this.gameObject, delta,SenderParam);
			}
		}
	}
}
