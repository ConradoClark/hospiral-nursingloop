using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Unity.Extensions;
using Licht.Unity.Objects;
public class HospitalSection : BaseGameObject
{
    public bool HasBeds { get; private set; }

    private HospitalSectionManager _sectionManager;

    protected override void OnAwake()
    {
        base.OnAwake();
        _sectionManager = _sectionManager.FromScene();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _sectionManager.AddSection(this);
    }
}
