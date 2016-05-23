using UnityEngine;
using System.Collections;

public class Driver
{
    public string name;
    public int padal;
    public int shift;
    public int steer;
    public int appl;
    public int tech;
    public int anlys;
    public int pss;
    public int ata;
    public int salary;

    public Driver(string name, int padal, int shift, int steer, int appl, int tech, int anlys, int pss, int ata, int salary)
    {
        this.name = name;
        this.padal = padal;
        this.shift = shift;
        this.steer = steer;
        this.appl = appl;
        this.tech = tech;
        this.anlys = anlys;
        this.pss = pss;
        this.ata = ata;
        this.salary = salary;
    }
}
