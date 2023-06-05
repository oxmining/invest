using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntDesign.ProLayout;
using OX.Wallets;

namespace OX.Web.Models
{
    public class MiningWebBox : WebBoxBlazor
    {

        public override uint BoxIndex { get { return 100; } }
        public MiningWebBox() : base()
        {
        }
        public override void Init()
        {

        }
        public override MenuDataItem[] GetMemus()
        {
            List<MenuDataItem> list = new List<MenuDataItem>();
            list.Add(new MenuDataItem
            {
                Path = "/invest",
                Name = UIHelper.LocalString("交易", "Exchange"),
                Key = "exchange",
                //Icon = "smile",
                Children = new MenuDataItem[] {
                    new MenuDataItem
                    {
                        Path = "/invest/deposit",
                        Name = UIHelper.LocalString("场外入金", "OTC Buy"),
                        Key = "deposit"
                    },
                     new MenuDataItem
                    {
                        Path = "/invest/otcsale",
                        Name = UIHelper.LocalString("场外出金", "OTC Sale"),
                        Key = "otcsale"
                    },
                     new MenuDataItem
                    {
                        Path = "/invest/swap",
                        Name = UIHelper.LocalString("兑换", "Swap"),
                        Key = "swap"
                    }
                }
            });
            if (OXRunTime.RunMode == RunMode.Server)
            {
                list.Add(new MenuDataItem
                {
                    Path = "/mining",
                    Name = UIHelper.LocalString("挖矿", "Mining"),
                    Key = "mining",
                    //Icon = "smile",
                    Children = new MenuDataItem[] {
                         new MenuDataItem
                    {
                        Path = "/mining/myminer",
                        Name = UIHelper.LocalString("矿机", "Miner"),
                        Key = "myminer"
                    },
                    new MenuDataItem
                    {
                        Path = "/mining/selflockmining",
                        Name =  UIHelper.LocalString("自锁挖矿", "Self Lock Mining"),
                        Key = "selflockmining"
                    },
                    new MenuDataItem
                    {
                        Path = "/mining/mutuallockmining",
                        Name =  UIHelper.LocalString("互锁挖矿", "Mutual Lock Mining"),
                        Key = "mutuallockmining"
                    },
                    new MenuDataItem
                    {
                        Path = "/mining/levellockmining",
                        Name =  UIHelper.LocalString("级锁挖矿", "Level Lock Mining"),
                        Key = "levellockmining"
                    }
                }
                });
            }
            return list.ToArray();
        }

    }
}
