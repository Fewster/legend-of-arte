using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour, IEnumerable<Ability>
{
    [SerializeField]
    protected Entity entity;

    [SerializeField] // TODO: Not serialized, needs to just show in inspector to help debugging...
    private List<Ability> abilities;

    public int Count { get { return abilities.Count; } }

    public Ability this[int index]
    {
        get { return abilities[index]; }
    }

    private void Awake()
    {
        abilities = new List<Ability>();
    }

    private void OnDestroy()
    {
        foreach(var ability in abilities)
        {
            Destroy(ability);
        }
    }

    public void Add(Ability source)
    {
        var instance = Instantiate(source);

        abilities.Add(instance);
    }

    public IEnumerator<Ability> GetEnumerator()
    {
        return abilities.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return abilities.GetEnumerator();
    }
}
