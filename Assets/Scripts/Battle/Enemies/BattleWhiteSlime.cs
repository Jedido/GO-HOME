using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// White Slime AI
// Phase 1
// Movement: fast, moves around player
// Attack: stream of non-phasing shots
// Phase 2 (phase)
// Movement: Center
// Attack: Spinning
// Phase 3 (berserk)
// Movement: Charge at player
// Attack: sideways bullets
public class BattleWhiteSlime : BattleCPU {
    public static readonly Vector2[] corners = new Vector2[] { new Vector2(-3, -3),
        new Vector2(3, -3),
        new Vector2(3, 3),
        new Vector2(-3, 3) };
    public Sprite angry;

    private int phase;
    private Vector2 dir;
    private float moveTimer, shotTimer;
    private static readonly float moveCooldown = 1.5f;
    private static readonly float shotCooldown = 0.3f;
    private float angle, angleEnd;  // spread shot angle
    private int curPos;
    private float orientation;
    private bool spin;
    private bool Spin
    {
        set {
            spin = value;
            if (!spin)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            }
        }
    }

    new protected void Start()
    {
        base.Start();
        phase = 0;
        moveTimer = 0;
        curPos = 0;
        orientation = 0;
    }

    protected override int GetHealth()
    {
        return 20;
    }

    // Update is called once per frame
    protected override void UpdateCPU()
    {
        if (spin)
        {
            transform.rotation = Quaternion.AngleAxis(orientation, Vector3.forward);
            orientation += 6;
        }

        if (phase != (20 - HP) / 7)
        {
            phase = (20 - HP) / 7;
            Invincible = true;
            moveTimer = Time.time + moveCooldown;
            if (phase == 1)
            {
                Velocity = (Vector2.zero - (Vector2)transform.localPosition) / moveCooldown;
            }
            else if (phase == 2)
            {
                Spin = true;
                GetComponent<SpriteRenderer>().sprite = angry;
            }
        }
        Vector2 playerPos = PlayerManager.player.battleAlien.transform.localPosition;
        Vector2 direction = (playerPos - (Vector2)transform.localPosition).normalized;

        switch (phase)
        {
            case 0:
                {
                    Invincible = false;
                    // Attack
                    if (angle < angleEnd)
                    {
                        angle += 10;
                        float rad = angle * Mathf.PI / 180;
                        Vector2 shotDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                        EnemyProjectile shot = Instantiate(SProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
                        shot.InitialVelocity = shotDir * 3f;
                    }

                    // Movement
                    if (moveTimer < Time.time)
                    {
                        if (dir.x != 0)
                        {
                            // Ready a spread attack
                            dir = new Vector2();
                            angle = Vector3.Angle(Vector3.up, direction);
                            if (direction.x > 0)
                            {
                                angle = -angle;
                            }
                            angle += 90f;

                            angle -= 45f;
                            angleEnd = angle + 90f;
                            moveTimer = Time.time + moveCooldown;
                        }
                        if (angle >= angleEnd)
                        {
                            // Choose next direction and leave a mine
                            moveTimer = Time.time + moveCooldown;
                            float maxDist = 0;
                            int corner = 0;
                            for (int i = 0; i < corners.Length; i++)
                            {
                                if (i != curPos && i != (curPos + 2) % 4) {
                                    float dist = Vector2.Distance(corners[i], playerPos);
                                    if (maxDist < dist)
                                    {
                                        corner = i;
                                        maxDist = dist;
                                    }
                                }
                            }
                            curPos = corner;
                            dir = (corners[corner] - (Vector2)transform.localPosition) / moveCooldown;
                        }
                        Velocity = dir;
                    }
                    break;
                }
            case 1:
                {
                    // Attack
                    if (moveTimer < Time.time && shotTimer < Time.time)
                    {
                        shotTimer = Time.time + 0.2f;
                        angle += 10f;
                        Invincible = false;
                        Velocity = Vector2.zero;
                        float rad = angle * Mathf.PI / 180;
                        Vector2 shotDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                        EnemyProjectile shot = Instantiate(SPNRProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
                        EnemyProjectile shot2 = Instantiate(SPNRProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
                        shot.InitialVelocity = shotDir * 3f;
                        shot2.InitialVelocity = -shotDir * 3f;

                    }

                    // Movement
                    // Go to center and stay there
                    break;
                }
            case 2:
                if (moveTimer < Time.time)
                {
                    // Attack
                    if (shotTimer < Time.time)
                    {
                        shotTimer = Time.time + shotCooldown;
                        EnemyProjectile forward = Instantiate(SProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
                        EnemyProjectile left = Instantiate(SProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
                        EnemyProjectile right = Instantiate(SProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
                        forward.InitialVelocity = direction * 5;
                        left.InitialVelocity = Vector2.Perpendicular(direction) * 5;
                        right.InitialVelocity = -Vector2.Perpendicular(direction) * 5;
                    }

                    // Movement
                    if (!Invincible)
                    {
                        Invincible = true;
                        spin = true;
                        Velocity = direction * 4f;
                    }
                    else
                    {
                        Velocity = (direction * 0.2f + Velocity * 0.8f).normalized * 4f;
                    }
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (phase == 2 && collision.tag.Equals("Wall") && moveTimer < Time.time)
        {
            Velocity *= -0.1f;
            Invincible = false;
            Spin = false;
            moveTimer = Time.time + 1.3f;
        }
    }
}
