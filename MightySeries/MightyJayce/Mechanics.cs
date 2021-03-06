﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Common.Data;
using SharpDX;

namespace MightyJayce
{
    class Mechanics : Extensions
    {
        public static void EventLoader() //Call OrbwalkerModes
        {
            Obj_AI_Hero.OnProcessSpellCast += Combo_Timer;
            Obj_AI_Hero.OnBuffRemove += CannonWtimer;
            
        }
        public static bool HammerMode { get { return HammerQ.Instance.Name == "JayceToTheSkies"; } }
        private static void CannonWtimer(Obj_AI_Base sender, Obj_AI_BaseBuffRemoveEventArgs args)
        {
            if (sender.IsMe && args.Buff.Name == "jaycehypercharge")
                cannonwtime = Utils.GameTimeTickCount;
        }

        public static int hammerqtime, hammerwtime, hammeretime, cannonqtime, cannonwtime, cannonetime, hammerrtime, cannonrtime;

        public static void Combo_Timer(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            //JayceToTheSkies           //Q
            //JayceBasicAttack          //A
            //JayceThunderingBlow       //E
            //JayceStaticField          //W
            //JayceStanceHtG            //HR
            //jayceshockblast           //QR
            //jaycehypercharge          //WR
            //jayceaccelerationgate     //ER
            //jaycestancegth            //RR
            //jaycepassiverangedattack //PAA
            //jaycepassivemeleeattack  //PAA

            if (!sender.IsMe) return;
            var spell = args.SData;

            switch (spell.Name)
            {
                case "JayceToTheSkies": //Hammer [Q]
                    hammerqtime = Utils.GameTimeTickCount;
                    break;
                case "JayceStaticField": //Hammer [W]
                    hammerwtime = Utils.GameTimeTickCount;
                    break;
                case "JayceThunderingBlow": //Hammer [E]
                    hammeretime = Utils.GameTimeTickCount;
                    break;
                case "jayceshockblast": //Cannon [Q]
                    cannonqtime = Utils.GameTimeTickCount;
                    break;
                case "jayceaccelerationgate": //Cannon [E]
                    cannonetime = Utils.GameTimeTickCount;
                    break;
                case "JayceStanceHtG": //Switch to Cannon
                    hammerrtime = Utils.GameTimeTickCount;
                    break;
                case "jaycestancegth": //Switch to Hammer
                    hammerrtime = Utils.GameTimeTickCount;
                    return;
            }


            if (sender.IsMe)
            {
                Printchat(args.SData.Name);
            }
        }

        #region -------------------- Ready Checks -------------------------------
        public static double Ready_Hammer_Q()
        {
            if (Utils.GameTimeTickCount - hammerqtime >= Hammer_Q_CD() * (1 + ObjectManager.Player.PercentCooldownMod))
                return 0;

            else return Hammer_Q_CD() * (1 + ObjectManager.Player.PercentCooldownMod) - (Utils.GameTimeTickCount - hammerqtime);
        }
        public static double Ready_Hammer_W()
        {
            if (Utils.GameTimeTickCount - hammerwtime >= 10000 * (1 + ObjectManager.Player.PercentCooldownMod))
                return 0;

            else return 10000 * (1 + ObjectManager.Player.PercentCooldownMod) - (Utils.GameTimeTickCount - hammerwtime);
        }
        public static double Ready_Hammer_E()
        {
            if (Utils.GameTimeTickCount - hammeretime >= Hammer_E_CD() * (1 + ObjectManager.Player.PercentCooldownMod))
                return 0;

            else return Hammer_E_CD() * (1 + ObjectManager.Player.PercentCooldownMod) - (Utils.GameTimeTickCount - hammeretime);
        }
        public static double Ready_R()
        {
            if (Utils.GameTimeTickCount - hammerrtime >= 6000 * (1 + ObjectManager.Player.PercentCooldownMod))
                return 0;

            else return 6000 * (1 + ObjectManager.Player.PercentCooldownMod) - (Utils.GameTimeTickCount - hammerrtime);
        }
        public static double Ready_Cannon_Q()
        {
            if (Utils.GameTimeTickCount - cannonqtime >= 8000 * (1 + ObjectManager.Player.PercentCooldownMod))
                return 0;

            else return 8000 * (1 + ObjectManager.Player.PercentCooldownMod) - (Utils.GameTimeTickCount - cannonqtime);
        }
        public static double Ready_Cannon_W()
        {
            if (Utils.GameTimeTickCount - cannonwtime >= Cannon_W_CD() * (1 + ObjectManager.Player.PercentCooldownMod))
                return 0;

            else return Cannon_W_CD() * (1 + ObjectManager.Player.PercentCooldownMod) - (Utils.GameTimeTickCount - cannonwtime);
        }
        public static double Ready_Cannon_E()
        {
            if (Utils.GameTimeTickCount - cannonetime >= 16000 * (1 + ObjectManager.Player.PercentCooldownMod))
                return 0;

            else return 16000 * (1 + ObjectManager.Player.PercentCooldownMod) - (Utils.GameTimeTickCount - cannonetime);
        }
        #endregion ------------------ End Ready Checks ----------------------------

        #region   ------------------- Get Cooldown times --------------------------
        public static double Hammer_Q_CD() //Actual Hammer Q cooldown - From The Skies!
        {
            return
                new double[] { 0, 16, 14, 12, 10, 8, 6 }[CannonQ.Level] * 1000;
        }
        public static double Hammer_E_CD() //Actual Hammer E cooldown - Thundering Blow
        {
            return
                new double[] { 0, 15, 14, 13, 12, 11, 10 }[HammerE.Level] * 1000;
        }
        public static double Cannon_W_CD() //Actual Cannon E Cooldown - Acceleration Gate
        {
            return
                new double[] { 0, 13, 11.4, 9.8, 8.2, 6.6, 5 }[CannonW.Level] * 1000;
        }

        #endregion ------------------ Get Cooldown Times --------------------------


    }
}
    

