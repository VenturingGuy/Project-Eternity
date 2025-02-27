﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ProjectEternity.Core;
using ProjectEternity.Core.Skill;
using ProjectEternity.Core.Units;
using ProjectEternity.GameScreens.BattleMapScreen;

namespace ProjectEternity.GameScreens.DeathmatchMapScreen
{
    public class ActionPanelMapSwitch : ActionPanelDeathmatch
    {
        private Squad ActiveSquad;
        private MapSwitchPoint ActiveSwitchPoint;

        public ActionPanelMapSwitch(DeathmatchMap Map, Squad ActiveSquad, MapSwitchPoint ActiveSwitchPoint)
            : base("Map Switch", Map, false)
        {
            this.ActiveSquad = ActiveSquad;
            this.ActiveSwitchPoint = ActiveSwitchPoint;
        }

        public override void OnSelect()
        {
        }

        public override void DoUpdate(GameTime gameTime)
        {
            ChangeSquadBetweenMaps(Map, ActiveSquad, ActiveSwitchPoint);
            RemoveAllSubActionPanels();
        }

        public static void ChangeSquadBetweenMaps(DeathmatchMap Map, Squad ActiveSquad, MapSwitchPoint ActiveSwitchPoint)
        {
            Map.ListPlayer[Map.ActivePlayerIndex].ListSquad.Remove(ActiveSquad);
            Map.ListPlayer[Map.ActivePlayerIndex].UpdateAliveStatus();
            DeathmatchMap SwitchMap = (DeathmatchMap)Map.ListSubMap.Find(x => x.BattleMapPath == ActiveSwitchPoint.SwitchMapPath);

            if (!SwitchMap.IsInit)
            {
                SwitchMap.Init();
            }

            for (int U = 0; U < ActiveSquad.UnitsInSquad; ++U)
            {
                ActiveSquad.At(U).ReinitializeMembers(Map.DicUnitType[ActiveSquad.At(U).UnitTypeName]);
            }

            ActiveSquad.ReloadSkills(SwitchMap.DicRequirement, SwitchMap.DicEffect, ManualSkillTarget.DicManualSkillTarget);
            SwitchMap.ListPlayer[Map.ActivePlayerIndex].ListSquad.Add(ActiveSquad);
            SwitchMap.ListPlayer[Map.ActivePlayerIndex].UpdateAliveStatus();
            ActiveSquad.SetPosition(new Vector3(ActiveSwitchPoint.OtherMapEntryPoint.X, ActiveSwitchPoint.OtherMapEntryPoint.Y, ActiveSquad.Z));

            Map.ListGameScreen.Remove(Map);
            Map.ListGameScreen.Insert(0, SwitchMap);
        }

        public static List<BattleMap> GetActiveSubMaps(DeathmatchMap Map)
        {
            List<BattleMap> ListActiveSubMaps = new List<BattleMap>();

            for (int i = 0; i < Map.ListSubMap.Count; i++)
            {
                DeathmatchMap ActiveMap = (DeathmatchMap)Map.ListSubMap[i];

                // Only update map with an active player on it
                if (ActiveMap.ListPlayer.Count > 0 && ActiveMap.ListPlayer[0].IsAlive)
                {
                    ListActiveSubMaps.Add(ActiveMap);
                }
            }

            return ListActiveSubMaps;
        }

        public override void Draw(CustomSpriteBatch g)
        {
        }
    }
}
