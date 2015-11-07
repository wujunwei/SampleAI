using System.Linq;
using TuringCup;
using Newtonsoft.Json.Linq;
public class DefaultTea : AIBase
{

    public DefaultTea(int a_index) : base(a_index) { }

    public override string TeamName
    {
        get
        {
            return "bing";
        }
    }

    public override Character ChooseCharacter
    {
        get
        {
            return Character.Yui;
        }
    }

    private float shootRange = 10;

    protected override void Act(JObject state)
    {
        Move(50, 50);
        var me = state["me"];
        var targets = state["barrels"].Children();

        if (targets.Count() > 0)
        {
            float x = targets.OrderBy(b => Distance(me, b))
            .Where(b => Distance(me, b) < shootRange * shootRange)
            .Select(b => (float)b["pos"]["x"])
            .FirstOrDefault();
            float z = targets.OrderBy(b => Distance(me, b))
            .Where(b => Distance(me, b) < shootRange * shootRange)
            .Select(b => (float)b["pos"]["z"])
            .FirstOrDefault();
            UseSkill(0, x, z);
        }
        var pickups = state["pickups"].Children();
        if (pickups.Count() > 0)
        {
            float x = pickups.OrderBy(b => Distance(me, b))
                .Select(b => (float)b["pos"]["x"])
                .FirstOrDefault();
            float z = pickups.OrderBy(b => Distance(me, b))
                .Select(b => (float)b["pos"]["z"])
                .FirstOrDefault();
            UnityEngine.Debug.Log("x:" + x + " z:" + z);
            Move(x, z);
        }
        else { Move(50, 50); }
        var enemies = state["enemies"] as JArray;
        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            float x = (float)enemy["pos"]["x"];
            float z = (float)enemy["pos"]["z"];
            if ((int)me["skills"][1] == 0)
            {

                UseSkill(1);
            }
            else { UseSkill(0, (int)enemy["index"]); }
        }

    }

    private float Distance(JToken a, JToken b)
    {
        float dx = (float)a["pos"]["x"] - (float)b["pos"]["x"];
        float dz = (float)a["pos"]["z"] - (float)b["pos"]["z"];
        return dx * dx + dz * dz;
    }

}
