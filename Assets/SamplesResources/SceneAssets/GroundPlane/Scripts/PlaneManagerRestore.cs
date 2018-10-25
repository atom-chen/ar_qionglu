using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class PlaneManagerRestore : PlaneManager
{
    public override void HandleInteractiveHitTest(HitTestResult result)
    {

        if (result == null)
        {
            Debug.LogError("Invalid hit test result!");
            return;
        }
        if (!m_GroundPlaneUI.IsCanvasButtonPressed())
        {
            Debug.Log("HandleInteractiveHitTest() called.");
            // If the PlaneFinderBehaviour's Mode is Automatic, then the Interactive HitTestResult will be centered.
            // PlaneMode.Ground and PlaneMode.Placement both use PlaneFinder's ContentPositioningBehaviour
            m_ContentPositioningBehaviour = m_PlaneFinder.GetComponent<ContentPositioningBehaviour>();
            m_ContentPositioningBehaviour.DuplicateStage = false;
            // Place object based on Ground Plane mode
            if (showGameObject != null)
            {
                showGameObject.gameObject.SetActive(true);
                
                UtilityHelper.EnableRendererColliderCanvas(showGameObject, true);
                switch (showGameObject.GetComponent<WriteItem>().goodsPositionEnum)
                {
                    case PlaneMode.None:


                        break;
                    case PlaneMode.GROUND:
                        m_ContentPositioningBehaviour.AnchorStage = m_PlaneAnchor;
                        m_ContentPositioningBehaviour.PositionContentAtPlaneAnchor(result);
                        showGameObject.transform.parent = planeAnchor.transform;

                        break;
                    case PlaneMode.MIDAIR:
                        m_ContentPositioningBehaviour.AnchorStage = m_MidAirAnchor;
                        m_ContentPositioningBehaviour.PositionContentAtMidAirAnchor(showGameObject.transform);
                        showGameObject.transform.parent = midAirAnchor.transform;
                        break;
                    default:
                        break;
                }
                showGameObject.transform.localPosition = Vector3.zero;
                Transform tempTrans = YiyouRestoreManager.Instance.GetModelTranform();
                showGameObject.transform.localEulerAngles = tempTrans.localEulerAngles;
                showGameObject.transform.localScale = tempTrans.localScale;
 
                UtilityHelper.RotateTowardCameraVuforia(showGameObject);
                WriteManager.Instance.SetText(YiyouRestoreManager.Instance.GetModelText());
                WriteManager.Instance.SetFont(font: YiyouRestoreManager.Instance.GetModelFont());
                GroundPlaneUI.Instance.ShowEffectPanel();
            }
        }
    }

}

