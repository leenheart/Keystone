using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour {
    
    public enum Type {deplacement, spell1, spell2, spell3, spell4};
    public Type type;
    public Vector3 coordonnees;



    public Action(Type enumeration, Vector3 vector_coor)
    {
        type = enumeration;
        coordonnees = vector_coor;
    }


}
