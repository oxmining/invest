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
        public override bool SupportMobile => false;
        public override uint BoxIndex { get { return 100; } }
        public MiningWebBox() : base()
        {
        }
        public override void Init()
        {

        }
        public override MenuDataItem[] GetMemus(string language)
        {
            List<MenuDataItem> list = new List<MenuDataItem>();
            list.Add(new MenuDataItem
            {
                Path = "/_pc/invest",
                Name = UIHelper.WebLocalString(language, "交易", "Exchange"),
                Key = "exchange",
                //Icon = "smile",
                Children = new MenuDataItem[] {
                    new MenuDataItem
                    {
                        Path = "/_pc/invest/deposit",
                        Name = UIHelper.WebLocalString(language,"场外入金", "OTC Buy"),
                        Key = "deposit"
                    },
                     new MenuDataItem
                    {
                        Path = "/_pc/invest/otcsale",
                        Name =UIHelper.WebLocalString(language,"场外出金", "OTC Sale"),
                        Key = "otcsale"
                    },
                     new MenuDataItem
                    {
                        Path = "/_pc/invest/swap",
                        Name = UIHelper.WebLocalString(language,"兑换", "Swap"),
                        Key = "swap"
                    }
                }
            });
            if (OXRunTime.RunMode == RunMode.Server)
            {
                list.Add(new MenuDataItem
                {
                    Path = "/mining",
                    Name = UIHelper.WebLocalString(language, "挖矿", "Mining"),
                    Key = "mining",
                    //Icon = "smile",
                    Children = new MenuDataItem[] {
                         new MenuDataItem
                    {
                        Path = "/_pc/mining/myminer",
                        Name =UIHelper.WebLocalString(language,"矿机", "Miner"),
                        Key = "myminer"
                    },
                    new MenuDataItem
                    {
                        Path = "/_pc/mining/selflockmining",
                        Name =  UIHelper.WebLocalString(language,"自锁挖矿", "Self Lock Mining"),
                        Key = "selflockmining"
                    },
                    new MenuDataItem
                    {
                        Path = "/_pc/mining/mutuallockmining",
                        Name =  UIHelper.WebLocalString(language,"互锁挖矿", "Mutual Lock Mining"),
                        Key = "mutuallockmining"
                    },
                    new MenuDataItem
                    {
                        Path = "/_pc/mining/levellockmining",
                        Name =  UIHelper.WebLocalString(language,"级锁挖矿", "Level Lock Mining"),
                        Key = "levellockmining"
                    },
                    new MenuDataItem
                    {
                        Path = "/_pc/mining/checkinmining",
                        Name =  UIHelper.WebLocalString(language,"签到挖矿", "Checkin Mining"),
                        Key = "checkinmining"
                    },
                    new MenuDataItem
                    {
                        Path = "/_pc/mining/buybacktrusst",
                        Name =  UIHelper.WebLocalString(language,"回购信托池","Buy-Back Trust Pool"),
                        Key = "buybacktrusst"
                    }
                }
                });
            }
            return list.ToArray();
        }
        public override MenuDataItem[] GetMobileMemus(string language)
        {
            List<MenuDataItem> list = new List<MenuDataItem>();

            return list.ToArray();
        }
    }
}
