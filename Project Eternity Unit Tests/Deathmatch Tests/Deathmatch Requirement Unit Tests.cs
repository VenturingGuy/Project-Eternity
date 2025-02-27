﻿using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectEternity.Core.Item;
using ProjectEternity.Core.Units;
using ProjectEternity.GameScreens.BattleMapScreen;
using ProjectEternity.GameScreens.DeathmatchMapScreen;

namespace ProjectEternity.UnitTests
{
    public partial class DeathmatchTests
    {
        [TestMethod]
        public void BattleStartRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BattleStartRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BattleStartRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BattleStartRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.BattleStartRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeAttackRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeAttackRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeAttackRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeAttackRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.BeforeAttackRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeGettingAttackedRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeGettingAttackedRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeGettingAttackedRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeGettingAttackedRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.BeforeGettingAttackedRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeHitRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeHitRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeHitRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeHitRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.BeforeHitRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeGettingHitRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeGettingHitRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeGettingHitRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeGettingHitRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.BeforeGettingHitRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeMissRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeMissRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeMissRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeMissRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.BeforeMissRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeGettingMissedRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeGettingMissedRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeGettingMissedRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeGettingMissedRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.BeforeGettingMissedRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeGettingDestroyedRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeGettingDestroyedRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void BeforeGettingDestroyedRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.BeforeGettingDestroyedRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.BeforeGettingDestroyedRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterAttackRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterAttackRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterAttackRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterAttackRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.AfterAttackRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterGettingAttackedRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterGettingAttackedRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterGettingAttackedRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterGettingAttackedRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.AfterGettingAttackedRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterHitRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterHitRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterHitRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterHitRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.AfterHitRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterGettingHitRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterGettingHitRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterGettingHitRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterGettingHitRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.AfterGettingHitRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterMissRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterMissRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterMissRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterMissRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.AfterMissRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterGettingMissedRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterGettingMissedRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterGettingMissedRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterGettingMissedRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.AfterGettingMissedRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterGettingDestroyRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterGettingDestroyRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AfterGettingDestroyRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.AfterGettingDestroyRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.AfterGettingDestroyRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void VSPilotRequirementFail()
        {
            VSPilotRequirement NewRequirement = (VSPilotRequirement)DummyMap.DicRequirement[DeathmatchSkillRequirement.VSPilotRequirementName].Copy();
            NewRequirement.PilotName = "";

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            Squad DummyTargetSquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummyTargetSquad, DummyTargetSquad.CurrentLeader, DummyTargetSquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void VSPilotRequirementSuccess()
        {
            VSPilotRequirement NewRequirement = (VSPilotRequirement)DummyMap.DicRequirement[DeathmatchSkillRequirement.VSPilotRequirementName].Copy();
            NewRequirement.PilotName = "Dummy Pilot";

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            Squad DummyTargetSquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummyTargetSquad, DummyTargetSquad.CurrentLeader, DummyTargetSquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void VSUnitRequirementFail()
        {
            VSUnitRequirement NewRequirement = (VSUnitRequirement)DummyMap.DicRequirement[DeathmatchSkillRequirement.VSUnitRequirementName].Copy();
            NewRequirement.UnitName = "";

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            Squad DummyTargetSquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummyTargetSquad, DummyTargetSquad.CurrentLeader, DummyTargetSquad.CurrentLeader.Pilot);
            
            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void VSUnitRequirementSuccess()
        {
            VSUnitRequirement NewRequirement = (VSUnitRequirement)DummyMap.DicRequirement[DeathmatchSkillRequirement.VSUnitRequirementName].Copy();
            NewRequirement.UnitName = "Dummy Unit";

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            Squad DummyTargetSquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummyTargetSquad, DummyTargetSquad.CurrentLeader, DummyTargetSquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AttackUsedRequirementFail()
        {
            AttackUsedRequirement NewRequirement = (AttackUsedRequirement)DummyMap.DicRequirement[DeathmatchSkillRequirement.AttackUsedRequirementName].Copy();
            NewRequirement.AttackName = "";

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            Squad DummyTargetSquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummyTargetSquad, DummyTargetSquad.CurrentLeader, DummyTargetSquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AttackUsedRequirementSuccess()
        {
            AttackUsedRequirement NewRequirement = (AttackUsedRequirement)DummyMap.DicRequirement[DeathmatchSkillRequirement.AttackUsedRequirementName].Copy();
            NewRequirement.AttackName = "Dummy Attack";

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            Squad DummyTargetSquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummyTargetSquad, DummyTargetSquad.CurrentLeader, DummyTargetSquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AttackDefendedRequirementFail()
        {
            AttackDefendedRequirement NewRequirement = (AttackDefendedRequirement)DummyMap.DicRequirement[DeathmatchSkillRequirement.AttackDefendedRequirementName].Copy();
            NewRequirement.AttackName = "";

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            Squad DummyTargetSquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummyTargetSquad, DummyTargetSquad.CurrentLeader, DummyTargetSquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void AttackDefendedRequirementSuccess()
        {
            AttackDefendedRequirement NewRequirement = (AttackDefendedRequirement)DummyMap.DicRequirement[DeathmatchSkillRequirement.AttackDefendedRequirementName].Copy();
            NewRequirement.AttackName = "Dummy Attack";

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            Squad DummyTargetSquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummyTargetSquad, DummyTargetSquad.CurrentLeader, DummyTargetSquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void SupportAttackRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.SupportAttackRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);


            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void SupportAttackRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.SupportAttackRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.SupportAttackRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }

        [TestMethod]
        public void SupportDefendRequirementFail()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.SupportDefendRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(string.Empty);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(0, ListActiveEffect.Count);
        }

        [TestMethod]
        public void SupportDefendRequirementSuccess()
        {
            BaseSkillRequirement NewRequirement = DummyMap.DicRequirement[DeathmatchSkillRequirement.SupportDefendRequirementName].Copy();

            FinalDamageEffect NewEffect = (FinalDamageEffect)DummyMap.DicEffect[FinalDamageEffect.Name].Copy();
            NewEffect.FinalDamageValue = "1000";

            BaseAutomaticSkill DummySkill = CreateDummySkill(NewRequirement,
                                                            AutomaticSkillTargetType.DicTargetType[EffectActivationSelf.Name].Copy(),
                                                            NewEffect);

            Squad DummySquad = CreateDummySquad();
            GlobalDeathmatchContext.SetContext(DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot, DummySquad, DummySquad.CurrentLeader, DummySquad.CurrentLeader.Pilot);

            DummySkill.AddSkillEffectsToTarget(DeathmatchSkillRequirement.SupportDefendRequirementName);
            List<BaseEffect> ListActiveEffect = GlobalDeathmatchContext.EffectOwnerUnit.Pilot.Effects.GetActiveEffects("Dummy");
            Assert.AreEqual(1, ListActiveEffect.Count);
        }
    }
}
