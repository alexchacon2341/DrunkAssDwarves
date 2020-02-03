using System;
using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    [Serializable]
    public struct LightConfig
    {
        public float lightRange;
        public float maskScale;
    }

    [SerializeField] private Light pointLight;
    [SerializeField] private Light spotLight;

    [SerializeField] private LightConfig[] LanternLevelConfig;
    [SerializeField] private LightConfig[] SpotlightLevelConfig;

    private int lanternLevel;
    private int spotlightLevel;

    private float lastFrameAngle;

    public void IncreaseLanternLightLevelBy1()
    {
        SetLightLevel(ref lanternLevel, lanternLevel + 1);
    }

    public void DecreaseLanternLightLevelBy1()
    {
        SetLightLevel(ref lanternLevel, lanternLevel - 1);
    }

	public void SetSpotlightLevel(int level)
	{
		spotlightLevel = level;
		SetLightLevel(ref spotlightLevel, spotlightLevel);
	}

    public void IncreaseSpotlightLevelBy1()
    {
        SetLightLevel(ref spotlightLevel, spotlightLevel + 1);
    }

    public void DecreaseSpotlightLevelBy1()
    {
        SetLightLevel(ref spotlightLevel, spotlightLevel - 1);
    }

    public void SetLightLevel(ref int target, int newLevel)
    {
        var targetLevel = Mathf.Clamp(newLevel, 0, LanternLevelConfig.Length - 1);
        target = targetLevel;
    }

    public void UpdateLights(Vector2 moveDirection)
    {
        updateLight(pointLight, moveDirection);
        updateLight(spotLight, moveDirection);
    }

    private void updateLight(Light light, Vector2 moveDirection)
    {
        LightConfig[] levelConfig = null;
        int level = 0;
        if (light.type == LightType.Point)
        {
            levelConfig = LanternLevelConfig;
            level = lanternLevel;
        }
        else if (light.type == LightType.Spot)
        {
            levelConfig = SpotlightLevelConfig;
            level = spotlightLevel;
        }

        light.range = levelConfig[level].lightRange;
        var randomScale = UnityEngine.Random.Range(levelConfig[level].maskScale - 0.03f, levelConfig[level].maskScale + 0.03f);
        var targetScale = Mathf.Lerp(light.transform.localScale.z, randomScale, 0.25f);
        light.transform.localScale = new Vector3(targetScale, targetScale, targetScale);

        if (light.type == LightType.Spot && moveDirection != Vector2.zero)
        {
            var joyAngle = Mathf.Rad2Deg * Mathf.Atan2(moveDirection.y, -moveDirection.x);
            var targetAngle = Mathf.LerpAngle(lastFrameAngle, -joyAngle, 0.25f);
            light.transform.localRotation = Quaternion.Euler(targetAngle, -90f, 0);

            lastFrameAngle = targetAngle;
        }
    }

#if UNITY_EDITOR
    //Debug Logic
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            IncreaseLanternLightLevelBy1();
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            DecreaseLanternLightLevelBy1();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            IncreaseSpotlightLevelBy1();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            DecreaseSpotlightLevelBy1();
        }
    }
#endif
}
