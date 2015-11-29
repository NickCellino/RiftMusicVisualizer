using UnityEngine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class LookInputModule : BaseInputModule {
	
	private static LookInputModule _singleton;
	public static LookInputModule singleton {
		get {
			return _singleton;
		}
	}
	public const int kLookId = -3;
	public string submitButtonName = "Fire1";
	public string controlAxisName = "Horizontal";
	public bool useSmoothAxis = true;
	public float smoothAxisMultiplier = 0.01f;
	// if smooth axis is off - the input is stepped - need an interval
	public float steppedAxisStepsPerSecond = 10f;
	private bool _guiRaycastHit;
	public bool guiRaycastHit {
		get {
			return _guiRaycastHit;
		}
	}
	private bool _controlAxisUsed;
	public bool controlAxisUsed {
		get {
			return _controlAxisUsed;
		}
	}
	private bool _buttonUsed;
	public bool buttonUsed {
		get {
			return _buttonUsed;
		}
	}
	private PointerEventData lookData;
	private Color currentSelectedNormalColor;
	private Color currentSelectedHighlightedColor;
	public bool useSelectColor = true;
	public Color selectColor = Color.blue;
	private float nextAxisActionTime;
	
	// use screen midpoint as locked pointer location, enabling look location to be the "mouse"
	private PointerEventData GetLookPointerEventData() {
		Vector2 lookPosition;
		lookPosition.x = Screen.width/2;
		lookPosition.y = Screen.height/2;
		if (lookData == null) {
			lookData = new PointerEventData(eventSystem);
		}
		lookData.Reset();
		lookData.delta = Vector2.zero;
		lookData.position = lookPosition;
		lookData.scrollDelta = Vector2.zero;
		eventSystem.RaycastAll(lookData, m_RaycastResultCache);
		lookData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
		if (lookData.pointerCurrentRaycast.gameObject != null) {
			_guiRaycastHit = true;
		} else {
			_guiRaycastHit = false;
		}
		m_RaycastResultCache.Clear();
		return lookData;
	}
	
	private void SetSelectedColor(GameObject go) {
		if (useSelectColor) {
			Selectable s = go.GetComponent<Selectable>();
			if (s != null) {
				ColorBlock cb = s.colors;
				currentSelectedNormalColor = cb.normalColor;
				currentSelectedHighlightedColor = cb.highlightedColor;
				cb.normalColor = selectColor;
				cb.highlightedColor = selectColor;
				s.colors = cb;
			}
		}
	}
	
	private void RestoreColor(GameObject go) {
		if (useSelectColor) {
			Selectable s = go.GetComponent<Selectable>();
			if (s != null) {
				ColorBlock cb = s.colors;
				cb.normalColor = currentSelectedNormalColor;
				cb.highlightedColor = currentSelectedHighlightedColor;
				s.colors = cb;
			}
		}
	}
	
	public void ClearSelection() {
		if (eventSystem.currentSelectedGameObject) {
			RestoreColor(eventSystem.currentSelectedGameObject);
			eventSystem.SetSelectedGameObject(null);
		}
	}
	
	public void Select(GameObject go) {
		ClearSelection();
		SetSelectedColor(go);
		eventSystem.SetSelectedGameObject(go);
	}
	
	private bool SendUpdateEventToSelectedObject() {
		if (eventSystem.currentSelectedGameObject == null)
			return false;
		BaseEventData data = GetBaseEventData ();
		ExecuteEvents.Execute (eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
		return data.used;
	}
	
	public override void Process() {
		_singleton = this;
		// send update events if there is a selected object - this is important for InputField to receive keyboard events
		SendUpdateEventToSelectedObject();
		PointerEventData lookData = GetLookPointerEventData();
		HandlePointerExitAndEnter(lookData,lookData.pointerCurrentRaycast.gameObject);
		_buttonUsed = false;
		if (Input.GetButtonDown (submitButtonName)) {
			ClearSelection();
			if (lookData.pointerCurrentRaycast.gameObject != null) {
				GameObject go = lookData.pointerCurrentRaycast.gameObject;
				GameObject newPressed = ExecuteEvents.ExecuteHierarchy (go, lookData, ExecuteEvents.submitHandler);
				if (newPressed == null) {
					// try select handler instead
					newPressed = ExecuteEvents.ExecuteHierarchy (go, lookData, ExecuteEvents.selectHandler);
					if (newPressed != null) {
						Select(newPressed);
					}
				} else {
					InputField infield = newPressed.GetComponent<InputField>();
					if (infield != null) {
						Select(newPressed);
					}
				}
				if (newPressed != null) {
					_buttonUsed = true;
				}
			}
		}
		_controlAxisUsed = false;
		if (eventSystem.currentSelectedGameObject && controlAxisName != null && controlAxisName != "") {
			float newVal = Input.GetAxis (controlAxisName);
			if (newVal > 0.01f || newVal < -0.01f) {
				if (useSmoothAxis) {
					Slider sl = eventSystem.currentSelectedGameObject.GetComponent<Slider>();
					if (sl != null) {
						sl.value += newVal*smoothAxisMultiplier;
						_controlAxisUsed = true;
					} else {
						Scrollbar sb = eventSystem.currentSelectedGameObject.GetComponent<Scrollbar>();
						if (sb != null) {
							sb.value += newVal*smoothAxisMultiplier;
							_controlAxisUsed = true;
						}
					}
				} else {
					_controlAxisUsed = true;
					float time = Time.unscaledTime;
					if (time > nextAxisActionTime) {
						nextAxisActionTime = time + 1f/steppedAxisStepsPerSecond;
						AxisEventData axisData = GetAxisEventData(newVal,0.0f,0.0f);
						if (!ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisData, ExecuteEvents.moveHandler)) {
							_controlAxisUsed = false;
						} 
					}
				}
			}
		}
	}	
}
