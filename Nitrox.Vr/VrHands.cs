extern alias SteamVRRef;
using System.Collections;
using Nitrox.Vr.Common;
using Nitrox.Vr.Extensions;
using NitroxModel.Logger;
using RootMotion.FinalIK;
using SteamVRRef::Valve.VR;
using UnityEngine;

namespace Nitrox.Vr;

public class VrHands : MonoBehaviour
{
    public FullBodyBipedIK? ik;

    public Transform? leftTarget;
    public Transform? rightTarget;

    public Transform? leftHand;
    public Transform? rightHand;
    public Transform? leftElbow;
    public Transform? rightElbow;

    public static VrHands? Instance;

    public Transform[] leftHandFingers = [];
    public Transform[] rightHandFingers = [];
    public Vector3[] minRotation = [];
    public Vector3[] maxRotation = [];

    public void Setup(FullBodyBipedIK fullBodyBipedIK)
    {
        Instance = this;
        ik = fullBodyBipedIK;

        leftHand = fullBodyBipedIK.solver.leftHandEffector.bone;
        rightHand = fullBodyBipedIK.solver.rightHandEffector.bone;
        leftElbow = leftHand.parent;
        rightElbow = rightHand.parent;
        leftHand.parent = leftElbow.parent;
        rightHand.parent = rightElbow.parent;

        VrCameraRig? camRig = VrCameraRig.Instance;
        if (camRig != null)
        {
            if (camRig.leftHandTarget != null)
            {
                leftTarget = camRig.leftHandTarget.transform;
            }

            if (camRig.rightHandTarget != null)
            {
                rightTarget = camRig.rightHandTarget.transform;
            }
        }

        ResetHandTargets();
        StartCoroutine(UpdateBodyRendering());
        SetupFingers();
    }

