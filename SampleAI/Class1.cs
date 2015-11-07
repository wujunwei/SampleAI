using System.Linq;
using TuringCup;
using Newtonsoft.Json.Linq;

public class HATeam : AIBase
{

    public HATeam(int a_index) : base(a_index) { }

    public override string TeamName
    {
        get
        {
            return "lei";
        }
    }

    public override Character ChooseCharacter
    {
        get
        {
            return Character.Tyor;
        }
    }

    private float shootRange = 3;
    private float BigRange = 16;
    protected override void Act(JObject state)
    {
        var me = state["me"];
        float x1 = (float)me["pos"]["x"];
        float z1 = (float)me["pos"]["z"];
        /* if (x1 < 30 && z1 < 70) Move(30, 30);
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
         if (50 < x1 && x1 < 70 && 50 <= z1 && z1 < 70) Move(50, 70);*/
        Move(50, 50);

        //UnityEngine.Debug.Log("X:" + x1 + " Y:" + z1);
        var targets = state["barrels"].Children();
        var targets2 = state["pickups"].Children();

        if (targets.Count() > 0)
        {
            var target = targets.OrderBy(b => Distance(me, b)).Select(b => b["pos"])
            .ToArray();
            if (target.Count() > 0)
            {
                var t = targets.OrderBy(b => Distance(me, b))
                    .ToArray();
                float x4 = (float)target[0]["x"];
                float z4 = (float)target[0]["z"];
                Move(x4, z4);
                if (Distance(me, t[0]) < shootRange * shootRange)
                    UseSkill(0);
            }

        }
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

        var enemies = state["enemies"] as JArray;
        bool flag = false;
        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            if (((int)enemy["state"] & 1) == 1)
                continue;
            float x2 = (float)enemy["pos"]["x"];
            float z2 = (float)enemy["pos"]["z"];


            if (Distance(me, enemy) > BigRange * BigRange)
            {
                Move(x2, z2);

            }
            else
            {
                float x3 = (float)me["pos"]["x"];
                float z3 = (float)me["pos"]["z"];

                if ((int)me["skills"][1] == 0)
                {
                    int b = (int)enemy["index"];

                    flag = true;
                    UseSkill(1, b);
                }

                if (Distance(me, enemy) < shootRange * shootRange)
                {
                    // UnityEngine.Debug.Log("enemyposition X:" + enemy["pos"]["x"] + " z:" + enemy["pos"]["z"] + " myposition X:" + me["pos"]["x"] + "Z:" + me["pos"]["z"]);
                    UseSkill(0);
                }
            }
            if (flag)
            {
                break;
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
