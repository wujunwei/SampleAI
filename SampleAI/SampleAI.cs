using System.Linq;
using TuringCup;
using Newtonsoft.Json.Linq;

public class HAHATeam : AIBase
{

    public HAHATeam(int a_index) : base(a_index) { }

    public override string TeamName
    {
        get
        {
            return "NULL";
        }
    }

    public override Character ChooseCharacter
    {
        get
        {
            return Character.Locky;
        }
    }

    private float shootRange = 10;
    private float BigRange = 15;
    protected override void Act(JObject state)
    {
        var me = state["me"];
        float x1 = (float)me["pos"]["x"];
        float z1 = (float)me["pos"]["z"];
        if (x1 < 30 && z1 < 70) Move(30, 30);
        if (x1 > 30 && z1 < 30) Move(70, 30);
        if (x1 > 70 && z1 > 30) Move(70, 70);
        if (x1 < 70 && z1 > 70) Move(30, 70);
        if (x1 == 30 && z1 != 30) Move(30, 30);
        if (x1 == 70 && z1 != 70) Move(70, 70);
        if (z1 == 70 && x1 != 30) Move(30, 70);
        if (z1 == 30 && x1 != 70) Move(70, 30);
        if (30 < x1 && x1 <= 50 && 50 < z1 && z1 < 70) Move(30, 50);
        if (30 < x1 && x1 < 50 && 30 < z1 && z1 <= 50) Move(50, 30);
        if (50 <= x1 && x1 < 70 && 30 < z1 && z1 < 50) Move(70, 50);
        if (50 < x1 && x1 < 70 && 50 <= z1 && z1 < 70) Move(50, 70);

        //UnityEngine.Debug.Log("X:" + x1 + " Y:" + z1);
        var targets = state["barrels"].Children();
        var targets2 = state["pickups"].Children();
        if (targets2.Count() > 0)
        {
            if ((float)me["hp"] > 50.0f)
            {
                var target2 = targets2.OrderBy(b => Distance(me, b)).Where(b => (int)b["type"] != 1).Select(b => b["pos"])
                 .ToArray();
                if (target2.Count() > 0)
                {
                    float a = (float)target2[0]["x"];
                    float c = (float)target2[0]["z"];
                    Move(a, c);
                }
            }
            else
            {
                var target2 = targets2.OrderBy(b => Distance(me, b)).Select(b => b["pos"])
                 .ToArray();
                if (target2.Count() > 0)
                {
                    float a = (float)target2[0]["x"];
                    float c = (float)target2[0]["z"];
                    Move(a, c);
                }
            }


        }
        //追踪
        var BestEnemies = state["enemies"].Children();
        var BestEnemy = BestEnemies.OrderBy(b => Distance(me, b)).Where(b => ((int)b["state"]&1) !=1).ToArray();
        if (BestEnemy.Count() > 0)
        {
            var enemy = BestEnemy[0];
            float enemyX = (float)enemy["pos"]["x"];
            float enemyZ = (float)enemy["pos"]["z"];
            switch((int)enemy["type"])
            {
                case 0:{
                    if ((int)me["hp"] < (int)enemy["hp"])
                    {
                        break;
                    }

                    float distance = (float)System.Math.Sqrt(Distance(me, enemy));
                    float increaseX = x1 - enemyX;
                    float increaseZ = z1 - enemyZ;
                    if (increaseX == 0.0f)
                    {
                        float trueZ = enemyZ > z1 ? (enemyZ - 9) : (enemyZ + 9);
                        Move(x1, trueZ);
                    }
                    else
                    {
                        float trueX = enemyX + increaseX * 9 / distance;
                        float trueZ = enemyZ + increaseZ * 9 / distance;
                        if (trueX > 0 && trueZ > 0)
                            Move(trueX, trueZ);
                    }
                    break;
                }
                case 1:{
                    break;
                }
                case 2:
                {
                    //UnityEngine.Debug.Log("Tyor");
                    
                    float distance = (float)System.Math.Sqrt(Distance(me, enemy));
                    float increaseX = x1 - enemyX;
                    float increaseZ = z1 - enemyZ;
                    if (increaseX == 0.0f)
                    {
                        float trueZ = enemyZ > z1 ? (enemyZ - 9) : (enemyZ + 9);
                        Move(x1, trueZ);
                    }
                    else
                    {
                        float trueX = enemyX + increaseX * 9 / distance;
                        float trueZ = enemyZ + increaseZ * 9 / distance;
                        if (trueX > 0 && trueZ > 0)
                            Move(trueX, trueZ);
                    }
                    break;
                }
                case 3:
                {
                    //UnityEngine.Debug.Log("yui");

                    float distance = (float)System.Math.Sqrt(Distance(me,enemy));
                    float increaseX = x1 - enemyX;
                    float increaseZ = z1 - enemyZ;
                    if (increaseX == 0.0f)
                    {
                        float trueZ = enemyZ > z1 ? (enemyZ - 9) : (enemyZ + 9);
                        Move(x1, trueZ);
                    }
                    else
                    {
                        float trueX = enemyX + increaseX * 9 / distance;
                        float trueZ = enemyZ + increaseZ * 9 / distance;
                        if(trueX > 0 && trueZ > 0)
                            Move(trueX, trueZ);
                    }
                    
                    break;
                }
                default:{
                    break;
                }
            }
        }


        var enemies = state["enemies"] as JArray;
        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            if (((int)enemy["state"] & 1) == 1)
                continue;
            float x2 = (float)enemy["pos"]["x"];
            float z2 = (float)enemy["pos"]["z"];
            float distan = Distance(me, enemy);
            if (distan > BigRange * BigRange)
            {
                if (distan > 21*21)
                {
                    continue;
                }
                float[] a = maxskill(x1, z1, x2, z2);
                float x = a[0];
                float z = a[1];
                if ((int)me["skills"][1] == 0)
                {
                    UseSkill(1, x, z);
                }
            }
            else
            {
                if ((int)me["skills"][1] == 0)
                {
                    UseSkill(1, x2, z2);
                }
                if (Distance(me, enemy) < shootRange * shootRange)
                {
                    UseSkill(0, (int)enemy["index"]);
                }
            }
 
        }
        if (targets.Count() > 0)
        {
            var target = targets.OrderBy(b => Distance(me, b)).Where(b => Distance(me, b) < shootRange * shootRange).Select(b => (int)b["index"])
            .ToArray();
            if (target.Count() > 0)
            {
                UseSkill(0, target[0]);
            }

        }

    }

    private float Distance(JToken a, JToken b)
    {
        float dx = (float)a["pos"]["x"] - (float)b["pos"]["x"];
        float dz = (float)a["pos"]["z"] - (float)b["pos"]["z"];
        return dx * dx + dz * dz;
    }
    private float[] maxskill(float x1, float z1, float x2, float z2)
    {
        float x = (float)(x1 + 15 * (x2 - x1) / System.Math.Sqrt((x2 - x1) * (x2 - x1) + (z2 - z1) * (z2 - z1)));
        float z = (float)(z1 + 15 * (z2 - z1) / System.Math.Sqrt((x2 - x1) * (x2 - x1) + (z2 - z1) * (z2 - z1)));
        float[] a = { x, z };
        return a;
    }
}
