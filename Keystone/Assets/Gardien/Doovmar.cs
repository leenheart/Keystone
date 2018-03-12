using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doovmar : Personnage {

    public Doovmar(Vector3 position) : base ("Doovmar", 1000,100,25,5,100,5,position)
    {
        vectRangeSpell1 = Vector3.zero;
        vectRangeSpell2 = Vector3.zero;
        vectRangeSpell3 = Vector3.zero;
        vectRangeSpell4 = Vector3.zero;
    }
}
