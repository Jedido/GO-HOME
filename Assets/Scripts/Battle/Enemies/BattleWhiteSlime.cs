using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// White Slime AI
// Phase 1
// Movement: fast, moves around player
// Attack: stream of non-phasing shots
// Phase 2 (phase)
// Movement: 
// Attack:
// Phase 3 (berserk)
// Invulnerable until hit with a shock
// Movement: 
// Attack:
public class BattleWhiteSlime : BattleCPU {
    private int phase;

    // Phase 1
    private Vector2 dir;
    private float moveTimer;
    private static readonly float moveCooldown = 1.5f;
    private float angle, angleEnd;  // spread shot angle
    public static readonly Vector2[] corners = new Vector2[] { new Vector2(-3, -3),
        new Vector2(3, -3),
        new Vector2(3, 3),
        new Vector2(-3, 3) };
    private int curPos;

    new protected void Start()
    {
        base.Start();
        phase = 0;
        moveTimer = 0;
        curPos = 0;
    }

    protected override int GetHealth()
    {
        return 20;
    }

    // Update is called once per frame
    protected override void UpdateCPU()
    {
        if (phase != (20 - HP) / 7)
        {
            phase = (20 - HP) / 7;
        }
        Vector2 playerPos = PlayerManager.player.battleAlien.transform.localPosition;
        Vector2 direction = playerPos - (Vector2)transform.localPosition;

        switch (phase)
        {
            case 0:
                {
                    // Attack
                    if (angle < angleEnd)
                    {
                        angle += 5;
                        float rad = angle * Mathf.PI / 180;
                        Vector2 shotDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
                        EnemyProjectile shot = Instantiate(smallProj, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
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

                            angle -= 30f;
                            angleEnd = angle + 60f;
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
                // Attack

                // Movement
                break;
            case 2:
                // Attack

                // Movement
                break;
        }
    }
}
