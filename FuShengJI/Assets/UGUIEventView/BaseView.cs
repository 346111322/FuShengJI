using UnityEngine;

namespace BH.Core.Base
{
    public class BaseView : MonoBehaviour
	{
		/// Add a button event for a gameobject
		public void AddButtonEvent(GameObject button, UIButtonEvent.OnClickEvent callback, object sender = null)
		{
			if (null == button)
				return;
			UIButtonEvent btnEvent = button.GetComponent<UIButtonEvent>();
			if (null == btnEvent)
			{
				btnEvent = button.AddComponent<UIButtonEvent>();
			}
			btnEvent.Callback = callback;
			btnEvent.SenderParam = sender;
		}

        public void AddButtonEvent(GameObject button, UIButtonEvent.OnClickEvent callback, object sender, AudioClip clickSound)
        {
            AddButtonEvent(button, callback, sender);
            UIButtonEvent btnEvent = button.GetComponent<UIButtonEvent>();
            if (null == btnEvent)
            {
                return;
            }

            btnEvent.ClickSound = clickSound;
        }

	    /// Add a button event for a gameobject
	    public void AddButtonPressDownEvent(GameObject button, UIButtonEvent.OnPressEvent callback, object sender = null)
	    {
	        UIButtonEvent btnEvent = button.GetComponent<UIButtonEvent>();
	        if (null == btnEvent)
	        {
	            btnEvent = button.AddComponent<UIButtonEvent>();
	        }
	        btnEvent.PressDownCallback = callback;
	        btnEvent.SenderParam = sender;
	    }
	    /// Add a button event for a gameobject
	    public void AddButtonPressUpEvent(GameObject button, UIButtonEvent.OnPressEvent callback, object sender = null)
	    {
	        UIButtonEvent btnEvent = button.GetComponent<UIButtonEvent>();
	        if (null == btnEvent)
	        {
	            btnEvent = button.AddComponent<UIButtonEvent>();
	        }
	        btnEvent.PressUpCallback = callback;
	        btnEvent.SenderParam = sender;
	    }
		

		/// Remove the button event.
		public void RemoveEvent(GameObject button)
		{
			UIButtonEvent btnEvent = button.GetComponent<UIButtonEvent>();
			if(null != btnEvent)
			{
                btnEvent.Callback = null;
                btnEvent.PressDownCallback = null;
                btnEvent.PressUpCallback = null;
				btnEvent.SenderParam = null;
			}
		}
		

//		 Add a slide right over event for a gameobject
		public void AddSlideStartEvent (GameObject obj, UISlideEvent.OnSlideEvent callback, object sender = null)
		{
			UISlideEvent slideEvent = obj.GetComponent<UISlideEvent> ();
			if (null == slideEvent) {
				slideEvent = obj.AddComponent<UISlideEvent> ();
			}
			slideEvent.SlideStart = callback;
			slideEvent.SenderParam = sender;
		}
		
		// Add a slide click event for a gameobject
		public void AddSlideClickEvent (GameObject obj, UISlideEvent.OnSlideEvent callback, object sender = null)
		{
			UISlideEvent slideEvent = obj.GetComponent<UISlideEvent> ();
			if (null == slideEvent) {
				slideEvent = obj.AddComponent<UISlideEvent> ();
			}
			slideEvent.Click = callback;
			slideEvent.SenderParam = sender;
		}
		
//		 Add a slide left over event for a gameobject
		public void AddSlideLeftOverEvent (GameObject obj, UISlideEvent.OnSlideEvent callback, object sender = null)
		{
			UISlideEvent slideEvent = obj.GetComponent<UISlideEvent> ();
			if (null == slideEvent) {
				slideEvent = obj.AddComponent<UISlideEvent> ();
			}
			slideEvent.SlideLeftOver = callback;
			slideEvent.SenderParam = sender;
		}
	
		// Add a slide right over event for a gameobject
		public void AddSlideRightOverEvent (GameObject obj, UISlideEvent.OnSlideEvent callback, object sender = null)
		{
			UISlideEvent slideEvent = obj.GetComponent<UISlideEvent> ();
			if (null == slideEvent) {
				slideEvent = obj.AddComponent<UISlideEvent> ();
			}
			slideEvent.SlideRightOver = callback;
			slideEvent.SenderParam = sender;
		}
		
