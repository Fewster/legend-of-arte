using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour, IEnumerable<AbilityComponent>
{
    [SerializeField]
    protected Entity entity;

    [SerializeField] // TODO: Not serialized, needs to just show in inspector to help debugging...
    private List<AbilityComponent> abilities;

    public int Count { get { return abilities.Count; } }

    public AbilityComponent this[int index]
    {
        get { return abilities[index]; }
    }

    private void Awake()
    {
        abilities = new List<AbilityComponent>();
    }

    private void OnDestroy()
    {
        foreach(var ability in abilities)
        {
            Destroy(ability);
        }
    }

    public void Add(AbilityComponent source)
    {
        var instance = Instantiate(source);

        abilities.Add(instance);
    }

    public IEnumerator<AbilityComponent> GetEnumerator()
    {
        return abilities.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return abilities.GetEnumerator();
    }
}
