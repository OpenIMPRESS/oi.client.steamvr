using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WandController : MonoBehaviour {

    public Valve.VR.SteamVR_Behaviour_Pose pose;
    public oi.plugin.linedrawing.LineDrawCapture lineDrawCapture;

    private Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId padButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;

    //Valve.VR.SteamVR_Behaviour_Pose controller;
    //private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    //private SteamVR_TrackedObject trackedObj;


    public Vector3 prevPos;
    public Vector3 deltaPos = new Vector3();
    public bool interacting = false;

    public bool thumbPress = false;


    // Use this for initialization
    void Awake() {
        //trackedObj = GetComponent<SteamVR_TrackedObject>();
//        lineDrawCapture = GetComponentInChildren<LineDrawCapture>();
    }



    // Update is called once per frame
    void Update() {
        if (!pose.isActive) return;

        if (Valve.VR.SteamVR_Input._default.inActions.GrabPinch.GetStateDown(pose.inputSource)) {
            lineDrawCapture.SetButtonDown(true);
        }

        if (Valve.VR.SteamVR_Input._default.inActions.GrabPinch.GetStateUp(pose.inputSource)) {
            lineDrawCapture.SetButtonDown(false);
        }
    }

    public void UpdateDrag() {

        if (!gameObject.activeInHierarchy) return;

        if (Valve.VR.SteamVR_Input._default.inActions.GrabGrip.GetLastStateDown(pose.inputSource) && !interacting) {
            interacting = true;
        } else if (Valve.VR.SteamVR_Input._default.inActions.GrabGrip.GetLastStateUp(pose.inputSource) && interacting) {
            interacting = false;
        }

        if (Valve.VR.SteamVR_Input._default.inActions.Teleport.GetLastStateDown(pose.inputSource) && !thumbPress) {
            thumbPress = true;
        } else if (Valve.VR.SteamVR_Input._default.inActions.Teleport.GetLastStateUp(pose.inputSource) && thumbPress) {
            thumbPress = false;
        }

        if (interacting) {
            deltaPos = transform.position - prevPos;
        }

    }

    public void SavePos() {
        prevPos = transform.position;
    }

    private void StartInteraction() {
        interacting = true;
        prevPos = transform.position;
    }

    private void EndInteraction() {
        interacting = false;
    }
}
