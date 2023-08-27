using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallTypesHelper
{
    public static HashSet<int> wallTop = new()
    {
        0b00011100,
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
        0b10011101,
        0b11001100, 
        0b10001101,
        0b10011001,
    };

    public static HashSet<int> wallBottom = new()
    {
        0b11000001,
        0b10000000,
        0b10000001,
        0b11000000,
    };

    public static HashSet<int> wallSideRight = new()
    {
        0b00000111,
        0b00000010,
        0b00000011,
        0b00000110,
        0b00000101,
    };

    public static HashSet<int> wallSideLeft = new()
    {
        0b01110000,
        0b00100000,
        0b000110000,
        0b01100000,
    };

    public static HashSet<int> wallInnerCornerDownRight = new()
    {
        0b00111000,
        0b00111100,
        0b01111000,
        0b01111100,

        0b01011100,
        0b01110100,
    };

    public static HashSet<int> wallInnerCornerDownLeft = new()
    {
        0b00001110,
        0b00001111,
        0b00011110,
        0b00011111,

        0b11000101,
        0b00011101,
        0b00011001,
    };

    public static HashSet<int> wallInnerCornerTopRight = new()
    {
        0b11100000,
        0b11100001,
        0b11110000,
        0b11110001,
        
        0b11010001,
        0b01110001,
        0b10010001,
    };

    public static HashSet<int> wallInnerCornerTopLeft = new()
    {
        0b10000011,
        0b11000011,
        0b10000111,
        0b11000111,

        0b01000111,
        0b00010111,
    };

    public static HashSet<int> wallOuterCornerUpRight = new()
    {
        0b00000100,
    };

    public static HashSet<int> wallOuterCornerUpLeft = new()
    {
        0b00010000,
    };

    public static HashSet<int> wallOuterCornerDownRight = new()
    {
        0b00000001,
    };
    
    public static HashSet<int> wallOuterCornerDownLeft = new()
    {
        0b01000000,
    };

    public static HashSet<int> wallFull = new()
    {
        0b11111111,
        0b10111111,
        0b11101111,
        0b11111011,
        0b11111110,
    };

    public static HashSet<int> wallFillRight = new()
    {
        0b11111000,
        0b11111001,
        0b11111100,
        0b11111101,
    };

    public static HashSet<int> wallFillLeft = new()
    {
        0b10001111,
        0b11001111,
        0b10011111,
        0b11011111,
    };

    public static HashSet<int> wallFillUp = new()
    {
        0b11110111,
        0b11110011,
    };

    public static HashSet<int> wallFillDown = new()
    {
        0b01111111,
        0b01111110,
    };

    public static HashSet<int> wallFillUpAndDown = new()
    {
        0b01110111,
        0b00110111,
        0b01100111,
        0b01110011,
        0b01110110,
        0b00110011,
    };
}
