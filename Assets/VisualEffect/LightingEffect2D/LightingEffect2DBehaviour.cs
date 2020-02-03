using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Camera)), RequireComponent(typeof(PostProcessVolume))]
[ExecuteInEditMode]
public class LightingEffect2DBehaviour : Singleton
{
    [Serializable]
    public struct LightingEffect2DUnitData
    {
        public Vector2 position;
        public Vector2 direction;
        //=======================
        //Other varaibles
    }

    public Camera LightingEffectCamera;
    public List<GameObject> Units = new List<GameObject>();
    public LightingEffect2DUnitData[] UnitData;

    private void OnPreRender()
    {
        if (LightingEffectCamera != null)
            Shader.SetGlobalTexture("_PreRenderSource", LightingEffectCamera.targetTexture);
    }

    public void AddUnit(GameObject UnitToAdd)
    {
        if (UnitToAdd != null)
            Units.Add(UnitToAdd);
    }

    public void RemoveUnit(GameObject UnitToRemove)
    {
        if (UnitToRemove != null)
            Units.Remove(UnitToRemove);
    }

    private void Update()
    {
        foreach (var unit in Units)
        {

        }
    }
}