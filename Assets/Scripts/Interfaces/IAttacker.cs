using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    void StartAttack(Vector2 closestPlayer);

    void EndAttack();
}
