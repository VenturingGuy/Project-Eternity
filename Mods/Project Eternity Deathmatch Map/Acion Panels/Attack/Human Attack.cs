﻿using Microsoft.Xna.Framework;
using ProjectEternity.Core;
using ProjectEternity.Core.Attacks;
using ProjectEternity.Core.ControlHelper;
using ProjectEternity.Core.Effects;
using ProjectEternity.Core.Units;
using static ProjectEternity.GameScreens.BattleMapScreen.BattleMap;
using static ProjectEternity.GameScreens.DeathmatchMapScreen.DeathmatchMap;

namespace ProjectEternity.GameScreens.DeathmatchMapScreen
{
    public class ActionPanelHumanAttack : ActionPanelDeathmatch
    {
        private Squad ActiveSquad;
        private SupportSquadHolder ActiveSquadSupport;
        private int ActivePlayerIndex;
        private Squad TargetSquad;
        private SupportSquadHolder TargetSquadSupport;
        private int TargetPlayerIndex;

        int ActiveSquadSupportIndexOld;//Index of the support squad used for reset.

        public ActionPanelHumanAttack(DeathmatchMap Map, Squad ActiveSquad, SupportSquadHolder ActiveSquadSupport, int ActivePlayerIndex,
            Squad TargetSquad, SupportSquadHolder TargetSquadSupport, int TargetPlayerIndex)
            : base("Human Attack", Map, false)
        {
            this.ActiveSquad = ActiveSquad;
            this.ActiveSquadSupport = ActiveSquadSupport;
            this.ActivePlayerIndex = ActivePlayerIndex;
            this.TargetSquad = TargetSquad;
            this.TargetSquadSupport = TargetSquadSupport;
            this.TargetPlayerIndex = TargetPlayerIndex;
        }

        public override void OnSelect()
        {
            //Attack Support
            ActiveSquadSupport.PrepareAttackSupport(Map, Map.ActivePlayerIndex, ActiveSquad, TargetSquad);
        }

