﻿using System.IO;
using System.ComponentModel;
using ProjectEternity.Core.Item;

namespace ProjectEternity.GameScreens.DeathmatchMapScreen
{
    public sealed class WithinAttackRangeRequirement : DeathmatchSkillRequirement
    {
        private int _Range;
        private bool _ConsiderWalls;

        public WithinAttackRangeRequirement()
            : this(null)
        {
        }

        public WithinAttackRangeRequirement(DeathmatchContext Context)
            : base(WithinAttackRangeRequirementName, Context)
        {
        }

        protected override void Load(BinaryReader BR)
        {
            _Range = BR.ReadInt32();
            _ConsiderWalls = BR.ReadBoolean();
        }

        protected override void DoSave(BinaryWriter BW)
        {
            BW.Write(_Range);
            BW.Write(_ConsiderWalls);
        }

        public override BaseSkillRequirement Copy()
        {
            WithinAttackRangeRequirement NewSkillEffect = new WithinAttackRangeRequirement(Context);
            NewSkillEffect._Range = _Range;
            NewSkillEffect._ConsiderWalls = _ConsiderWalls;

            return NewSkillEffect;
        }

        public override bool CanActicateManually(string ManualActivationName)
        {
            return base.CanActicateManually(AfterAttackRequirementName);
        }

        [CategoryAttribute("Requirement Attributes"),
        DescriptionAttribute(".")]
        public int Range
        {
            get { return _Range; }
            set { _Range = value; }
        }

        [CategoryAttribute("Requirement Attributes"),
        DescriptionAttribute(".")]
        public bool ConsiderWalls
        {
            get { return _ConsiderWalls; }
            set { _ConsiderWalls = value; }
        }
    }
}
