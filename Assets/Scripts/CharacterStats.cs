using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stores character stats across scenes
public static class CharacterStats
{
    private static PlayerCharacter samurai;
    private static PlayerCharacter shinobi;
    private static PlayerCharacter archer;
    private static PlayerCharacter ninja;
    private static Vector3 playerPosition = Vector3.zero;

    public static PlayerCharacter Samurai
    {
        get
        {
            return samurai;
        }
        set
        {
            samurai = value;
        }
    }

    public static PlayerCharacter Shinobi
    {
        get
        {
            return shinobi;
        }
        set
        {
            shinobi = value;
        }
    }

    public static PlayerCharacter Archer
    {
        get
        {
            return archer;
        }
        set
        {
            archer = value;
        }
    }

    public static PlayerCharacter Ninja
    {
        get
        {
            return ninja;
        }
        set
        {
            ninja = value;
        }
    }

    public static Vector3 PlayerPosition
    {
        get
        {
            return playerPosition;
        }
        set 
        {  
            playerPosition = value;
        }
    }
}
