using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    public float timeScaleTargetValue = 0.2f;

    public float timeScaleOriginalValue = 1f;

    public float timeScaleChangeSpeed = 2f;

    public float cooldownDuration = 5f;

    [Header("Debug Info")]
    [SerializeField] private float currentCooldown;

    [SerializeField] private bool isCooldownActive;

    private bool isBulletTimeActive;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScaleOriginalValue;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        currentCooldown = cooldownDuration;
    }

    // Update is called once per frame
    void Update()
    {
        HandleBulletTime();
        HandleCooldown();
    }

    private void HandleBulletTime()
    {
        // 激活子弹时间
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isBulletTimeActive = true;
        }
        
        // 取消子弹时间
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isBulletTimeActive = false;
        }

        // 平滑调整时间缩放
        if (isBulletTimeActive && !isCooldownActive)
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, timeScaleTargetValue, timeScaleChangeSpeed * Time.unscaledDeltaTime);
            Time.timeScale = Mathf.Max(Time.timeScale, timeScaleTargetValue);
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, timeScaleOriginalValue, timeScaleChangeSpeed * Time.unscaledDeltaTime);
            Time.timeScale = Mathf.Min(Time.timeScale, timeScaleOriginalValue);
        }

        // 确保时间缩放不会低于0或高于原始值
        Time.timeScale = Mathf.Clamp(Time.timeScale, timeScaleTargetValue, timeScaleOriginalValue);
        
        // 同步物理时间步长
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private void HandleCooldown()
    {
        if (isBulletTimeActive && !isCooldownActive)
        {
            currentCooldown -= Time.unscaledDeltaTime;
            if (currentCooldown <= 0)
            {
                isCooldownActive = true;
                currentCooldown = 0;
            }
        }
        else if (isCooldownActive)
        {
            currentCooldown += Time.unscaledDeltaTime;
            if (currentCooldown >= cooldownDuration)
            {
                isCooldownActive = false;
                currentCooldown = cooldownDuration;
            }
        }
    }

    // 外部调用接口，重置冷却时间
    public void ResetCooldown()
    {
        isCooldownActive = false;
        currentCooldown = cooldownDuration;
    }

    // 获取剩余冷却时间
    public float GetRemainingCooldown()
    {
        return isCooldownActive ? 0 : currentCooldown;
    }

    // 获取是否在冷却中
    public bool IsOnCooldown()
    {
        return isCooldownActive;
    }
}