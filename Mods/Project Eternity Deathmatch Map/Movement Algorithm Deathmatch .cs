﻿using ProjectEternity.Core.Units;
using ProjectEternity.GameScreens.BattleMapScreen;

namespace ProjectEternity.GameScreens.DeathmatchMapScreen
{
    public class MovementAlgorithmDeathmatch : MovementAlgorithm
    {
        DeathmatchMap Map;

        public MovementAlgorithmDeathmatch(DeathmatchMap Map)
        {
            this.Map = Map;
        }

        public override float GetMVCost(UnitMapComponent MapComponent, UnitStats UnitStat, MovementAlgorithmTile CurrentNode, MovementAlgorithmTile TerrainToGo)
        {
            float MovementCostToNeighbor = 0;
            if (MapComponent.CurrentMovement == "Air")
            {
                if (Map.GetTerrainLetterAttribute(UnitStat, "Air") == 'C' || Map.GetTerrainLetterAttribute(UnitStat, "Air") == 'D' || Map.GetTerrainLetterAttribute(UnitStat, "Air") == '-')
                    MovementCostToNeighbor += 0.5f;
                else
                    MovementCostToNeighbor += 1;
            }
            else
            {
                char TerrainCharacter = Map.GetTerrainLetterAttribute(UnitStat, Map.GetTerrainType(TerrainToGo));

                if ((TerrainCharacter == 'C' || TerrainCharacter == 'D' || TerrainCharacter == '-') && Map.GetTerrainType(TerrainToGo) != "Land")
                    MovementCostToNeighbor += TerrainToGo.MVMoveCost + 0.5f;
                else if (TerrainCharacter == 'S' && TerrainToGo.MVMoveCost > 1)
                    MovementCostToNeighbor += TerrainToGo.MVMoveCost / 2;
                else
                    MovementCostToNeighbor += TerrainToGo.MVMoveCost;

                if (TerrainToGo.TerrainTypeIndex != GetTile(CurrentNode.Position.X, CurrentNode.Position.Y, MapComponent.LayerIndex).TerrainTypeIndex)
                    MovementCostToNeighbor += TerrainToGo.MVEnterCost;
            }

            return MovementCostToNeighbor;
        }

        public override MovementAlgorithmTile GetTile(float PosX, float PosY, int LayerIndex)
        {
            if (PosX < 0 || PosY < 0 || PosX >= Map.MapSize.X || PosY >= Map.MapSize.Y)
            {
                return null;
            }

            return Map.GetTerrain(PosX, PosY, LayerIndex);
        }
    }
}