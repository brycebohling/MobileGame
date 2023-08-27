using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallTypesHelper
{
    public static HashSet<int> wallTop = new HashSet<int>
    {
        0b00011100, // Only bottom 3
        0b00001000,
        0b00011000,
        0b00001100,
        0b11011101,
        0b00011111,
        0b10011111,
        0b11001101,
        0b11001111,
        0b11011001,
        0b11011100,
    };

    public static HashSet<int> wallBottom = new HashSet<int>
    {
        0b11000001, // Only top 3
        0b10000000,
        0b10000001,
        0b11000000,
    };

    public static HashSet<int> wallSideRight = new HashSet<int>
    {
        0b00000111,
        0b00000010,
        0b00000011,
        0b00000110,
    };

    public static HashSet<int> wallSideLeft = new HashSet<int>
    {
        0b01110000,
        0b00100000,
        0b000110000,
        0b01100000,
    };

    public static HashSet<int> wallInnerCornerDownRight = new HashSet<int>
    {
        0b00111000,
        0b00111100,
        0b01111000,
        0b01111100,

        0b01011100,
    };

    public static HashSet<int> wallInnerCornerDownLeft = new HashSet<int>
    {
        0b00001110,
        0b00001111,
        0b00011110,
        0b00011111,

        0b11000101,
        0b00011101,
    };

    public static HashSet<int> wallInnerCornerTopRight = new HashSet<int>
    {
        0b11100000,
        0b11100001,
        0b11110000,
        0b11110001,
        
        0b11010001,
        0b01110001,
    };

    public static HashSet<int> wallInnerCornerTopLeft = new HashSet<int>
    {
        0b10000011,
        0b11000011,
        0b10000111,
        0b11000111,

        0b01000111,
    };

    public static HashSet<int> wallOuterCornerUpRight = new HashSet<int>
    {
        0b00000100, // Only diagonal down left


        // 0b00000100,
        // 0b00000101
    };

    public static HashSet<int> wallOuterCornerUpLeft = new HashSet<int>
    {
        0b00010000, // Only diagonal down right


        // 0b00010000,
        // 0b01010000,
    };

    public static HashSet<int> wallOuterCornerDownRight = new HashSet<int>
    {
        
        0b00000001 // Only diagonal top left


        // 0b00000001
    };
    
    public static HashSet<int> wallOuterCornerDownLeft = new HashSet<int>
    {
        0b01000000, // Only diagonal top right


        // 0b01000000
    };

    public static HashSet<int> wallFull = new HashSet<int>
    {
        0b11111111, // Nothing around
        0b10111111, // E DTR
        0b11101111, // E DDR
        0b11111011, // E DDL
        0b11111110, // E DTL
    };

    public static HashSet<int> wallFillRight = new HashSet<int>
    {
        0b11111000,
        0b11111001,
        0b11111100,
        0b11111101,
    };

    public static HashSet<int> wallFillLeft = new HashSet<int>
    {
        0b10001111,
        0b11001111,
        0b10011111,
        0b11011111,
    };
}
