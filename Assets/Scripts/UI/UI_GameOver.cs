using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Licht.Impl.Orchestration;
using Licht.Unity.Objects;

public class UI_GameOver : BaseGameObject
{
    public IEnumerable<IEnumerable<Action>> Show()
    {
        gameObject.SetActive(true);
        yield return TimeYields.WaitSeconds(UITimer, 0.5f);
    }
}