    private IEnumerator UpdateBodyRendering()
    {
        while (true)
        {
            IEnumerable<SkinnedMeshRenderer> bodyRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>().Where(r => r.name.Contains("body") || r.name.Contains("vest"));
            foreach (SkinnedMeshRenderer? bodyRenderer in bodyRenderers)
            {
                bodyRenderer.enabled = Settings.FullBody;
            }
            IEnumerable<SkinnedMeshRenderer> handRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true).Where(m => m.name.Contains("glove") || m.name.Contains("hands"));
            handRenderers.ForEach(mr =>
            {
                mr.updateWhenOffscreen = true;
            });

            yield return new WaitForSeconds(2.0f);
        }
    }

    public void ResetHandTargets()
    {
        if (leftTarget != null)
        {
            HandOffset.LeftHand.Apply(leftTarget);
        }
        if (rightTarget != null)
        {
            HandOffset.RightHand.Apply(rightTarget);
        }
    }

    public void SetupFingers()
    {
        string[] boneNamesLeft = new string[(int)HandSkeletonBone.eBone_Count];
        boneNamesLeft[(int)HandSkeletonBone.eBone_Thumb1] = "/hand_L_thumb_base";
        boneNamesLeft[(int)HandSkeletonBone.eBone_Thumb2] = "/hand_L_thumb_base/hand_L_thumb_mid";
        boneNamesLeft[(int)HandSkeletonBone.eBone_Thumb3] = "/hand_L_thumb_base/hand_L_thumb_mid/hand_L_thumb_tip";
        boneNamesLeft[(int)HandSkeletonBone.eBone_IndexFinger1] = "/hand_L_point_base";
        boneNamesLeft[(int)HandSkeletonBone.eBone_IndexFinger2] = "/hand_L_point_base/hand_L_point_mid";
        boneNamesLeft[(int)HandSkeletonBone.eBone_IndexFinger3] = "/hand_L_point_base/hand_L_point_mid/hand_L_point_tip";
        boneNamesLeft[(int)HandSkeletonBone.eBone_MiddleFinger1] = "/hand_L_midl_base";
        boneNamesLeft[(int)HandSkeletonBone.eBone_MiddleFinger2] = "/hand_L_midl_base/hand_L_midl_mid";
        boneNamesLeft[(int)HandSkeletonBone.eBone_MiddleFinger3] = "/hand_L_midl_base/hand_L_midl_mid/hand_L_midl_tip";
        boneNamesLeft[(int)HandSkeletonBone.eBone_RingFinger1] = "/hand_L_ring_base";
        boneNamesLeft[(int)HandSkeletonBone.eBone_RingFinger2] = "/hand_L_ring_base/hand_L_ring_mid";
        boneNamesLeft[(int)HandSkeletonBone.eBone_RingFinger3] = "/hand_L_ring_base/hand_L_ring_mid/hand_L_ring_tip";
        boneNamesLeft[(int)HandSkeletonBone.eBone_PinkyFinger1] = "/hand_L_pinky_base";
        boneNamesLeft[(int)HandSkeletonBone.eBone_PinkyFinger2] = "/hand_L_pinky_base/hand_L_pinky_mid";
        boneNamesLeft[(int)HandSkeletonBone.eBone_PinkyFinger3] = "/hand_L_pinky_base/hand_L_pinky_mid/hand_L_pinky_tip";

        string[] boneNamesRight = new string[(int)HandSkeletonBone.eBone_Count];
        boneNamesRight[(int)HandSkeletonBone.eBone_Thumb1] = "/hand_R_thumb_base";
        boneNamesRight[(int)HandSkeletonBone.eBone_Thumb2] = "/hand_R_thumb_base/hand_R_thumb_mid";
        boneNamesRight[(int)HandSkeletonBone.eBone_Thumb3] = "/hand_R_thumb_base/hand_R_thumb_mid/hand_R_thumb_tip_rig";
        boneNamesRight[(int)HandSkeletonBone.eBone_IndexFinger1] = "/hand_R_point_base";
        boneNamesRight[(int)HandSkeletonBone.eBone_IndexFinger2] = "/hand_R_point_base/hand_R_point_mid";
        boneNamesRight[(int)HandSkeletonBone.eBone_IndexFinger3] = "/hand_R_point_base/hand_R_point_mid/hand_R_point_tip_rig";
        boneNamesRight[(int)HandSkeletonBone.eBone_MiddleFinger1] = "/hand_R_midl_base";
        boneNamesRight[(int)HandSkeletonBone.eBone_MiddleFinger2] = "/hand_R_midl_base/hand_R_midl_mid";
        boneNamesRight[(int)HandSkeletonBone.eBone_MiddleFinger3] = "/hand_R_midl_base/hand_R_midl_mid/hand_R_midl_tip_rig";
        boneNamesRight[(int)HandSkeletonBone.eBone_RingFinger1] = "/hand_R_ring_base";
        boneNamesRight[(int)HandSkeletonBone.eBone_RingFinger2] = "/hand_R_ring_base/hand_R_ring_mid";
        boneNamesRight[(int)HandSkeletonBone.eBone_RingFinger3] = "/hand_R_ring_base/hand_R_ring_mid/hand_R_ring_tip_rig";
        boneNamesRight[(int)HandSkeletonBone.eBone_PinkyFinger1] = "/hand_R_pinky_base";
        boneNamesRight[(int)HandSkeletonBone.eBone_PinkyFinger2] = "/hand_R_pinky_base/hand_R_pinky_mid";
        boneNamesRight[(int)HandSkeletonBone.eBone_PinkyFinger3] = "/hand_R_pinky_base/hand_R_pinky_mid/hand_R_pinky_tip_rig";

        minRotation = new Vector3[(int)HandSkeletonBone.eBone_Count];
        for (int i = 0; i < minRotation.Length; i++)
        {
            minRotation[i] = Vector3.zero;
        }

        minRotation[(int)HandSkeletonBone.eBone_Thumb1] = new Vector3(50.2f, 65.0f, 23.1f);
        minRotation[(int)HandSkeletonBone.eBone_Thumb2] = new Vector3(2.7f, -8f, 10f);
        minRotation[(int)HandSkeletonBone.eBone_Thumb3] = new Vector3(0.0f, 0.0f, 2.2f);

        maxRotation = new Vector3[(int)HandSkeletonBone.eBone_Count];
        maxRotation[(int)HandSkeletonBone.eBone_Thumb1] = new Vector3(20.2f, 50.2f, 31.6f);
        maxRotation[(int)HandSkeletonBone.eBone_Thumb2] = new Vector3(37.7f, -8f, 34.0f);
        maxRotation[(int)HandSkeletonBone.eBone_Thumb3] = new Vector3(0.0f, 0.0f, 52.9f);
        maxRotation[(int)HandSkeletonBone.eBone_IndexFinger1] = new Vector3(-10f, -16f, 79.1f);
        maxRotation[(int)HandSkeletonBone.eBone_IndexFinger2] = new Vector3(30.0f, 0.0f, 109.8f);
        maxRotation[(int)HandSkeletonBone.eBone_IndexFinger3] = new Vector3(0.0f, 2.7f, 76.5f);
        maxRotation[(int)HandSkeletonBone.eBone_MiddleFinger1] = new Vector3(-9f, -16f, 77.1f);
        maxRotation[(int)HandSkeletonBone.eBone_MiddleFinger2] = new Vector3(20.0f, 0.0f, 96.8f);
        maxRotation[(int)HandSkeletonBone.eBone_MiddleFinger3] = new Vector3(7.0f, 2.7f, 78.5f);
        maxRotation[(int)HandSkeletonBone.eBone_RingFinger1] = new Vector3(-10f, -20f, 74.1f);
        maxRotation[(int)HandSkeletonBone.eBone_RingFinger2] = new Vector3(15.0f, 0.0f, 94.8f);
        maxRotation[(int)HandSkeletonBone.eBone_RingFinger3] = new Vector3(0.0f, 2.7f, 78.5f);
        maxRotation[(int)HandSkeletonBone.eBone_PinkyFinger1] = new Vector3(-7f, -15f, 71.1f);
        maxRotation[(int)HandSkeletonBone.eBone_PinkyFinger2] = new Vector3(6.0f, 0.0f, 101.8f);
        maxRotation[(int)HandSkeletonBone.eBone_PinkyFinger3] = new Vector3(-8f, 2.7f, 78.5f);

        leftHandFingers = new Transform[(int)HandSkeletonBone.eBone_Count];
        rightHandFingers = new Transform[(int)HandSkeletonBone.eBone_Count];
        Animator? animator = Player.main.playerAnimator;
        if (animator is Animator anim)
        {
            for (int i = 0; i < boneNamesLeft.Length; i++)
            {
                string boneName = boneNamesLeft[i];
                if (string.IsNullOrWhiteSpace(boneName))
                {
                    continue;
                }

                leftHandFingers[i] = anim.transform.Find($"export_skeleton/head_rig/neck/chest/clav_L/clav_L_aim/shoulder_L/hand_L{boneName}");
                if (leftHandFingers[i] == null)
                {
                    leftHandFingers[i] = anim.transform.Find($"export_skeleton/head_rig/neck/chest/clav_L/clav_L_aim/shoulder_L/elbow_L/hand_L{boneName}");
                }
            }
            for (int i = 0; i < boneNamesRight.Length; i++)
            {
                string boneName = boneNamesRight[i];
                if (string.IsNullOrWhiteSpace(boneName))
                {
                    continue;
                }

                rightHandFingers[i] = anim.transform.Find($"export_skeleton/head_rig/neck/chest/clav_R/clav_R_aim/shoulder_R/hand_R{boneName}");
                if (rightHandFingers[i] == null)
                {
                    rightHandFingers[i] = anim.transform.Find($"export_skeleton/head_rig/neck/chest/clav_R/clave_R_aim/shoulder_R/elbow_R/hand_R{boneName}");
                }
            }
        }
    }
    
    private void Update()
    {
        if (ik != null && ik.enabled)
        {
            ik.solver.leftHandEffector.target = leftTarget;
            ik.solver.rightHandEffector.target = rightTarget;
        }
    }
    
    public void OnToolEquipped(PlayerTool tool)
    {
        TransformOffset aimOffset = tool.GetAimOffset();
        
        if (VrCameraRig.Instance != null)
        {
            VrCameraRig.Instance.TargetTransform = aimOffset;
        }
        
        if (rightTarget != null)
        {
            tool.GetHandOffset().Apply(rightTarget.transform);
        }
    }
    
    public void OnOpenPDA()
    {
        if (leftTarget == null)
        {
            return;
        }
        
        HandOffset.PDA.Apply(leftTarget);
    }
    public void OnClosePDA()
    {
        ResetHandTargets();
    }

    private void LateUpdate()
    {
        if (ik != null && ik.enabled)
        {
            // TODO: Add back experimental IK behind an option
            return;
        }

        if (leftHand != null && leftTarget != null && leftElbow != null)
        {
            leftHand.transform.SetPositionAndRotation(leftTarget.position, leftTarget.rotation);
            leftElbow.transform.SetPositionAndRotation(leftHand.position, leftHand.rotation);
            leftElbow.localScale = Vector3.zero;
        }
        if (rightHand != null && rightElbow != null && rightTarget != null)
        {
            rightHand.transform.SetPositionAndRotation(rightTarget.position, rightTarget.rotation);
            rightElbow.transform.SetPositionAndRotation(rightHand.position, rightHand.rotation);
            rightElbow.localScale = Vector3.zero;
        }
        
        if (Settings.ArticulatedHands)
        {
            SteamVR_Action_Skeleton rightSkeletonAction = SteamVR_Input.GetSkeletonAction("RightHandSkeleton");
            SteamVR_Action_Skeleton leftSkeletonAction = SteamVR_Input.GetSkeletonAction("LeftHandSkeleton");

            if (!Player.main.pda.isOpen)
            {
                UpdateFinger(leftHandFingers, (int)HandSkeletonBone.eBone_PinkyFinger1, leftSkeletonAction.pinkyCurl);
                UpdateFinger(leftHandFingers, (int)HandSkeletonBone.eBone_RingFinger1, leftSkeletonAction.ringCurl);
                UpdateFinger(leftHandFingers, (int)HandSkeletonBone.eBone_MiddleFinger1, leftSkeletonAction.middleCurl);
                UpdateFinger(leftHandFingers, (int)HandSkeletonBone.eBone_IndexFinger1, leftSkeletonAction.indexCurl);
                UpdateFinger(leftHandFingers, (int)HandSkeletonBone.eBone_Thumb1, leftSkeletonAction.thumbCurl);
            }

            if (Inventory.main.GetHeld() == null)
            {
                UpdateFinger(rightHandFingers, (int)HandSkeletonBone.eBone_PinkyFinger1, rightSkeletonAction.pinkyCurl);
                UpdateFinger(rightHandFingers, (int)HandSkeletonBone.eBone_RingFinger1, rightSkeletonAction.ringCurl);
                UpdateFinger(rightHandFingers, (int)HandSkeletonBone.eBone_MiddleFinger1, rightSkeletonAction.middleCurl);
                UpdateFinger(rightHandFingers, (int)HandSkeletonBone.eBone_IndexFinger1, rightSkeletonAction.indexCurl);
                UpdateFinger(rightHandFingers, (int)HandSkeletonBone.eBone_Thumb1, rightSkeletonAction.thumbCurl);
            }
        }
    }
    
    public void UpdateFinger(Transform[] fingers, int fingerID, float percent)
    {
        fingers[fingerID].transform.localRotation = Quaternion.Euler(minRotation[fingerID].x + ((maxRotation[fingerID].x - minRotation[fingerID].x) * percent), minRotation[fingerID].y + ((maxRotation[fingerID].y - minRotation[fingerID].y) * percent), minRotation[fingerID].z + ((maxRotation[fingerID].z - minRotation[fingerID].z) * percent));
        fingers[fingerID + 1].transform.localRotation = Quaternion.Euler(minRotation[fingerID + 1].x + ((maxRotation[fingerID + 1].x - minRotation[fingerID + 1].x) * percent), minRotation[fingerID + 1].y + ((maxRotation[fingerID + 1].y - minRotation[fingerID + 1].y) * percent), minRotation[fingerID + 1].z + ((maxRotation[fingerID + 1].z - minRotation[fingerID + 1].z) * percent));
        fingers[fingerID + 2].transform.localRotation = Quaternion.Euler(minRotation[fingerID + 2].x + ((maxRotation[fingerID + 2].x - minRotation[fingerID + 2].x) * percent), minRotation[fingerID + 2].y + ((maxRotation[fingerID + 2].y - minRotation[fingerID + 2].y) * percent), minRotation[fingerID + 2].z + ((maxRotation[fingerID + 2].z - minRotation[fingerID + 2].z) * percent));
    }
}