        public override void DoUpdate(GameTime gameTime)
        {
            switch (Map.BattleMenuStage)
            {
                #region Default

                case BattleMenuStages.Default:
                    if (InputHelper.InputLeftPressed())
                    {
                        Map.BattleMenuCursorIndex--;

                        if (Map.BattleMenuCursorIndex == BattleMenuChoices.Support)
                        {//Can't pick Support. (No Support Squads nearby)
                            if (ActiveSquadSupport.Count <= 0)
                                Map.BattleMenuCursorIndex = BattleMenuChoices.Formation;
                        }
                        if (Map.BattleMenuCursorIndex == BattleMenuChoices.Formation)
                        {//Can't pick Formation. (No wingmans or ALL attack)
                            if (ActiveSquad.UnitsAliveInSquad == 1 || Map.BattleMenuDefenseFormationChoice == FormationChoices.ALL)
                                Map.BattleMenuCursorIndex = BattleMenuChoices.Action;
                        }
                        if (Map.BattleMenuCursorIndex < BattleMenuChoices.Start)
                            Map.BattleMenuCursorIndex = BattleMenuChoices.Demo;

                        Map.sndSelection.Play();
                    }
                    else if (InputHelper.InputRightPressed())
                    {
                        Map.BattleMenuCursorIndex++;

                        if (Map.BattleMenuCursorIndex == BattleMenuChoices.Formation)
                        {//Can't pick Formation. (No wingmans or ALL attack)
                            if (ActiveSquad.UnitsAliveInSquad == 1 || Map.BattleMenuDefenseFormationChoice == FormationChoices.ALL)
                                Map.BattleMenuCursorIndex = BattleMenuChoices.Support;
                        }
                        if (Map.BattleMenuCursorIndex == BattleMenuChoices.Support)
                        {//Can't pick Support. (No Support Squads nearby)
                            if (ActiveSquadSupport.Count <= 0)
                                Map.BattleMenuCursorIndex = BattleMenuChoices.Demo;
                        }
                        if (Map.BattleMenuCursorIndex > BattleMenuChoices.Demo)
                            Map.BattleMenuCursorIndex = BattleMenuChoices.Start;

                        Map.sndSelection.Play();
                    }
                    else if (MouseHelper.MouseMoved())
                    {
                        if (MouseHelper.MouseStateCurrent.Y >= Constants.Height - 30)
                        {
                            if (MouseHelper.MouseStateCurrent.X <= 125)
                            {
                                Map.BattleMenuCursorIndex = BattleMenuChoices.Start;
                            }
                            else if (MouseHelper.MouseStateCurrent.X <= 255)
                            {
                                Map.BattleMenuCursorIndex = BattleMenuChoices.Action;
                            }
                            else if (MouseHelper.MouseStateCurrent.X <= 385
                                && (ActiveSquad.UnitsAliveInSquad > 1 && Map.BattleMenuDefenseFormationChoice != FormationChoices.ALL))
                            {
                                Map.BattleMenuCursorIndex = BattleMenuChoices.Formation;
                            }
                            else if (MouseHelper.MouseStateCurrent.X <= 510 && ActiveSquadSupport.Count > 0)
                            {
                                Map.BattleMenuCursorIndex = BattleMenuChoices.Support;
                            }
                            else if (MouseHelper.MouseStateCurrent.X > 510 && MouseHelper.MouseStateCurrent.X <= 635)
                            {
                                Map.BattleMenuCursorIndex = BattleMenuChoices.Demo;
                            }
                        }
                    }
                    else if (InputHelper.InputCommand1Pressed())
                    {
                        Map.SpiritMenu.InitSpiritScreen(ActiveSquad);

                        Map.BattleMenuStage = BattleMenuStages.Default;
                    }
                    else if (InputHelper.InputCommand2Pressed())
                    {
                        //Constants.ShowAnimation = !Constants.ShowAnimation;

                        Map.sndSelection.Play();
                    }
                    if (InputHelper.InputConfirmPressed())
                    {
                        switch (Map.BattleMenuCursorIndex)
                        {
                            //Begin attack.
                            case BattleMenuChoices.Start:
                                if (Map.ListPlayer[TargetPlayerIndex].IsHuman)
                                    Map.InitPlayerDefence(ActiveSquad, ActiveSquadSupport, ActivePlayerIndex, TargetSquad, TargetSquadSupport, TargetPlayerIndex);
                                else
                                {
                                    Map.InitPlayerAttack(ActiveSquad, ActiveSquadSupport, ActivePlayerIndex, TargetSquad, TargetSquadSupport, TargetPlayerIndex);
                                    ActiveSquad.CurrentLeader.UpdateSkillsLifetime(SkillEffect.LifetimeTypeOnAction);
                                }
                                break;

                            case BattleMenuChoices.Action:
                                if (ActiveSquad.UnitsAliveInSquad == 1)
                                {
                                    ActiveSquad.CurrentLeader.UpdateNonMAPAttacks(ActiveSquad.Position, TargetSquad.Position, TargetSquad.CurrentMovement, ActiveSquad.CanMove);

                                    ActiveSquad.CurrentLeader.AttackIndex = 0;//Make sure you select the first weapon.
                                    Map.BattleMenuStage = BattleMenuStages.ChooseAttack;
                                }
                                else
                                {
                                    Map.BattleMenuStage = BattleMenuStages.ChooseSquadMember;//Choose squad member.
                                    Map.BattleMenuCursorIndexSecond = 1;//Leader is attacking, can't put him on defend or evade.
                                }
                                break;

                            case BattleMenuChoices.Formation:
                                Map.BattleMenuCursorIndexSecond = (int)Map.BattleMenuOffenseFormationChoice;
                                Map.BattleMenuStage = BattleMenuStages.ChooseFormation;
                                break;

                            case BattleMenuChoices.Support:
                                Map.BattleMenuCursorIndexSecond = 0;
                                ActiveSquadSupportIndexOld = ActiveSquadSupport.ActiveSquadSupportIndex;
                                Map.BattleMenuStage = BattleMenuStages.ChooseSupport;
                                break;

                            case BattleMenuChoices.Demo:
                                Constants.ShowAnimation = !Constants.ShowAnimation;
                                break;
                        }

                        Map.sndConfirm.Play();
                    }
                    else if (InputHelper.InputCancelPressed())
                    {
                        CancelPanel();
                    }
                    break;

                #endregion

                #region Choose attack

                case BattleMenuStages.ChooseAttack://Attack selection.
                    if (InputHelper.InputUpPressed())
                    {
                        --ActiveSquad.CurrentLeader.AttackIndex;
                        if (ActiveSquad.CurrentLeader.AttackIndex < 0)
                            ActiveSquad.CurrentLeader.AttackIndex = ActiveSquad.CurrentLeader.ListAttack.Count - 1;

                        Map.sndSelection.Play();
                    }
                    else if (InputHelper.InputDownPressed())
                    {
                        ++ActiveSquad.CurrentLeader.AttackIndex;
                        if (ActiveSquad.CurrentLeader.AttackIndex >= ActiveSquad.CurrentLeader.ListAttack.Count)
                            ActiveSquad.CurrentLeader.AttackIndex = 0;

                        Map.sndSelection.Play();
                    }
                    else if (MouseHelper.MouseMoved())
                    {
                        int YStep = 25;
                        int YStart = 122;

                        if (MouseHelper.MouseStateCurrent.Y >= YStart)
                        {
                            int NewValue = (MouseHelper.MouseStateCurrent.Y - YStart) / YStep;
                            if (NewValue >= 0 && NewValue < ActiveSquad.CurrentLeader.ListAttack.Count)
                            {
                                ActiveSquad.CurrentLeader.AttackIndex = NewValue;
                            }
                        }
                    }
                    //Exit the weapon panel.
                    else if (InputHelper.InputConfirmPressed())
                    {
                        if (ActiveSquad.CurrentLeader.CurrentAttack.CanAttack)
                        {
                            if (ActiveSquad.CurrentLeader.CurrentAttack.Pri == WeaponPrimaryProperty.ALL)
                                Map.BattleMenuOffenseFormationChoice = FormationChoices.ALL;

                            Map.BattleMenuStage = BattleMenuStages.Default;
                            ActiveSquad.CurrentLeader.AttackAccuracy = Map.CalculateHitRate(ActiveSquad.CurrentLeader, ActiveSquad,
                                TargetSquad.CurrentLeader, TargetSquad, TargetSquad.CurrentLeader.BattleDefenseChoice).ToString() + "%";
                            if (!Map.ListPlayer[TargetPlayerIndex].IsHuman)
                            {
                                PrepareDefenseSquadForBattle(Map, ActiveSquad, TargetSquad);
                            }

                            Map.sndConfirm.Play();
                        }
                        else
                        {
                            Map.sndDeny.Play();
                        }
                    }
                    else if (InputHelper.InputCancelPressed())
                    {
                        Map.BattleMenuStage = BattleMenuStages.Default;
                        Map.sndCancel.Play();
                    }
                    break;

                #endregion

                #region Choose formation

                case BattleMenuStages.ChooseFormation://Formation choice.

                    if (InputHelper.InputUpPressed())
                    {
                        Map.BattleMenuCursorIndexSecond -= Map.BattleMenuCursorIndexSecond > 0 ? 1 : 0;
                        Map.sndSelection.Play();
                    }
                    else if (InputHelper.InputDownPressed())
                    {
                        Map.BattleMenuCursorIndexSecond += Map.BattleMenuCursorIndexSecond < 1 ? 1 : 0;
                        Map.sndSelection.Play();
                    }
                    if (InputHelper.InputConfirmPressed())
                    {
                        if (Map.BattleMenuCursorIndexSecond == 0)
                            Map.BattleMenuOffenseFormationChoice = FormationChoices.Focused;
                        else if (Map.BattleMenuCursorIndexSecond == 1)
                            Map.BattleMenuOffenseFormationChoice = FormationChoices.Spread;

                        Map.UpdateWingmansSelection(ActiveSquad, TargetSquad, Map.BattleMenuOffenseFormationChoice);

                        if (ActiveSquad.CurrentWingmanA != null)
                        {
                            if (ActiveSquad.CurrentWingmanA.CanAttack)
                                ActiveSquad.CurrentWingmanA.BattleDefenseChoice = Unit.BattleDefenseChoices.Attack;
                            else
                                ActiveSquad.CurrentWingmanA.BattleDefenseChoice = Unit.BattleDefenseChoices.Defend;
                        }
                        if (ActiveSquad.CurrentWingmanB != null)
                        {
                            if (ActiveSquad.CurrentWingmanB.CanAttack)
                                ActiveSquad.CurrentWingmanB.BattleDefenseChoice = Unit.BattleDefenseChoices.Attack;
                            else
                                ActiveSquad.CurrentWingmanB.BattleDefenseChoice = Unit.BattleDefenseChoices.Defend;
                        }

                        //Simulate defense reaction.
                        PrepareDefenseSquadForBattle(Map, ActiveSquad, TargetSquad);
                        PrepareAttackSquadForBattle(Map, ActiveSquad, TargetSquad);

                        Map.BattleMenuStage = BattleMenuStages.Default;
                        Map.sndConfirm.Play();
                    }
                    else if (InputHelper.InputCancelPressed())
                    {
                        Map.BattleMenuStage = BattleMenuStages.Default;
                        Map.sndCancel.Play();
                    }
                    break;

                #endregion

                #region Choose squad member

                case BattleMenuStages.ChooseSquadMember:

                    if (InputHelper.InputDownPressed())
                    {
                        Map.BattleMenuCursorIndexSecond += Map.BattleMenuCursorIndexSecond < ActiveSquad.UnitsAliveInSquad - 1 ? 1 : 0;
                        Map.sndSelection.Play();
                    }
                    else if (InputHelper.InputUpPressed())
                    {
                        Map.BattleMenuCursorIndexSecond -= Map.BattleMenuCursorIndexSecond > 1 ? 1 : 0;
                        Map.sndSelection.Play();
                    }
                    if (InputHelper.InputConfirmPressed())
                    {
                        if (Map.BattleMenuCursorIndexSecond == 1)
                            Map.BattleMenuCursorIndexThird = (int)ActiveSquad.CurrentWingmanA.BattleDefenseChoice;
                        else if (Map.BattleMenuCursorIndexSecond == 2)
                            Map.BattleMenuCursorIndexThird = (int)ActiveSquad.CurrentWingmanB.BattleDefenseChoice;

                        Map.BattleMenuStage = BattleMenuStages.ChooseSquadMemberDefense;
                        Map.sndConfirm.Play();
                    }
                    else if (InputHelper.InputCancelPressed())
                    {
                        Map.BattleMenuStage = BattleMenuStages.Default;
                        Map.sndCancel.Play();
                    }
                    break;

                #endregion

                #region Choose squad member defense

                case BattleMenuStages.ChooseSquadMemberDefense:

                    if (InputHelper.InputUpPressed())
                    {
                        Map.BattleMenuCursorIndexThird++;
                        if (Map.BattleMenuCursorIndexThird >= 3)
                        {
                            if (Map.BattleMenuCursorIndexSecond > 0 && Map.BattleMenuCursorIndexThird >= 3 &&
                                (!ActiveSquad[Map.BattleMenuCursorIndexSecond].CanAttack || ActiveSquad.CurrentLeader.BattleDefenseChoice != Unit.BattleDefenseChoices.Attack || Map.BattleMenuOffenseFormationChoice == FormationChoices.ALL))
                                Map.BattleMenuCursorIndexThird = 1;
                            else
                                Map.BattleMenuCursorIndexThird = 0;
                        }

                        Map.sndSelection.Play();
                    }
                    else if (InputHelper.InputDownPressed())
                    {
                        Map.BattleMenuCursorIndexThird--;
                        if (Map.BattleMenuCursorIndexSecond > 0 && Map.BattleMenuCursorIndexThird <= 0 &&
                            (!ActiveSquad[Map.BattleMenuCursorIndexSecond].CanAttack || ActiveSquad.CurrentLeader.BattleDefenseChoice != Unit.BattleDefenseChoices.Attack || Map.BattleMenuOffenseFormationChoice == FormationChoices.ALL))
                            Map.BattleMenuCursorIndexThird = 2;
                        else if (Map.BattleMenuCursorIndexThird < 0)
                            Map.BattleMenuCursorIndexThird = 2;

                        Map.sndSelection.Play();
                    }
                    if (InputHelper.InputConfirmPressed())
                    {
                        ActiveSquad[Map.BattleMenuCursorIndexSecond].BattleDefenseChoice = (Unit.BattleDefenseChoices)Map.BattleMenuCursorIndexThird;

                        if (ActiveSquad[Map.BattleMenuCursorIndexSecond].BattleDefenseChoice == Unit.BattleDefenseChoices.Attack)
                        {
                            //Simulate defense reaction.
                            PrepareDefenseSquadForBattle(Map, ActiveSquad, TargetSquad);

                            if (Map.BattleMenuOffenseFormationChoice == FormationChoices.Spread)
                            {
                                if (Map.BattleMenuCursorIndexSecond == 1)
                                {
                                    ActiveSquad.CurrentWingmanA.AttackIndex = ActiveSquad.CurrentWingmanA.PLAAttack;
                                    ActiveSquad.CurrentWingmanA.AttackAccuracy = Map.CalculateHitRate(ActiveSquad.CurrentWingmanA, ActiveSquad,
                                        TargetSquad.CurrentWingmanA, TargetSquad, TargetSquad.CurrentWingmanA.BattleDefenseChoice).ToString() + "%";
                                }
                                else if (Map.BattleMenuCursorIndexSecond == 2)
                                {
                                    ActiveSquad.CurrentWingmanB.AttackIndex = ActiveSquad.CurrentWingmanB.PLAAttack;
                                    ActiveSquad.CurrentWingmanB.AttackAccuracy = Map.CalculateHitRate(ActiveSquad.CurrentWingmanB, ActiveSquad,
                                        TargetSquad.CurrentWingmanB, TargetSquad, TargetSquad.CurrentWingmanB.BattleDefenseChoice).ToString() + "%";
                                }
                            }
                            else if (Map.BattleMenuOffenseFormationChoice == FormationChoices.Focused)
                            {
                                if (Map.BattleMenuCursorIndexSecond == 1)
                                {
                                    ActiveSquad.CurrentWingmanA.AttackIndex = ActiveSquad.CurrentWingmanA.PLAAttack;
                                    ActiveSquad.CurrentWingmanA.AttackAccuracy = Map.CalculateHitRate(ActiveSquad.CurrentWingmanA, ActiveSquad,
                                        TargetSquad.CurrentLeader, TargetSquad, TargetSquad.CurrentLeader.BattleDefenseChoice).ToString() + "%";
                                }
                                else if (Map.BattleMenuCursorIndexSecond == 2)
                                {
                                    ActiveSquad.CurrentWingmanB.AttackIndex = ActiveSquad.CurrentWingmanB.PLAAttack;
                                    ActiveSquad.CurrentWingmanB.AttackAccuracy = Map.CalculateHitRate(ActiveSquad.CurrentWingmanB, ActiveSquad,
                                        TargetSquad.CurrentLeader, TargetSquad, TargetSquad.CurrentLeader.BattleDefenseChoice).ToString() + "%";
                                }
                            }
                        }

                        Map.BattleMenuStage = BattleMenuStages.Default;
                        Map.sndConfirm.Play();
                    }
                    else if (InputHelper.InputCancelPressed())
                    {
                        Map.BattleMenuStage = BattleMenuStages.ChooseSquadMember;
                        Map.sndCancel.Play();
                    }
                    break;

                #endregion

                #region Choose support

                case BattleMenuStages.ChooseSupport:
                    if (InputHelper.InputUpPressed())
                    {
                        --ActiveSquadSupport.ActiveSquadSupportIndex;
                        if (ActiveSquadSupport.ActiveSquadSupportIndex < -1)
                            ActiveSquadSupport.ActiveSquadSupportIndex = ActiveSquadSupport.Count - 1;

                        Map.sndSelection.Play();
                    }
                    else if (InputHelper.InputDownPressed())
                    {
                        ++ActiveSquadSupport.ActiveSquadSupportIndex;
                        if (ActiveSquadSupport.ActiveSquadSupportIndex >= ActiveSquadSupport.Count)
                            ActiveSquadSupport.ActiveSquadSupportIndex = -1;

                        Map.sndSelection.Play();
                    }
                    else if (InputHelper.InputConfirmPressed())
                    {
                        if (ActiveSquadSupport.ActiveSquadSupportIndex >= 0)
                        {
                            Map.BattleMenuStage = BattleMenuStages.ChooseSupportAttack;
                            //Update weapons so you know which one is in attack range.

                            ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.DisableAllAttacks();
                            ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.UpdateAllAttacks(
                                ActiveSquadSupport.ActiveSquadSupport.Position,
                                TargetSquad.Position, TargetSquad.CurrentMovement,
                                ActiveSquadSupport.ActiveSquadSupport.CanMove);

                            Map.WeaponIndexOld = ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.AttackIndex;
                            ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.AttackIndex = 0;//Make sure you select the first weapon.
                        }
                        else
                        {
                            Map.BattleMenuStage = BattleMenuStages.Default;
                        }
                        Map.sndConfirm.Play();
                    }
                    else if (InputHelper.InputCancelPressed())
                    {
                        Map.BattleMenuStage = BattleMenuStages.Default;
                        Map.sndCancel.Play();
                    }
                    break;

                #endregion

                #region Choose support attack

                case BattleMenuStages.ChooseSupportAttack:
                    if (InputHelper.InputUpPressed())
                    {
                        --ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.AttackIndex;
                        if (ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.AttackIndex < 0)
                            ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.AttackIndex = ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.ListAttack.Count - 1;

                        Map.sndSelection.Play();
                    }
                    else if (InputHelper.InputDownPressed())
                    {
                        ++ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.AttackIndex;
                        if (ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.AttackIndex >= ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.ListAttack.Count)
                            ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.AttackIndex = 0;

                        Map.sndSelection.Play();
                    }
                    else if (InputHelper.InputConfirmPressed())
                    {
                        if (ActiveSquadSupport.ActiveSquadSupport.CurrentLeader.CurrentAttack.CanAttack)
                        {
                            Map.BattleMenuStage = BattleMenuStages.Default;
                            Map.sndConfirm.Play();
                        }
                        else
                            Map.sndDeny.Play();
                    }
                    else if (InputHelper.InputCancelPressed())
                    {
                        ActiveSquadSupport.ActiveSquadSupportIndex = ActiveSquadSupportIndexOld;
                        Map.BattleMenuStage = BattleMenuStages.ChooseSupport;
                        Map.sndCancel.Play();
                    }
                    break;

                    #endregion
            }
        }

        public override void Draw(CustomSpriteBatch g)
        {
            Map.BattleSumaryAttackDraw(g, TargetSquad, TargetSquadSupport, ActiveSquad, ActiveSquadSupport);
        }
    }
}
