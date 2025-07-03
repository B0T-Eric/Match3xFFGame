using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Character
{
    class YellowCharacter : BaseCharacter
    {
        public override void Init()
        {
            manaType = ManaType.Yellow;
            hexColour = "FFEA00";
            base.Init();
        }
    }
}
