﻿using System.IO;
using ProjectEternity.Core.Item;

namespace ProjectEternity.GameScreens.SorcererStreetScreen
{
    public sealed class SorcererStreesBeforeAttackRequirement : SorcererStreetRequirement
    {
        public SorcererStreesBeforeAttackRequirement()
            : this(null)
        {
        }

        public SorcererStreesBeforeAttackRequirement(SorcererStreetBattleContext GlobalContext)
            : base(ActionPanelBattleAttackPhase.BeforeAttackRequirement, GlobalContext)
        {
        }

        public override bool CanActivatePassive()
        {
            return false;
        }

        public override BaseSkillRequirement Copy()
        {
            return new SorcererStreesBeforeAttackRequirement(GlobalContext);
        }

        protected override void DoSave(BinaryWriter BW)
        {
        }

        protected override void Load(BinaryReader BR)
        {
        }
    }
}
