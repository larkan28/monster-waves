using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActorManager : Singleton<ActorManager>
{
    public event Action<Actor> OnActorAdded;
    public List<Actor> Actors = new();

    public void Add(Actor actor)
    {
        if (actor != null)
        {
            Actors.Add(actor);
            OnActorAdded?.Invoke(actor);
        }
    }

    public void Remove(Actor actor)
    {
        if (actor == null)
            return;

        for (int i = 0; i < Actors.Count; i++)
        {
            if (Actors[i] == actor)
            {
                Actors.RemoveAt(i);
                break;
            }
        }
    }

    public Actor Find(string name)
    {
        return Actors.SingleOrDefault(x => x.Name == name);
    }

    public Actor Find(Vector3 center, float radius, int teamId, bool closest = false)
    {
        if (closest)
        {
            Actor[] actors = FindAll(center, radius, teamId);

            if (actors.Length < 1)
                return null;

            int closestId = 0;
            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < actors.Length; i++)
            {
                float distance = Vector3.Distance(actors[i].transform.position, center);

                if (distance < closestDistance)
                {
                    closestId = i;
                    closestDistance = distance;
                }
            }

            return actors[closestId];
        }

        return Actors.Find(x => Vector3.Distance(x.transform.position, center) <= radius && x.Team == teamId);
    }

    public Actor[] FindAll(Vector3 center, float radius, int teamId)
    {
        return Actors.FindAll(x => Vector3.Distance(x.transform.position, center) <= radius && x.Team == teamId).ToArray();
    }
}