		// Add a sliding event for a gameobject
		public void AddSlidingEvent (GameObject obj, UISlideEvent.OnSlidingEvent callback, object sender = null)
		{
			UISlideEvent slideEvent = obj.GetComponent<UISlideEvent> ();
			if (null == slideEvent) {
				slideEvent = obj.AddComponent<UISlideEvent> ();
			}
			slideEvent.Sliding = callback;
			slideEvent.SenderParam = sender;
		}
		
		// Add a sliding left event for a gameobject
		public void AddSlidingLeftEvent (GameObject obj, UISlideEvent.OnSlidingEvent callback, object sender = null)
		{
			UISlideEvent slideEvent = obj.GetComponent<UISlideEvent> ();
			if (null == slideEvent) {
				slideEvent = obj.AddComponent<UISlideEvent> ();
			}
			slideEvent.SlidingLeft = callback;
			slideEvent.SenderParam = sender;
		}
		
		// Add a sliding right event for a gameobject
		public void AddSlidingRightEvent (GameObject obj, UISlideEvent.OnSlidingEvent callback, object sender = null)
		{
			UISlideEvent slideEvent = obj.GetComponent<UISlideEvent> ();
			if (null == slideEvent) {
				slideEvent = obj.AddComponent<UISlideEvent> ();
			}
			slideEvent.SlidingRight = callback;
			slideEvent.SenderParam = sender;
		}
		
		//Remove all slide event
		public void RemoveAllSlideEvent(GameObject obj)
		{
			UISlideEvent slideEvent = obj.GetComponent<UISlideEvent>();
			if(null != slideEvent)
			{
				slideEvent.SlideStart = null;
				slideEvent.Click = null;
				slideEvent.SlideLeftOver = null;
				slideEvent.SlideRightOver = null;
				slideEvent.Sliding = null;
				slideEvent.SlidingLeft = null;
				slideEvent.SlidingRight = null;
				slideEvent.SenderParam = null;
				Destroy(slideEvent);
			}
		}

		/// Find GameObject named 'objName'. Use the Transform.FindChild instead of it if you can.
		public GameObject FindChild(string objName, GameObject fromParent = null, bool isRecursively = true)
		{
			GameObject parent = fromParent;
			if (parent == null)
			{
				parent = this.gameObject;
			}
			
			if (isRecursively)
			{
				Transform child = FindChild(parent.transform, objName);
				if (null != child)
				{
					return child.gameObject;
				}
				else
				{
					return null;
				}
			}
			else
			{
                for (int idx = 0; idx < parent.transform.childCount; idx++)
                {
                    Transform child = parent.transform.GetChild(idx);
                    if (child == null)
                    {
                        continue;
                    }
                    if (child.name.Equals(objName))
                    {
                        return child.gameObject;
                    }
                }
			}
			
			return null;
		}

		/// Find Component named 'objName'
		public T FindComponent<T>(string objName, GameObject fromParent = null) where T : Component
		{
			GameObject obj = FindChild(objName, fromParent);
			if (null != obj)
			{
				return obj.GetComponent<T>();
			}
			else
			{
				return null;
			}
		}
		
		/// Destroy the Game Object and all it's children.
		public void DestroySelf(GameObject obj)
		{
			obj.name += "[delete]";
			Destroy(obj);
		}

		/// Destroy all children except itself.
		public void DestroyChildren(GameObject parent)
		{
            if (parent == null)
            {
                return;
            }

            for (int idx = 0; idx < parent.transform.childCount;idx++)
            {
                Transform child = parent.transform.GetChild(idx);
                if (child == null)
                {
                    continue;
                }
                child.name += "[delete]";
                Destroy(child.gameObject);
            }
		}

        private Transform FindChild(Transform parent, string objName)
        {
            if (parent.name.Equals(objName))
            {
                return parent;
            }
            else
            {
                for (int idx = 0; idx < parent.transform.childCount; idx++)
                {
                    Transform child = parent.transform.GetChild(idx);
                    if (child == null)
                    {
                        continue;
                    }
                    child = FindChild(child, objName);
                    if (null != child)
                    {
                        return child;
                    }
                }
                return null;
            }
        }
				
		public virtual void SetInstance()
		{
			
		}

//        public void ShakeGameObject(GameObject gb, bool shake)
//        {
//            if (shake)
//            {
//                Hashtable args = new Hashtable();
//                args["z"] = 30;
//                args["looptype"] = iTween.LoopType.pingPong;
//                args["time"] = 1.0f;
//                iTween.PunchRotation(gb, args);
//            }
//            else
//            {
//                iTween.Stop(gb);
//                gb.transform.rotation = Quaternion.identity;
//            }
//        }
	}
}
