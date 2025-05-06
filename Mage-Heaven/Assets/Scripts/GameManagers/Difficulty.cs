using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulties
{
    NoviceMage,
    AprenticeMage,
    Wizard,
}

public class Difficulty : MonoBehaviour
{
    public static Difficulty instance;
    public Difficulties difficulty;

    private void Awake()
    {
        instance = this;
    }

    public void SetDifficulty(Difficulties difficulty)
    {
        this.difficulty = difficulty;
    }

    public int GetDifficulty()
    {
        switch (difficulty)
        {
            case Difficulties.NoviceMage:
                return 0;
            case Difficulties.AprenticeMage:
                return 1;
            case Difficulties.Wizard:
                return 2;
        }
        return 0; // This needs a return type 
    }
}
