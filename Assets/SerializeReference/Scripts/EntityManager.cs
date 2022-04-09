using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeReference]
    List<EntityBase> entities = new List<EntityBase>();

    [ContextMenu("Add Turtle")] void AddTurtle() => entities.Add(new Turtle());
    [ContextMenu("Add Bird")] void AddBird() => entities.Add(new Bird());
}