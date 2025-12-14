using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 重定位组件，用于处理游戏对象离开区域时的重新定位逻辑
/// </summary>
public class Reposition : MonoBehaviour
{
    /// <summary>
    /// 当碰撞体退出触发器时调用此方法
    /// </summary>
    /// <param name="Collision">与其他碰撞体的碰撞信息</param>
    private void OnTriggerExit2D(Collider2D Collision)
    {
        // 如果碰撞的对象不是"Area"标签的对象，则直接返回
        if (!Collision.CompareTag("Area"))
            return;
            
        // 获取玩家位置和当前对象位置
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        
        // 计算玩家与当前对象在x轴和y轴上的距离差的绝对值
        float diffx = Math.Abs(playerPos.x - myPos.x);
        float diffy = Math.Abs(playerPos.y - myPos.y);
        
        // 获取玩家移动方向
        Vector3 playerDir = GameManager.instance.player.inputVec;
        // 根据玩家x轴输入确定水平方向（左或右）
        float dirX = playerDir.x < 0 ? -1 : 1;
        // 根据玩家y轴输入确定垂直方向（上或下）
        float dirY = playerDir.y < 0 ? -1 : 1;
        
        // 根据对象标签执行不同的重定位逻辑
        switch (transform.tag)
        {
            case "Ground":
                // 如果在x轴上的距离差大于y轴上的距离差，则水平移动
                if (diffx > diffy)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                // 如果在y轴上的距离差大于x轴上的距离差，则垂直移动
                else if (diffx < diffy)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                // 敌人类型的对象暂无特殊处理
                break;
        }
    }
}