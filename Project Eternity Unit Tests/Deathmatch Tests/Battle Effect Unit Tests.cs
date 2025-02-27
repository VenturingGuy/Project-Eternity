﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectEternity.Core;
using ProjectEternity.Core.Item;
using ProjectEternity.Core.Effects;
using ProjectEternity.GameScreens.BattleMapScreen;
using ProjectEternity.GameScreens.DeathmatchMapScreen;
using ProjectEternity.Core.Units;

namespace ProjectEternity.UnitTests
{
    public partial class DeathmatchTests
    {
        #region StatusEffect

        [TestMethod]
        public void TestStatusEffectMEL()
        {
            StatusEffect NewEffect = (StatusEffect)DummyMap.DicEffect[StatusEffect.Name].Copy();
            NewEffect.Value = "100";
            NewEffect.StatusType = StatusTypes.MEL;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.BonusMEL, 100);
        }

        [TestMethod]
        public void TestStatusEffectRNG()
        {
            StatusEffect NewEffect = (StatusEffect)DummyMap.DicEffect[StatusEffect.Name].Copy();
            NewEffect.Value = "100";
            NewEffect.StatusType = StatusTypes.RNG;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.BonusRNG, 100);
        }

        [TestMethod]
        public void TestStatusEffectDEF()
        {
            StatusEffect NewEffect = (StatusEffect)DummyMap.DicEffect[StatusEffect.Name].Copy();
            NewEffect.Value = "100";
            NewEffect.StatusType = StatusTypes.DEF;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.BonusDEF, 100);
        }

        [TestMethod]
        public void TestStatusEffectSKL()
        {
            StatusEffect NewEffect = (StatusEffect)DummyMap.DicEffect[StatusEffect.Name].Copy();
            NewEffect.Value = "100";
            NewEffect.StatusType = StatusTypes.SKL;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.BonusSKL, 100);
        }

        [TestMethod]
        public void TestStatusEffectEVA()
        {
            StatusEffect NewEffect = (StatusEffect)DummyMap.DicEffect[StatusEffect.Name].Copy();
            NewEffect.Value = "100";
            NewEffect.StatusType = StatusTypes.EVA;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.BonusEVA, 100);
        }

        [TestMethod]
        public void TestStatusEffectHIT()
        {
            StatusEffect NewEffect = (StatusEffect)DummyMap.DicEffect[StatusEffect.Name].Copy();
            NewEffect.Value = "100";
            NewEffect.StatusType = StatusTypes.HIT;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.BonusHIT, 100);
        }

        #endregion

        #region UnitStatEffect

        [TestMethod]
        public void TestUnitStatEffectAbsoluteMaxHP()
        {
            UnitStatEffect NewEffect = (UnitStatEffect)DummyMap.DicEffect[UnitStatEffect.Name].Copy();
            NewEffect.Value = "1000";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;
            NewEffect.UnitStat = Core.Effects.UnitStats.MaxHP;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.HPMaxModifier, 1000);
        }

        [TestMethod]
        public void TestUnitStatEffectAbsoluteMaxEN()
        {
            UnitStatEffect NewEffect = (UnitStatEffect)DummyMap.DicEffect[UnitStatEffect.Name].Copy();
            NewEffect.Value = "1000";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;
            NewEffect.UnitStat = Core.Effects.UnitStats.MaxEN;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.ENMaxModifier, 1000);
        }

        [TestMethod]
        public void TestUnitStatEffectAbsoluteArmor()
        {
            UnitStatEffect NewEffect = (UnitStatEffect)DummyMap.DicEffect[UnitStatEffect.Name].Copy();
            NewEffect.Value = "1000";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;
            NewEffect.UnitStat = Core.Effects.UnitStats.Armor;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.ArmorModifier, 1000);
        }

        [TestMethod]
        public void TestUnitStatEffectAbsoluteMobility()
        {
            UnitStatEffect NewEffect = (UnitStatEffect)DummyMap.DicEffect[UnitStatEffect.Name].Copy();
            NewEffect.Value = "1000";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;
            NewEffect.UnitStat = Core.Effects.UnitStats.Mobility;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.MobilityModifier, 1000);
        }

        [TestMethod]
        public void TestUnitStatEffectAbsoluteMaxMV()
        {
            UnitStatEffect NewEffect = (UnitStatEffect)DummyMap.DicEffect[UnitStatEffect.Name].Copy();
            NewEffect.Value = "1000";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;
            NewEffect.UnitStat = Core.Effects.UnitStats.MaxMV;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.MVMaxModifier, 1000);
        }

        [TestMethod]
        public void TestUnitStatEffectRelativeMaxHP()
        {
            UnitStatEffect NewEffect = (UnitStatEffect)DummyMap.DicEffect[UnitStatEffect.Name].Copy();
            NewEffect.Value = "1";
            NewEffect.NumberType = Operators.NumberTypes.Relative;
            NewEffect.UnitStat = Core.Effects.UnitStats.MaxHP;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.HPMaxModifier, GlobalDeathmatchContext.EffectOwnerUnit.MaxHP / 2);
        }

        [TestMethod]
        public void TestUnitStatEffectRelativeMaxEN()
        {
            UnitStatEffect NewEffect = (UnitStatEffect)DummyMap.DicEffect[UnitStatEffect.Name].Copy();
            NewEffect.Value = "1";
            NewEffect.NumberType = Operators.NumberTypes.Relative;
            NewEffect.UnitStat = Core.Effects.UnitStats.MaxEN;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.ENMaxModifier, GlobalDeathmatchContext.EffectOwnerUnit.MaxEN / 2);
        }

        [TestMethod]
        public void TestUnitStatEffectRelativeArmor()
        {
            UnitStatEffect NewEffect = (UnitStatEffect)DummyMap.DicEffect[UnitStatEffect.Name].Copy();
            NewEffect.Value = "1";
            NewEffect.NumberType = Operators.NumberTypes.Relative;
            NewEffect.UnitStat = Core.Effects.UnitStats.Armor;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.ArmorModifier, GlobalDeathmatchContext.EffectOwnerUnit.Armor / 2);
        }

        [TestMethod]
        public void TestUnitStatEffectRelativeMobility()
        {
            UnitStatEffect NewEffect = (UnitStatEffect)DummyMap.DicEffect[UnitStatEffect.Name].Copy();
            NewEffect.Value = "1";
            NewEffect.NumberType = Operators.NumberTypes.Relative;
            NewEffect.UnitStat = Core.Effects.UnitStats.Mobility;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.MobilityModifier, GlobalDeathmatchContext.EffectOwnerUnit.Mobility / 2);
        }

        [TestMethod]
        public void TestUnitStatEffectRelativeMaxMV()
        {
            UnitStatEffect NewEffect = (UnitStatEffect)DummyMap.DicEffect[UnitStatEffect.Name].Copy();
            NewEffect.Value = "1";
            NewEffect.NumberType = Operators.NumberTypes.Relative;
            NewEffect.UnitStat = Core.Effects.UnitStats.MaxMV;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.MVMaxModifier, GlobalDeathmatchContext.EffectOwnerUnit.MaxMovement / 2);
        }

        #endregion

        [TestMethod]
        public void GivenAbsoluteWillEffectChange_WhenActivated_ThenWillAndBonusWillHasChanged()
        {
            WillEffect NewEffect = (WillEffect)DummyMap.DicEffect[WillEffect.Name].Copy();
            NewEffect.WillValue = "10";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Will, 110);
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.WillBonus, 10);
        }

        [TestMethod]
        public void GivenRelativeWillEffectChange_WhenActivated_ThenWillAndBonusWillHasChanged()
        {
            WillEffect NewEffect = (WillEffect)DummyMap.DicEffect[WillEffect.Name].Copy();
            NewEffect.WillValue = "0.10";
            NewEffect.NumberType = Operators.NumberTypes.Relative;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Will, 110);
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.WillBonus, 10);
        }

        [TestMethod]
        public void TestWillLimitBreakEffect()
        {
            WillLimitBreakEffect NewEffect = (WillLimitBreakEffect)DummyMap.DicEffect[WillLimitBreakEffect.Name].Copy();
            NewEffect.Value = "10";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.MaxWill, 160);
        }

        [TestMethod]
        public void TestSPEffectAbsolute()
        {
            SPEffect NewEffect = (SPEffect)DummyMap.DicEffect[SPEffect.Name].Copy();
            NewEffect.SPValue = "10";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            GlobalDeathmatchContext.EffectOwnerUnit.Pilot.SP = 20;

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.SP, 30);
        }

        [TestMethod]
        public void TestSPEffectRelative()
        {
            SPEffect NewEffect = (SPEffect)DummyMap.DicEffect[SPEffect.Name].Copy();
            NewEffect.SPValue = "0.10";
            NewEffect.NumberType = Operators.NumberTypes.Relative;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            GlobalDeathmatchContext.EffectOwnerUnit.Pilot.SP = 20;

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.SP, 22);
        }

        [TestMethod]
        public void TestMaxSPEffectAbsolute()
        {
            MaxSPEffect NewEffect = (MaxSPEffect)DummyMap.DicEffect[MaxSPEffect.Name].Copy();
            NewEffect.MaxSPValue = "10";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.MaxSP, 60);
        }

        [TestMethod]
        public void TestMaxSPEffectRelative()
        {
            MaxSPEffect NewEffect = (MaxSPEffect)DummyMap.DicEffect[MaxSPEffect.Name].Copy();
            NewEffect.MaxSPValue = "0.10";
            NewEffect.NumberType = Operators.NumberTypes.Relative;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Pilot.MaxSP, 55);
        }

        [TestMethod]
        public void TestHitRateEffect()
        {
            HitRateEffect NewEffect = (HitRateEffect)DummyMap.DicEffect[HitRateEffect.Name].Copy();
            NewEffect.Value = "10";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.AccuracyModifier, 10);
        }

        [TestMethod]
        public void TestCriticalHitRateEffect()
        {
            CriticalHitRateEffect NewEffect = (CriticalHitRateEffect)DummyMap.DicEffect[CriticalHitRateEffect.Name].Copy();
            NewEffect.Value = "10";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.CriticalHitRateModifier, 10);
        }

        [TestMethod]
        public void TestCriticalHitAlwaysSucceedEffect()
        {
            CriticalHitAlwaysSucceedEffect NewEffect = (CriticalHitAlwaysSucceedEffect)DummyMap.DicEffect[CriticalHitAlwaysSucceedEffect.Name].Copy();
            

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.IsTrue(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.CriticalAlwaysSucceed);
        }

        [TestMethod]
        public void TestCriticalHitAlwaysFailEffect()
        {
            CriticalHitAlwaysFailEffect NewEffect = (CriticalHitAlwaysFailEffect)DummyMap.DicEffect[CriticalHitAlwaysFailEffect.Name].Copy();

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.IsTrue(GlobalDeathmatchContext.EffectOwnerUnit.Boosts.CriticalAlwaysFail);
        }

        [TestMethod]
        public void TestEvasionRateEffect()
        {
            EvasionRateEffect NewEffect = (EvasionRateEffect)DummyMap.DicEffect[EvasionRateEffect.Name].Copy();
            NewEffect.Value = "10";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(10, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.EvasionModifier);
        }

        [TestMethod]
        public void TestBaseDamageEffectAbsolute()
        {
            BaseDamageEffect NewEffect = (BaseDamageEffect)DummyMap.DicEffect[BaseDamageEffect.Name].Copy();
            NewEffect.BaseDamageValue = "10";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(10, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.BaseDamageModifier);
        }

        [TestMethod]
        public void TestBaseDamageEffectRelative()
        {
            BaseDamageEffect NewEffect = (BaseDamageEffect)DummyMap.DicEffect[BaseDamageEffect.Name].Copy();
            NewEffect.BaseDamageValue = "10";
            NewEffect.NumberType = Operators.NumberTypes.Relative;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(10, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.BaseDamageMultiplier);
        }

        [TestMethod]
        public void TestDamageTakenEffectAbsolute()
        {
            DamageTakenEffect NewEffect = (DamageTakenEffect)DummyMap.DicEffect[DamageTakenEffect.Name].Copy();
            NewEffect.Value = "10";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(10, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.FinalDamageTakenFixedModifier);
        }

        [TestMethod]
        public void TestDamageTakenEffectRelative()
        {
            DamageTakenEffect NewEffect = (DamageTakenEffect)DummyMap.DicEffect[DamageTakenEffect.Name].Copy();
            NewEffect.Value = "10";
            NewEffect.NumberType = Operators.NumberTypes.Relative;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(10, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.BaseDamageTakenReductionMultiplier);
        }

        [TestMethod]
        public void TestFinalDamageEffectAbsolute()
        {
            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1000, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.FinalDamageModifier);
        }

        [TestMethod]
        public void TestFinalDamageEffectRelative()
        {
            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "2";
            NewEffect.NumberType = Operators.NumberTypes.Relative;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(2, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.FinalDamageMultiplier);
        }

        [TestMethod]
        public void TestENCostEffect()
        {
            ENCostEffect NewEffect = (ENCostEffect)DummyMap.DicEffect[ENCostEffect.Name].Copy();
            NewEffect.Value = "2";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(2, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.ENCostModifier);
        }

        [TestMethod]
        public void TestMaxAmmoEffect()
        {
            MaxAmmoEffect NewEffect = (MaxAmmoEffect)DummyMap.DicEffect[MaxAmmoEffect.Name].Copy();
            NewEffect.Value = "2";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(2, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.AmmoMaxModifier);
        }

        [TestMethod]
        public void TestMVEffect()
        {
            MVEffect NewEffect = (MVEffect)DummyMap.DicEffect[MVEffect.Name].Copy();
            NewEffect.Value = "2";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(2, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.MovementModifier);
        }

        [TestMethod]
        public void TestHPRegenEffectAbsolute()
        {
            HPRegenEffect NewEffect = (HPRegenEffect)DummyMap.DicEffect[HPRegenEffect.Name].Copy();
            NewEffect.HPRegenValue = "20";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            GlobalDeathmatchContext.EffectOwnerUnit.DamageUnit(500);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(9520, GlobalDeathmatchContext.EffectOwnerUnit.HP);
        }

        [TestMethod]
        public void TestHPRegenEffectRelative()
        {
            HPRegenEffect NewEffect = (HPRegenEffect)DummyMap.DicEffect[HPRegenEffect.Name].Copy();
            NewEffect.HPRegenValue = "0.01";
            NewEffect.NumberType = Operators.NumberTypes.Relative;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            GlobalDeathmatchContext.EffectOwnerUnit.DamageUnit(500);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(9600, GlobalDeathmatchContext.EffectOwnerUnit.HP);
        }

        [TestMethod]
        public void TestENRegenEffectAbsolute()
        {
            ENRegenEffect NewEffect = (ENRegenEffect)DummyMap.DicEffect[ENRegenEffect.Name].Copy();
            NewEffect.ENRegenValue = "20";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            GlobalDeathmatchContext.EffectOwnerUnit.ConsumeEN(50);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(170, GlobalDeathmatchContext.EffectOwnerUnit.EN);
        }

        [TestMethod]
        public void TestENRegenEffectRelative()
        {
            ENRegenEffect NewEffect = (ENRegenEffect)DummyMap.DicEffect[ENRegenEffect.Name].Copy();
            NewEffect.ENRegenValue = "0.01";
            NewEffect.NumberType = Operators.NumberTypes.Relative;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            GlobalDeathmatchContext.EffectOwnerUnit.ConsumeEN(50);
            
            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(152, GlobalDeathmatchContext.EffectOwnerUnit.EN);
        }

        [TestMethod]
        public void TestAmmoRegenEffectAbsolute()
        {
            AmmoRegenEffect NewEffect = (AmmoRegenEffect)DummyMap.DicEffect[AmmoRegenEffect.Name].Copy();
            NewEffect.AmmoRegenValue = "2";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            GlobalDeathmatchContext.EffectOwnerUnit.ListAttack[0].Ammo = 0;

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(2, GlobalDeathmatchContext.EffectOwnerUnit.ListAttack[0].Ammo);
        }

        [TestMethod]
        public void TestAmmoRegenEffectRelative()
        {
            AmmoRegenEffect NewEffect = (AmmoRegenEffect)DummyMap.DicEffect[AmmoRegenEffect.Name].Copy();
            NewEffect.AmmoRegenValue = "0.1";
            NewEffect.NumberType = Operators.NumberTypes.Relative;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            GlobalDeathmatchContext.EffectOwnerUnit.ListAttack[0].Ammo = 0;

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, GlobalDeathmatchContext.EffectOwnerUnit.ListAttack[0].Ammo);
        }

        [TestMethod]
        public void TestEXPEffect()
        {
            EXPEffect NewEffect = (EXPEffect)DummyMap.DicEffect[EXPEffect.Name].Copy();
            NewEffect.Value = "50";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, GlobalDeathmatchContext.EffectOwnerUnit.ListAttack[0].Ammo);
        }

        [TestMethod]
        public void TestMoneyEffect()
        {
            MoneyEffect NewEffect = (MoneyEffect)DummyMap.DicEffect[MoneyEffect.Name].Copy();
            NewEffect.Value = "50";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, GlobalDeathmatchContext.EffectOwnerUnit.ListAttack[0].Ammo);
        }

        [TestMethod]
        public void TestAttackRangeEffect()
        {
            AttackRangeEffect NewEffect = (AttackRangeEffect)DummyMap.DicEffect[AttackRangeEffect.Name].Copy();
            NewEffect.Value = "5";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(5, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.RangeModifier);
        }

        [TestMethod]
        public void TestSupportAttackEffect()
        {
            SupportAttackEffect NewEffect = (SupportAttackEffect)DummyMap.DicEffect[SupportAttackEffect.Name].Copy();
            NewEffect.Value = "5";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(5, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.SupportAttackModifierMax);
            Assert.AreEqual(5, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.SupportAttackModifier);
        }

        [TestMethod]
        public void TestSupportDefendEffect()
        {
            SupportDefendEffect NewEffect = (SupportDefendEffect)DummyMap.DicEffect[SupportDefendEffect.Name].Copy();
            NewEffect.Value = "5";

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(5, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.SupportDefendModifierMax);
            Assert.AreEqual(5, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.SupportDefendModifier);
        }

        [TestMethod]
        public void TestUnitTerrainEffect()
        {
            UnitTerrainEffect NewEffect = (UnitTerrainEffect)DummyMap.DicEffect[UnitTerrainEffect.Name].Copy();
            NewEffect.Value = "5";
            NewEffect.Terrain = "Land";
            NewEffect.NumberType = Operators.NumberTypes.Absolute;
            NewEffect.CanDowngrade = false;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            Assert.AreEqual(0, GlobalDeathmatchContext.EffectOwnerUnit.Boosts.DicTerrainLetterAttributeModifier["Land"]);
        }

        [TestMethod]
        public void TestUnitMovementTypeEffect()
        {
            UnitMovementTypeEffect NewEffect = (UnitMovementTypeEffect)DummyMap.DicEffect[UnitMovementTypeEffect.Name].Copy();
            NewEffect.UseAir = true;
            NewEffect.UseLand = true;
            NewEffect.UseSea = true;
            NewEffect.UseSpace = true;

            BaseAutomaticSkill DummySkill = CreateDummySkill(DummyMap.DicRequirement[PassiveRequirement.Name].Copy(),
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            Assert.IsFalse(GlobalDeathmatchContext.EffectOwnerUnit.ListTerrainChoices.Contains("Air"));
            Assert.IsFalse(GlobalDeathmatchContext.EffectOwnerUnit.ListTerrainChoices.Contains("Land"));
            Assert.IsFalse(GlobalDeathmatchContext.EffectOwnerUnit.ListTerrainChoices.Contains("Sea"));
            Assert.IsFalse(GlobalDeathmatchContext.EffectOwnerUnit.ListTerrainChoices.Contains("Space"));

            DummySkill.AddSkillEffectsToTarget(string.Empty);

            Assert.IsTrue(GlobalDeathmatchContext.EffectOwnerUnit.ListTerrainChoices.Contains("Air"));
            Assert.IsTrue(GlobalDeathmatchContext.EffectOwnerUnit.ListTerrainChoices.Contains("Land"));
            Assert.IsTrue(GlobalDeathmatchContext.EffectOwnerUnit.ListTerrainChoices.Contains("Sea"));
            Assert.IsTrue(GlobalDeathmatchContext.EffectOwnerUnit.ListTerrainChoices.Contains("Space"));
        }
    }
}
