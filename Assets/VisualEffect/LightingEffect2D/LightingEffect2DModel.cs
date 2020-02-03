
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(LightingEffect2DRenderer), PostProcessEvent.AfterStack, "Custom/LightingEffect2D")]
public sealed class LightingEffect2D : PostProcessEffectSettings
{
    [Range(0f, 1f), Tooltip("Grayscale effect intensity.")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };
}

public sealed class LightingEffect2DRenderer : PostProcessEffectRenderer<LightingEffect2D>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/LightingEffect2D"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}