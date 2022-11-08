using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Movable Config", menuName = "Game Data/Movable Config")]
public class MovableConfig : ScriptableObject
{
    [SerializeField] private float ACCELERATION;
    [SerializeField] private float MAX_SPEED;
    [SerializeField] private float FRICTION;
    [SerializeField] private float GRAVITY;
    [SerializeField] private float JUMP_FORCE;
    [SerializeField] private int HP;
    public void MapConfig(out float ACCELERATION, out float MAX_SPEED, out float FRICTION, out float GRAVITY, out float JUMP_FORCE, out int HP)
    {
        ACCELERATION = this.ACCELERATION;
        MAX_SPEED = this.MAX_SPEED;
        FRICTION = this.FRICTION;
        GRAVITY = this.GRAVITY;
        JUMP_FORCE = this.JUMP_FORCE;
        HP = this.HP;
    }
}
