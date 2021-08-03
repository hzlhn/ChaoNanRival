using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JiebaNet.Segmenter;
using Microsoft.International.Converters.PinYinConverter;
using System.Text.RegularExpressions;
using JiebaNet.Segmenter.PosSeg;
using System.IO;
using Microsoft.Win32;
using System.Threading;

namespace ChaoNanTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        List<string> NList = new List<string>();
        List<string> NRList = new List<string>();
        List<string> NSList = new List<string>();
        List<string> NTList = new List<string>();
        List<string> TList = new List<string>();
        List<string> SList = new List<string>();
        List<string> FList = new List<string>();
        List<string> VList = new List<string>();
        List<string> VSList = new List<string>();
        List<string> VTList = new List<string>();
        List<string> ADJList = new List<string>();
        List<string> BList = new List<string>();
        List<string> ZList = new List<string>();
        List<string> RList = new List<string>();
        List<string> MList = new List<string>();
        List<string> QList = new List<string>();
        List<string> DList = new List<string>();
        List<string> PList = new List<string>();
        List<string> CList = new List<string>();
        List<string> UList = new List<string>();
        List<string> HOKALost = new List<string>();

        short status = 0;
        string UTAUtext = "";
        string[] UTAUsplitText = { "" };
        string DicPath = System.IO.Path.Combine("Resources", "dict.txt");

        private void ClearWordLists()
        {
            NList.Clear();
            NRList.Clear();
            NSList.Clear();
            NTList.Clear();
            TList.Clear();
            SList.Clear();
            FList.Clear();
            VList.Clear();
            VSList.Clear();
            VTList.Clear();
            ADJList.Clear();
            BList.Clear();
            ZList.Clear();
            RList.Clear();
            MList.Clear();
            QList.Clear();
            DList.Clear();
            PList.Clear();
            CList.Clear();
            UList.Clear();
            HOKALost.Clear();
        }

        private void AutoSerielizeWords(string chineseWords)
        {
            ClearWordLists();
            var posSeg = new PosSegmenter();
            var tokens = posSeg.Cut(chineseWords);

            foreach (var item in tokens)
            {
                switch (item.Flag)
                {
                    case "n":
                        NList.Add(item.Word);
                        break;
                    case "nr":
                    case "nr1":
                    case "nr2":
                    case "nrj":
                    case "nrf":
                        NRList.Add(item.Word);
                        break;
                    case "ns":
                    case "nsf":
                        NSList.Add(item.Word);
                        break;
                    case "nt":
                    case "nz":
                    case "nl":
                    case "ng":
                        NTList.Add(item.Word);
                        break;
                    case "t":
                    case "tg":
                        TList.Add(item.Word);
                        break;
                    case "s":
                        SList.Add(item.Word);
                        break;
                    case "f":
                        FList.Add(item.Word);
                        break;
                    case "v":
                    case "vd":
                    case "vn":
                        VList.Add(item.Word);
                        break;
                    case "vshi":
                    case "vyou":
                        VSList.Add(item.Word);
                        break;
                    case "vf":
                    case "vx":
                    case "vi":
                    case "vl":
                    case "vg":
                        VTList.Add(item.Word);
                        break;
                    case "a":
                    case "ad":
                    case "an":
                    case "ag":
                    case "al":
                        ADJList.Add(item.Word);
                        break;
                    case "b":
                    case "bl":
                        BList.Add(item.Word);
                        break;
                    case "z":
                        ZList.Add(item.Word);
                        break;
                    case "r":
                    case "rr":
                    case "rz":
                    case "rzt":
                    case "rzs":
                    case "rzv":
                    case "ry":
                    case "ryt":
                    case "rys":
                    case "ryv":
                    case "rg":
                        RList.Add(item.Word);
                        break;
                    case "m":
                        MList.Add(item.Word);
                        break;
                    case "q":
                    case "qv":
                    case "qt":
                        QList.Add(item.Word);
                        break;
                    case "d":
                        DList.Add(item.Word);
                        break;
                    case "p":
                    case "pba":
                    case "pbei":
                        PList.Add(item.Word);
                        break;
                    case "c":
                    case "cc":
                        CList.Add(item.Word);
                        break;
                    case "u":
                    case "uzhe":
                    case "ule":
                    case "uguo":
                    case "ude1":
                    case "ude2":
                    case "ude3":
                    case "usuo":
                    case "udeng":
                    case "uyy":
                    case "udh":
                    case "uls":
                    case "uzhi":
                    case "ulian":
                        UList.Add(item.Word);
                        break;
                    default:
                        HOKALost.Add(item.Word);
                        break;
                }
            }
        }

        private string DeleteNonChineseChar(string inputString)
        {
            char[] result = inputString.ToCharArray();
            string onlychinesestr = "";

            for (int i = 0; i < inputString.Length; i++)
            {
                if (result[i] >= 0x4e00 && result[i] <= 0x9fbb)
                {
                    onlychinesestr = onlychinesestr.Insert(onlychinesestr.Length, result[i].ToString());
                }
            }
            return onlychinesestr;
        }

        private void YaYun_MakeRythmButton_Click(object sender, RoutedEventArgs e)
        {
            string str = YaYun_TypeInTextBox.Text;
            if (string.IsNullOrEmpty(str))
            {
                YaYun_OutputTextBox.Text = "没有输入任何内容！";
            }
            else
            {
                string onlychinesestr = DeleteNonChineseChar(str);
                JiebaSegmenter segmenter = new JiebaSegmenter();
                var segments = segmenter.Cut(onlychinesestr);
                List<string> hanzilist = new List<string>();

                List<string> pinyinlist = new List<string>();
                string temp = "";
                foreach (string item in segments)
                {
                    hanzilist.Add(item);
                    foreach (char ch in item)
                    {
                        ChineseChar chineseChar = new ChineseChar(ch);
                        temp = temp.Insert(temp.Length, chineseChar.Pinyins[0]);
                    }
                    pinyinlist.Add(temp.Remove(temp.Length - 1).ToLower());
                    temp = "";
                }

                Regex EndA = new Regex(@"^(\w)*a$");
                Regex EndO = new Regex(@"^(\w)*[^a]o$");
                Regex EndE = new Regex(@"^(\w)*[^iyuv]e$");
                Regex EndI = new Regex(@"^(\w)*[^zcshraeu]i$");
                Regex EndU = new Regex(@"^(\w)*[^jqxyio]u$");
                Regex EndV = new Regex(@"^(\w)*[jqxy][uv]$");
                Regex EndAI = new Regex(@"^(\w)*ai$");
                Regex EndEI = new Regex(@"^(\w)*[eu]i$");
                Regex EndAO = new Regex(@"^(\w)*ao$");
                Regex EndOU = new Regex(@"^(\w)*[oi]u$");
                Regex EndIE = new Regex(@"^(\w)*[iy]e$");
                Regex EndUE = new Regex(@"^(\w)*[uv]e$");
                Regex EndER = new Regex(@"^(\w)*er$");
                Regex EndAN1 = new Regex(@"^(\w)*[^yiu]an$");
                Regex EndAN2 = new Regex(@"^(\w)*[zcshdtnlgk]uan$");
                Regex EndIAN1 = new Regex(@"^(\w)*[yi]an$");
                Regex EndIAN2 = new Regex(@"^(\w)*[yjqx]uan$");
                Regex EndEN = new Regex(@"^(\w)*[bpmfdtnlgkhrzcsw][eu]n$");
                Regex EndIN = new Regex(@"^(\w)*in(g?)$");
                Regex EndUN = new Regex(@"^(\w)*[yjqx]un$");
                Regex EndANG = new Regex(@"^(\w)*ang$");
                Regex EndENG = new Regex(@"^(\w)*[dtnlgkhrzcsh]eng$");
                Regex EndONG1 = new Regex(@"^(\w)*ong$");
                Regex EndONG2 = new Regex(@"^(\w)*[bpmw]eng$");
                Regex EndSI = new Regex(@"^(\w)*[zcs]i$");
                Regex EndRI = new Regex(@"^(\w)*[rh]i$");

                List<string> endAList = new List<string>();
                List<string> endOList = new List<string>();
                List<string> endEList = new List<string>();
                List<string> endIList = new List<string>();
                List<string> endUList = new List<string>();
                List<string> endVList = new List<string>();
                List<string> endAIList = new List<string>();
                List<string> endEIList = new List<string>();
                List<string> endAOList = new List<string>();
                List<string> endOUList = new List<string>();
                List<string> endIEList = new List<string>();
                List<string> endUEList = new List<string>();
                List<string> endERList = new List<string>();
                List<string> endANList = new List<string>();
                List<string> endIANList = new List<string>();
                List<string> endENList = new List<string>();
                List<string> endINList = new List<string>();
                List<string> endUNList = new List<string>();
                List<string> endANGList = new List<string>();
                List<string> endENGList = new List<string>();
                List<string> endONGList = new List<string>();
                List<string> endSIList = new List<string>();
                List<string> endRIList = new List<string>();



                for (int i = 0; i < pinyinlist.Count; i++)
                {
                    if (EndA.IsMatch(pinyinlist[i]))
                    {
                        endAList.Add(hanzilist[i]);
                    }
                    else if (EndO.IsMatch(pinyinlist[i]))
                    {
                        endOList.Add(hanzilist[i]);
                    }
                    else if (EndE.IsMatch(pinyinlist[i]))
                    {
                        endEList.Add(hanzilist[i]);
                    }
                    else if (EndI.IsMatch(pinyinlist[i]))
                    {
                        endIList.Add(hanzilist[i]);
                    }
                    else if (EndU.IsMatch(pinyinlist[i]))
                    {
                        endUList.Add(hanzilist[i]);
                    }
                    else if (EndV.IsMatch(pinyinlist[i]))
                    {
                        endVList.Add(hanzilist[i]);
                    }
                    else if (EndAI.IsMatch(pinyinlist[i]))
                    {
                        endAIList.Add(hanzilist[i]);
                    }
                    else if (EndEI.IsMatch(pinyinlist[i]))
                    {
                        endEIList.Add(hanzilist[i]);
                    }
                    else if (EndAO.IsMatch(pinyinlist[i]))
                    {
                        endAOList.Add(hanzilist[i]);
                    }
                    else if (EndOU.IsMatch(pinyinlist[i]))
                    {
                        endOUList.Add(hanzilist[i]);
                    }
                    else if (EndIE.IsMatch(pinyinlist[i]))
                    {
                        endIEList.Add(hanzilist[i]);
                    }
                    else if (EndUE.IsMatch(pinyinlist[i]))
                    {
                        endUEList.Add(hanzilist[i]);
                    }
                    else if (EndER.IsMatch(pinyinlist[i]))
                    {
                        endERList.Add(hanzilist[i]);
                    }
                    else if (EndAN1.IsMatch(pinyinlist[i]) || EndAN2.IsMatch(pinyinlist[i]))
                    {
                        endANList.Add(hanzilist[i]);
                    }
                    else if (EndIAN1.IsMatch(pinyinlist[i]) || EndIAN2.IsMatch(pinyinlist[i]))
                    {
                        endIANList.Add(hanzilist[i]);
                    }
                    else if (EndEN.IsMatch(pinyinlist[i]))
                    {
                        endENList.Add(hanzilist[i]);
                    }
                    else if (EndIN.IsMatch(pinyinlist[i]))
                    {
                        endINList.Add(hanzilist[i]);
                    }
                    else if (EndUN.IsMatch(pinyinlist[i]))
                    {
                        endUNList.Add(hanzilist[i]);
                    }
                    else if (EndANG.IsMatch(pinyinlist[i]))
                    {
                        endANGList.Add(hanzilist[i]);
                    }
                    else if (EndENG.IsMatch(pinyinlist[i]))
                    {
                        endENGList.Add(hanzilist[i]);
                    }
                    else if (EndONG1.IsMatch(pinyinlist[i]) || EndONG2.IsMatch(pinyinlist[i]))
                    {
                        endONGList.Add(hanzilist[i]);
                    }
                    else if (EndSI.IsMatch(pinyinlist[i]))
                    {
                        endSIList.Add(hanzilist[i]);
                    }
                    else if (EndRI.IsMatch(pinyinlist[i]))
                    {
                        endRIList.Add(hanzilist[i]);
                    }
                }

                endAList = endAList.Distinct().ToList();
                endOList = endOList.Distinct().ToList();
                endEList = endEList.Distinct().ToList();
                endIList = endIList.Distinct().ToList();
                endUList = endUList.Distinct().ToList();
                endVList = endVList.Distinct().ToList();
                endAIList = endAIList.Distinct().ToList();
                endEIList = endEIList.Distinct().ToList();
                endAOList = endAOList.Distinct().ToList();
                endOUList = endOUList.Distinct().ToList();
                endIEList = endIEList.Distinct().ToList();
                endUEList = endUEList.Distinct().ToList();
                endERList = endERList.Distinct().ToList();
                endANList = endANList.Distinct().ToList();
                endIANList = endIANList.Distinct().ToList();
                endENList = endENList.Distinct().ToList();
                endINList = endINList.Distinct().ToList();
                endUNList = endUNList.Distinct().ToList();
                endANGList = endANGList.Distinct().ToList();
                endENGList = endENGList.Distinct().ToList();
                endONGList = endONGList.Distinct().ToList();
                endRIList = endRIList.Distinct().ToList();
                endSIList = endSIList.Distinct().ToList();


                string finalresult = "";

                if (endAList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "a: ");
                    for (int i = 0; i < endAList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endAList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endOList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "o: ");
                    for (int i = 0; i < endOList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endOList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endEList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "e: ");
                    for (int i = 0; i < endEList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endEList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endIList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "i: ");
                    for (int i = 0; i < endIList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endIList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endUList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "u: ");
                    for (int i = 0; i < endUList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endUList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endVList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "ü: ");
                    for (int i = 0; i < endVList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endVList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endAIList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "ai: ");
                    for (int i = 0; i < endAIList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endAIList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endEIList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "ei:");
                    for (int i = 0; i < endEIList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endEIList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endAOList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "ao: ");
                    for (int i = 0; i < endAOList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endAOList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endOUList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "ou: ");
                    for (int i = 0; i < endOUList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endOUList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endIEList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "ie: ");
                    for (int i = 0; i < endIEList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endIEList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endUEList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "ue: ");
                    for (int i = 0; i < endUEList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endUEList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endERList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "er: ");
                    for (int i = 0; i < endERList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endERList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endANList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "an: ");
                    for (int i = 0; i < endANList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endANList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endIANList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "ian: ");
                    for (int i = 0; i < endIANList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endIANList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endENList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "en: ");
                    for (int i = 0; i < endENList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endENList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endINList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "in or ing: ");
                    for (int i = 0; i < endINList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endINList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endUNList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "un: ");
                    for (int i = 0; i < endUNList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endUNList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endANGList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "ang:");
                    for (int i = 0; i < endANGList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endANGList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endENGList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "eng:");
                    for (int i = 0; i < endENGList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endENGList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endONGList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "ong:");
                    for (int i = 0; i < endONGList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endONGList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endSIList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "si:");
                    for (int i = 0; i < endSIList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endSIList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                if (endRIList.Count > 0)
                {
                    finalresult = finalresult.Insert(finalresult.Length, "ri:");
                    for (int i = 0; i < endRIList.Count; i++)
                    {
                        finalresult = finalresult.Insert(finalresult.Length, endRIList[i] + " ");
                    }
                    finalresult = finalresult.Insert(finalresult.Length, "\n");
                }

                YaYun_OutputTextBox.Text = finalresult;
            }
        }

        private void CiXing_MakeSeriesButton_Click(object sender, RoutedEventArgs e)
        {
            string str = CiXing_TypeInTextBox.Text;
            if (string.IsNullOrEmpty(str))
            {
                CiXing_OutputTextBox.Text = "没有输入任何内容！";
            }
            else
            {
                string onlychinesestr = DeleteNonChineseChar(str);

                AutoSerielizeWords(onlychinesestr);

                NList = NList.Distinct().ToList();
                NRList = NRList.Distinct().ToList();
                NSList = NSList.Distinct().ToList();
                NTList = NTList.Distinct().ToList();
                TList = TList.Distinct().ToList();
                SList = SList.Distinct().ToList();
                FList = FList.Distinct().ToList();
                VList = VList.Distinct().ToList();
                VSList = VSList.Distinct().ToList();
                VTList = VTList.Distinct().ToList();
                ADJList = ADJList.Distinct().ToList();
                BList = BList.Distinct().ToList();
                ZList = ZList.Distinct().ToList();
                RList = RList.Distinct().ToList();
                MList = MList.Distinct().ToList();
                QList = QList.Distinct().ToList();
                DList = DList.Distinct().ToList();
                PList = PList.Distinct().ToList();
                CList = CList.Distinct().ToList();
                UList = UList.Distinct().ToList();
                HOKALost = HOKALost.Distinct().ToList();

                string finalResult = "";

                string[] typeOfWord = { "名词", "人名", "地名", "专有名词", "时间词", "处所词", "方位词", "普通动词", "助动词", "其他动词", "形容词", "区别词", "状态词", "代词", "数词", "量词", "副词", "介词", "连词", "助词", "其他词语" };

                var wordsList = new List<string>[] { NList, NRList, NSList, NTList, TList, SList, FList, VList, VSList, VTList, ADJList, BList, ZList, RList, MList, QList, DList, PList, CList, UList, HOKALost }.ToList();

                int count = 0;
                foreach (var item in wordsList)
                {
                    if (item.Count > 0)
                    {
                        string temp = typeOfWord[count] + ":\n";
                        for (int i = 0; i < item.Count; i++)
                        {
                            temp = temp.Insert(temp.Length, item[i] + " ");
                        }
                        temp = temp.Insert(temp.Length, "\n\n");
                        finalResult = finalResult.Insert(finalResult.Length, temp);
                    }
                    count++;
                }

                CiXing_OutputTextBox.Text = finalResult;

            }
        }

        private void MakeTenSentence_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(GongShi_InputText.Text))
                {
                    ShowGongshiResult();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message,"提示");             
            }
        }

        private void ClearGongShiTextBox_Click(object sender, RoutedEventArgs e)
        {
            GongShi_OutputText.Clear();
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                StreamReader reader = new StreamReader(path);
                string temp = reader.ReadToEnd();
                GongShi_InputText.Text = temp;
                FilePathLabel.Text = path;
                reader.Close();
            }
            else FilePathLabel.Text = "未选择文件！";
        }

        private SentenceInfomation MakeSentence()
        {
            ClearWordLists();
            string typeInText = GongShi_InputText.Text;
            typeInText = DeleteNonChineseChar(typeInText);
            AutoSerielizeWords(typeInText);

            var obj1 = NList;
            obj1.AddRange(NRList);
            obj1.AddRange(NSList);
            obj1.AddRange(NTList);

            var obj2 = VList;
            obj2.AddRange(VSList);
            obj2.AddRange(VTList);

            var obj3 = ADJList;

            var obj4 = MList;

            var obj5 = TList;
            obj5.AddRange(SList);
            obj5.AddRange(FList);
            obj5.AddRange(BList);
            obj5.AddRange(ZList);
            obj5.AddRange(RList);
            obj5.AddRange(QList);
            obj5.AddRange(DList);
            obj5.AddRange(PList);
            obj5.AddRange(CList);
            obj5.AddRange(UList);
            obj5.AddRange(HOKALost);

            if (obj1.Count > 0 && obj2.Count > 0 && obj3.Count > 0 )
            {
                Random rd = new Random();
                string temp = "";
                string partn1, partn2, partv, parta;


                if (obj4.Count > 0)
                {
                    if (rd.NextDouble() > 0.1)
                        partn1 = obj1[rd.Next(0, obj1.Count())];
                    else
                        partn1 = obj4[rd.Next(0, obj4.Count())] + obj1[rd.Next(0, obj1.Count())];
                }
                else
                {
                    partn1 = obj1[rd.Next(0, obj1.Count())];
                }

                Thread.Sleep(30);

                if (obj4.Count > 0)
                {
                    if (rd.NextDouble() > 0.1)
                        partn2 = obj1[rd.Next(0, obj1.Count())];
                    else
                        partn2 = obj4[rd.Next(0, obj4.Count())] + obj1[rd.Next(0, obj1.Count())];
                }
                else
                {
                        partn2 = obj1[rd.Next(0, obj1.Count())];          
                }

                if (obj5.Count > 0)
                {
                    if (rd.NextDouble() > 0.35)
                        partv = obj2[rd.Next(0, obj2.Count())];
                    else
                        partv = obj5[rd.Next(0, obj5.Count())] + obj2[rd.Next(0, obj2.Count())];
                }
                else
                {
                    partv = obj2[rd.Next(0, obj2.Count())];
                }

                parta = obj3[rd.Next(0, obj3.Count())];

                if (SentencePatternCheck1.IsChecked == true)
                    temp = partn1 + partv + partn2;
                else if (SentencePatternCheck2.IsChecked == true)
                    temp = partn1 + parta + partv + partn2;
                else if (SentencePatternCheck3.IsChecked == true)
                    temp = partn1 + partv + parta;
                else if (SentencePatternCheck4.IsChecked == true)
                {
                    int num = rd.Next(1, 4);
                    switch (num)
                    {
                        case 1:
                            temp = partn1 + partv + partn2;
                            break;
                        case 2:
                            temp = partn1 + parta + partv + partn2;
                            break;
                        case 3:
                            temp = partn1 + partv + parta;
                            break;
                    }

                }
                else temp = null;
                var sentence = new SentenceInfomation(temp);
                return sentence;
            }
            else if(obj1.Count == 0)
            {
                var sentence = new SentenceInfomation { HasN = false };
                return sentence;
            }
            else if (obj2.Count == 0)
            {
                var sentence = new SentenceInfomation { HasV = false };
                return sentence;
            }
            else if (obj3.Count == 0)
            {
                var sentence = new SentenceInfomation { HasADJ = false };
                sentence.HasADJ = false;
                return sentence;
            }
            else
            return null; 
        }

        private bool IsMatch(string str)
        {
            if (Ratio1.IsChecked==true)
            {
                if (str.Length <= 4)
                    return true;
                else
                    return false;
            }
            else if (Ratio2.IsChecked==true)
            {
                if (str.Length >= 5 && str.Length <= 7)
                    return true;
                else
                    return false;

            }
            else if (Ratio3.IsChecked == true)
            {
                if (str.Length >= 8 && str.Length <= 10)
                    return true;
                else
                    return false;
            }
            else if (Ratio4.IsChecked == true)
            {
                if (str.Length >= 11)
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        private void UST_OpenTxtFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                string path = openFile.FileName;
                StreamReader reader = new StreamReader(path);
                Lyrics.Text = reader.ReadToEnd();
                reader.Close();
                TXTLabel.Text = path;
            }
            else
                TXTLabel.Text = "请选择txt文件！";
        }

        private void UST_OpenUstFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
            {
                string path = openFile.FileName;
                StreamReader reader = new StreamReader(path);
                UTAUtext = reader.ReadToEnd();
                UTAUsplitText = UTAUtext.Split('\n');
                reader.Close();
                status = 1;
                USTLabel.Text = path;
            }
            else
                USTLabel.Text = "请选择ust文件！";
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (status == 1)
            {
                string Lyric = Lyrics.Text;
                Lyric = DeleteNonChineseChar(Lyric);
                if (!string.IsNullOrEmpty(Lyric))
                {
                    List<string> pinyinList = ConvertToPinyin(Lyric);
                    Regex regex = new Regex(@"^Lyric=(.)*$");
                    Regex regex2 = new Regex(@"^Lyric=R(.)*$");
                    Regex regex3 = new Regex(@"^\[#(\d)+\](.)*$");
                    Regex regex4 = new Regex(@"^\[#0000\](.)*$");
                    Regex regex5 = new Regex(@"^\[#TRACKEND\](.)*$");
                    for (int i = 0, j = 0; i < UTAUsplitText.Length; i++)
                    {
                        if (regex.IsMatch(UTAUsplitText[i]) && (!regex2.IsMatch(UTAUsplitText[i])))
                        {
                            if (j < pinyinList.Count)
                            {
                                UTAUsplitText[i] = "Lyric=" + pinyinList[j] + "\n";
                            }
                            j++;
                        }

                        if (UseFlags.IsChecked == true)
                        {
                            if ((regex3.IsMatch(UTAUsplitText[i]) || regex5.IsMatch(UTAUsplitText[i])) && !regex4.IsMatch(UTAUsplitText[i]))
                            {
                                UTAUsplitText[i] = "Flags=" + Flags.Text + "\n" + UTAUsplitText[i];
                            }

                        }

                    }
                    string FinalResult = "";
                    for (int i = 0; i < UTAUsplitText.Length; i++)
                    {
                        FinalResult = FinalResult.Insert(FinalResult.Length, UTAUsplitText[i]);
                    }

                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.DefaultExt = "ust";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filename = saveFileDialog.FileName;
                        if (!string.IsNullOrEmpty(filename))
                        {
                            StreamWriter writer = new StreamWriter(filename, true, Encoding.UTF8);
                            writer.Write(FinalResult);
                            writer.Close();
                        }
                    }
                    else USTLabel.Text = "请选择保存目录！";
                }
                else MessageBox.Show("没有输入文本！", "提示");
            }
            else USTLabel.Text = "请选择ust文件！";
        }

        private List<string> ConvertToPinyin(string str)
        {
            List<string> textConverResult = new List<string>();
            char[] tempChar = str.ToCharArray();
            for (int i = 0; i < tempChar.Length; i++)
            {
                ChineseChar chineseChar = new ChineseChar(tempChar[i]);
                string pinyin = chineseChar.Pinyins.ToList().First();
                pinyin = pinyin.Remove(pinyin.Length - 1).ToLower();
                textConverResult.Add(pinyin);
            }
            return textConverResult;
        }

        private void ShowGongshiResult()
        {
            for (int i = 0; i < 10; i++)
            {
                int count = 0;
            again:
                var newsentence = MakeSentence();
                count++;
                string str = newsentence.Sentence;
                if (!string.IsNullOrEmpty(str))
                {
                    if (IsMatch(str))
                    { GongShi_OutputText.Text = GongShi_OutputText.Text.Insert(GongShi_OutputText.Text.Length, str + "\n"); }
                    else
                    {
                        if (count < 100)
                            goto again;
                        else
                            throw new GongShiException("没有足够的符合条件的词语用来生成。");
                    }
                    Thread.Sleep(35);
                }
                else if (newsentence.HasN == false)
                {
                    throw new GongShiException("没有名词用来生成。");
                }
                else if (newsentence.HasV == false)
                {
                    throw new GongShiException("没有动词用来生成。");
                }
                else if (newsentence.HasADJ == false)
                {
                    throw new GongShiException("没有形容词用来生成。");
                }
            }
        }

        private void Xinci_MakeTwoCharWord_Click(object sender, RoutedEventArgs e)
        {
            string InputText = DeleteNonChineseChar(Xinci_TypeInTextBox.Text);
            string OutputText = Xinci_OutputTextBox.Text;
            Random rd = new Random();
            List<string> result = new List<string>();
            int count = 1;
            Start:
            if (InputText.Length > 2)
            {
                JiebaSegmenter segmenter = new JiebaSegmenter();
                var segments = segmenter.Cut(InputText);
                var TextArray = InputText.ToCharArray();
                int rd1 = rd.Next(0, TextArray.Length - 1);
                Thread.Sleep(25);
                int rd2 = rd.Next(0, TextArray.Length - 1);
                char[] NewWordArray = { TextArray[rd1], TextArray[rd2] };
                string NewWord = new string(NewWordArray);
                if(!IsInSegment(NewWord,segments) && IsAWord(NewWord))
                {
                    result.Add(NewWord);
                }
                count++;
                if (count < 50)
                { goto Start; }
                result = result.Distinct().ToList();
                foreach(var item in result)
                {
                   OutputText = OutputText.Insert(OutputText.Length, item + " ");
                }
                Xinci_OutputTextBox.Text = OutputText;
            }

            else
            {
                MessageBox.Show("没有输入有效内容", "提示");
            }
        }

        private void Xinci_MakeThreeCharWord_Click(object sender, RoutedEventArgs e)
        {
            string InputText = DeleteNonChineseChar(Xinci_TypeInTextBox.Text);
            string OutputText = Xinci_OutputTextBox.Text;
            Random rd = new Random();
            List<string> result = new List<string>();
            int count = 1;
        Start:
            if (InputText.Length > 3)
            {
                JiebaSegmenter segmenter = new JiebaSegmenter();
                var segments = segmenter.Cut(InputText);
                var TextArray = InputText.ToCharArray();
                int rd1 = rd.Next(0, TextArray.Length - 1);
                Thread.Sleep(25);
                int rd2 = rd.Next(0, TextArray.Length - 1);
                Thread.Sleep(25);
                int rd3 = rd.Next(0, TextArray.Length - 1);
                char[] NewWordArray = { TextArray[rd1], TextArray[rd2] ,TextArray[rd3]};
                string NewWord = new string(NewWordArray);
                if (!IsInSegment(NewWord, segments) && IsAWord(NewWord))
                {
                    result.Add(NewWord);
                }
                count++;
                if (count < 50)
                { goto Start; }
                result = result.Distinct().ToList();
                foreach (var item in result)
                {
                    OutputText = OutputText.Insert(OutputText.Length, item + " ");
                }
                Xinci_OutputTextBox.Text = OutputText;
            }
            else
            {
                MessageBox.Show("没有输入有效内容", "提示");
            }

        }

        private bool IsInSegment(string word, IEnumerable<string> segment)
        {
            foreach(var item in segment)
            {
                if (word == item)
                    return true;
            }
            return false;
        }

        private bool IsAWord(string word)
        {
            using (StreamReader reader = new StreamReader(DicPath))
            {
                string DicText = reader.ReadToEnd();

                if (DicText.IndexOf(word) != -1)
                    return true;
            }
            return false;
        }

        private void ChooseAudioButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
