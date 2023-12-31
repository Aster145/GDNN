﻿using System;
using System.Windows.Forms;
using PKHeX.Core;

namespace PKHeXPluginExample
{
    public class 自定义插件 : IPlugin
    {
        public string Name => nameof(自定义插件); //Name是插件的名称
        public int Priority => 1; // Loading order, lowest is first.加载顺序，最低的在前。Priority定义了插件加载的优先级

        // Initialized on plugin load, 在插件加载时初始化
        public ISaveFileProvider SaveFileEditor { get; private set; } = null!;//PKHeX的存档编辑
        public IPKMView PKMEditor { get; private set; } = null!;//Pokémon编辑

        public void Initialize(params object[] args)//此方法在插件加载时调用，用于初始化插件并设置所需的组件和功能。
        {
            Console.WriteLine($"Loading {Name}...");
            SaveFileEditor = (ISaveFileProvider)Array.Find(args, z => z is ISaveFileProvider)!;
            PKMEditor = (IPKMView)Array.Find(args, z => z is IPKMView)!;
            var menu = (ToolStrip)Array.Find(args, z => z is ToolStrip)!;
            LoadMenuStrip(menu);
        }

        private void LoadMenuStrip(ToolStrip menuStrip)//这些方法用于向PKHeX的UI添加自定义的菜单项。
        {
            var items = menuStrip.Items;
            if (!(items.Find("Menu_Tools", false)[0] is ToolStripDropDownItem tools))
                throw new ArgumentException(nameof(menuStrip));
            AddPluginControl(tools);
        }

        private void AddPluginControl(ToolStripDropDownItem tools)//在这个示例中，它添加了一个新的菜单项并定义了三个子菜单项：
        {
            var ctrl = new ToolStripMenuItem(Name);
            tools.DropDownItems.Add(ctrl);

            //var c2 = new ToolStripMenuItem($"{Name} sub form");//按钮1,弹出一个新的窗口
            //c2.Click += (s, e) => new Form().ShowDialog();
            //var c3 = new ToolStripMenuItem($"{Name} show message");//按钮2,显示一个消息框,打印一个hello
            //c3.Click += (s, e) => MessageBox.Show("Hello!");

            var c4 = new ToolStripMenuItem($"全部设置为百变怪");//按钮3改成百变怪1
            c4.Click += (s, e) => ModifySaveFile();
            var c5 = new ToolStripMenuItem($"全部设置为50级");//按钮4,全部设置为50级1
            c5.Click += (s, e) => ModifyLV();
            var c6 = new ToolStripMenuItem($"删除全部精灵");//按钮5,删除全部精灵1
            c6.Click += (s, e) => Deleteallpkm();
            var c7 = new ToolStripMenuItem($"删除合法精灵");//按钮6,删除合法精灵1
            c7.Click += (s, e) => MessageBox.Show("不要乱点!!!!!!!!!!!!!!!!!!!\n这个功能还没有写好,呆逼!\n如果你知道这个功能怎么写请联系我\nQQ1451179481");
            //c7.Click += (s, e) => DeleteLegalpkm();

            //ctrl.DropDownItems.Add(c2);
            //ctrl.DropDownItems.Add(c3);
            ctrl.DropDownItems.Add(c4);//按钮3改成百变怪2
            Console.WriteLine($"{Name} added menu items.");

            ctrl.DropDownItems.Add(c5);//按钮4,全部设置为50级2
            Console.WriteLine($"{Name} added menu items.");

            ctrl.DropDownItems.Add(c6);//按钮5,删除全部精灵2
            Console.WriteLine($"{Name} added menu items.");

            ctrl.DropDownItems.Add(c7);

            //ctrl.DropDownItems.Add(c6);//按钮6,删除非法精灵2
            //Console.WriteLine($"{Name} added menu items.");
        }

        private void ModifySaveFile()//按钮3;全部设置为百变怪3
        {
            var sav = SaveFileEditor.SAV;
            sav.ModifyBoxes(ModifyPKM);
            SaveFileEditor.ReloadSlots();
        }
        private void ModifyLV()
        {
            var sav = SaveFileEditor.SAV;
            sav.ModifyBoxes(ModifyLVset);
            SaveFileEditor.ReloadSlots();
        }//按钮4,全部设置为50级3
        private void Deleteallpkm()
        {
            var sav = SaveFileEditor.SAV;
            sav.ModifyBoxes(delpkm);
            SaveFileEditor.ReloadSlots();
        }//按钮5,删除全部精灵3

        private void DeleteLegalpkm()
        {
            var sav = SaveFileEditor.SAV;
            sav.ModifyBoxes(delLegalpkm);
            SaveFileEditor.ReloadSlots();
        }//按钮6,删除非法精灵3
        public static void ModifyPKM(PKM pkm)
        {
            // Make everything Bulbasaur! 让一切变成妙蛙种子！
            pkm.Species = (int)Species.Ditto;//换成百变怪
            // pkm.Species = (int)Species.Bulbasaur;
            pkm.Move1 = 0; // pound技能1拍击
            pkm.Move2 = 0; // pound技能1拍击
            pkm.Move3 = 0; // pound技能1拍击
            pkm.Move4 = 0; // pound技能1拍击
            //pkm.Move1_PP = 40;
            pkm.Gender = 2;//无性别
            pkm.Language = 9;//简体中文
            //pkm.IsNicknamed = false;//重置昵称,无效
            pkm.SetDefaultNickname();//重置昵称
             pkm.PID = 0;//不闪
            // CommonEdits.SetShiny(pkm);变闪
            pkm.EV_ATK = 0;
            pkm.EV_DEF = 0;
            pkm.EV_HP = 0;
            pkm.EV_SPA = 0;
            pkm.EV_SPD = 0;
            pkm.EV_SPE = 0;
            pkm.IV_ATK = 0;
            pkm.IV_DEF = 0;
            pkm.IV_HP = 0;
            pkm.IV_SPA = 0;
            pkm.IV_SPD = 0;
            pkm.IV_SPE = 0;
            pkm.AbilityNumber = 0;//特性
            //pk.TeraType = 0;//钛晶
            pkm.Nature = 0;//性格
            pkm.RelearnMove1 = 0;
            pkm.RelearnMove2 = 0;
            pkm.RelearnMove3 = 0;
            pkm.RelearnMove4 = 0;
        }//按钮3;全部设置为百变怪4
        public static void ModifyLVset(PKM pkm)
        {
            pkm.CurrentLevel = 50;//全部设为50级
        }//按钮4,全部设置为50级4

        public static void delpkm(PKM pkm)
        {
            pkm.Species = (int)Species.None;//删除


        }//按钮5,删除全部精灵4

        public static void delLegalpkm(PKM pkm)
        {
            pkm.Species = (int)Species.None;//删除

        }//按钮6,删除非法精灵4

        public void NotifySaveLoaded()//当PKHeX加载一个新的存档文件时，NotifySaveLoaded方法会被调用。
        {
            Console.WriteLine($"{Name} was notified that a Save File was just loaded.");
        }

        public bool TryLoadFile(string filePath)//TryLoadFile方法在PKHeX尝试加载一个新文件时被调用，在这个示例中它没有执行任何操作。
        {
            Console.WriteLine($"{Name} was provided with the file path, but chose to do nothing with it.");
            return false; // no action taken,不采取行动
        }
    }
}
