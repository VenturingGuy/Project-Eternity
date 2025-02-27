﻿using System.IO;
using System.Collections.Generic;
using ProjectEternity.Core.Parts;
using ProjectEternity.Core.Skill;
using ProjectEternity.Core.Item;

namespace ProjectEternity.GameScreens.BattleMapScreen
{
    public class SystemList
    {
        public static Dictionary<string, UnitPart> ListPart = new Dictionary<string, UnitPart>();
        public static List<BaseAutomaticSkill> ListBuyableSkill = new List<BaseAutomaticSkill>();
        public static List<ManualSkill> ListSpirit = new List<ManualSkill>();
        public static List<BaseAutomaticSkill> ListSkill = new List<BaseAutomaticSkill>();
        public static List<BaseAutomaticSkill> ListAbility = new List<BaseAutomaticSkill>();

        public static void LoadSystemLists()
        {
            Dictionary<string, BaseSkillRequirement> DicRequirement = BaseSkillRequirement.LoadAllRequirements();
            Dictionary<string, BaseEffect> DicEffect = BaseEffect.LoadAllEffects();

            #region Parts

            if (File.Exists("Content/Parts List.txt"))
            {
                StreamReader SR = new StreamReader("Content/Parts List.txt");

                while (!SR.EndOfStream)
                {
                    string Line = SR.ReadLine();
                    string[] PartByType = Line.Split('/');
                    if (PartByType[0] == "Standard Parts")
                    {
                        ListPart.Add(Line, new UnitStandardPart("Content/Units/" + Line + ".pep", DicRequirement, DicEffect));
                    }
                    else if (PartByType[0] == "Consumable Parts")
                    {
                        ListPart.Add(Line, new UnitConsumablePart("Content/Units/" + Line + ".pep", DicRequirement, DicEffect));
                    }
                }
                SR.Close();
            }
            else
            {
                string[] Files = Directory.GetFiles("Content/Units/Standard Parts", "*.pep", SearchOption.AllDirectories);

                foreach (string File in Files)
                {
                    ListPart.Add(File, new UnitStandardPart(File, DicRequirement, DicEffect));
                }
                Files = Directory.GetFiles("Content/Units/Consumable Parts", "*.pep", SearchOption.AllDirectories);

                foreach (var File in Files)
                {
                    ListPart.Add(File, new UnitConsumablePart(File, DicRequirement, DicEffect));
                }
            }

            #endregion

            #region Buyable Skills

            if (File.Exists("Content/Buyable Skills List.txt"))
            {
                StreamReader SR = new StreamReader("Content/Buyable Skills List.txt");

                while (!SR.EndOfStream)
                {
                    string Line = SR.ReadLine();
                    ListBuyableSkill.Add(new BaseAutomaticSkill("Content/Characters/Skills/" + Line + ".pecs", DicRequirement, DicEffect));
                }
                SR.Close();
            }
            else
            {
                string[] Files = Directory.GetFiles("Content/Characters/Skills", "*.pecs", SearchOption.AllDirectories);

                foreach (var File in Files)
                {
                    ListBuyableSkill.Add(new BaseAutomaticSkill(File, DicRequirement, DicEffect));
                }
            }

            #endregion

            #region Spirits

            if (File.Exists("Content/Spirits List.txt"))
            {
                StreamReader SR = new StreamReader("Content/Spirits List.txt");

                while (!SR.EndOfStream)
                {
                    string Line = SR.ReadLine();
                    ListSpirit.Add(new ManualSkill("Content/Characters/Spirits/" + Line + ".pecs", DicRequirement, DicEffect));
                }
                SR.Close();
            }
            else
            {
                string[] Files = Directory.GetFiles("Content/Characters/Spirits", "*.pecs", SearchOption.AllDirectories);

                foreach (var File in Files)
                {
                    ListSpirit.Add(new ManualSkill(File, DicRequirement, DicEffect));
                }
            }

            #endregion

            #region Skills

            if (File.Exists("Content/Skills List.txt"))
            {
                StreamReader SR = new StreamReader("Content/Skills List.txt");

                while (!SR.EndOfStream)
                {
                    string Line = SR.ReadLine();
                    ListBuyableSkill.Add(new BaseAutomaticSkill("Content/Characters/Skills/" + Line + ".pecs", DicRequirement, DicEffect));
                }
                SR.Close();
            }
            else
            {
                string[] Files = Directory.GetFiles("Content/Characters/Skills", "*.pecs", SearchOption.AllDirectories);

                foreach (var File in Files)
                {
                    ListSkill.Add(new BaseAutomaticSkill(File, DicRequirement, DicEffect));
                }
            }

            #endregion

            #region Abilities

            if (File.Exists("Content/Abilities List.txt"))
            {
                StreamReader SR = new StreamReader("Content/Abilities List.txt");

                while (!SR.EndOfStream)
                {
                    string Line = SR.ReadLine();
                    ListAbility.Add(new BaseAutomaticSkill("Content/Units/Abilities/" + Line + ".pecs", DicRequirement, DicEffect));
                }
                SR.Close();
            }
            else
            {
                string[] Files = Directory.GetFiles("Content/Units/Abilities", "*.pecs", SearchOption.AllDirectories);

                foreach (var File in Files)
                {
                    ListAbility.Add(new BaseAutomaticSkill(File, DicRequirement, DicEffect));
                }
            }

            #endregion
        }
    }
}
