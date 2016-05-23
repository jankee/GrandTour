using UnityEngine;
using System.Collections;

public class Mashine
{
    public string name;
    public int rank;
    public int dur;
    public int spd;
    public int acc;
    public int hndl;
    public int lvup;
    public int upg;
    public int exp;
    public int adv;
    public float rep;
    public float onRoad;
    public float offRoad;
    public float icyRoad;
    public int cost;
    public string unlock;

    public Mashine(string name, int rank, int dur, int spd, int acc, int hndl, int lvup, int upg,
        int exp, int adv, float rep, float onRoad, float offRoad, float icyRoad, int cost, string unlock)
    {
        this.name = name;
        this.rank = rank;
        this.dur = dur;
        this.spd = spd;
        this.acc = acc;
        this.hndl = hndl;
        this.lvup = lvup;
        this.upg = upg;
        this.exp = exp;
        this.rep = rep;
        this.onRoad = onRoad;
        this.offRoad = offRoad;
        this.icyRoad = icyRoad;
        this.cost = cost;
        this.unlock = unlock;
    }
}
