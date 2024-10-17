using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelEffects : SingletonBaseManager<PanelEffects>
{

    private Transform panelTransform;
    private PanelEffects() { }
    
    public void PanelPopUp(Transform _panelTransform)
    {
        panelTransform = _panelTransform;
        panelTransform.LeanMoveLocalY(0, 0.3f).setEase(LeanTweenType.easeOutCirc);
    }

    public void PanelPopDown(Transform _panelTransform)
    {
        panelTransform = _panelTransform;
        panelTransform.LeanMoveLocalY(-1100, 0.3f).setEase(LeanTweenType.easeOutCubic);
    }

    
}
