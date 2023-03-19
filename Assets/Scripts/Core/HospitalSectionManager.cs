using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Objects;

public class HospitalSectionManager : BaseGameObject
{
    private List<HospitalSection> _sections;

    protected override void OnAwake()
    {
        base.OnAwake();
        _sections ??= new List<HospitalSection>();
    }

    public void AddSection(HospitalSection section)
    {
        _sections ??= new List<HospitalSection>();
        _sections.Add(section);
    }

    public void Clear()
    {
        _sections ??= new List<HospitalSection>();
        _sections.Clear();
    }
}
