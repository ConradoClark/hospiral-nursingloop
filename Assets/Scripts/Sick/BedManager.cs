using System.Collections.Generic;
using System.Linq;
using Licht.Unity.Objects;
using Random = UnityEngine.Random;

public class BedManager : BaseGameObject
{
    private List<Bed> _beds;
    protected override void OnAwake()
    {
        base.OnAwake();
        _beds ??= new List<Bed>();
    }

    public void AddBed(Bed bed)
    {
        _beds ??= new List<Bed>();
        _beds.Add(bed);
    }

    public void RemoveBed(Bed bed)
    {
        _beds ??= new List<Bed>();
        _beds.Remove(bed);
    }

    public Bed GetRandomUnoccupiedBed()
    {
        _beds ??= new List<Bed>();
        return _beds.OrderBy(_ => Random.value).FirstOrDefault(bed => !bed.IsOccupied);
    }

    public bool IsHospitalEmpty => _beds.All(bed => !bed.IsOccupied);
}
