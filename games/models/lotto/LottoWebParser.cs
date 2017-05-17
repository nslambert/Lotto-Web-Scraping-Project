using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Json;
using System.IO;
using HtmlAgilityPack;
using System.Drawing;
using ttpim.gamemodule.lotto.controllers;

namespace ttpim.gamemodule.games.models.lotto
{
    public class LottoWebParser
    {
        private static HtmlAgilityPack.HtmlDocument _htmldoc = new HtmlAgilityPack.HtmlDocument();
        private static Dictionary<string, string> _loadResults;
        private static Dictionary<string, string> _parseResults;
        private static Boolean _parsed;
        private static Boolean _loaded;
        private static string _parsedUrl;
        private static Dictionary<string, Image> _winimages;
        private static Dictionary<string, Image> _sortedwinimages;
        private static Dictionary<string, string> _imageUrls = new Dictionary<string, string>();
        private static Uri _baseUrl;
        public  static LottoCard _lotto;


        public Boolean IsParsed
        {
            get
            {
                return _parsed;
            }
            set
            {
                _parsed = value;
            }
        }

        public Boolean IsLoaded
        {
            get
            {
                return _loaded;
            }
            set
            {
                _loaded = value;
            }
        }

        public LottoCard Lotto
        {
            get
            {
                return _lotto;
            }
            set
            {
                _lotto = value;
            }
        }

        public Dictionary<string, Image> WinImages
        {
            get
            {
                return _winimages;
            }
            set
            {
                _winimages = value;
            }
        }

        public Dictionary<string, Image> SortedWinImages
        {
            get
            {
                return _sortedwinimages;
            }
            set
            {
                _sortedwinimages = value;
            }
        }


        public void Initialise(String HtmlURL)
        {
            _loadResults = new Dictionary<string, string>();
            _parseResults = new Dictionary<string, string>();
            _parsedUrl = String.Empty;
            _lotto = new LottoCard(false);
            _winimages = new Dictionary<string, Image>();
            _sortedwinimages = new Dictionary<string, Image>();

            SetParseOptions();

            try
            {
                _htmldoc.Load(HtmlURL);
                _baseUrl = new Uri(HtmlURL);
                DoParsing("DoParseAll");
                if (_htmldoc.ParseErrors != null && _htmldoc.ParseErrors.ToList().Count > 0)
                {
                    _loadResults.Add("parse-errors", Convert.ToString(_htmldoc.ParseErrors.ToList().Count));
                    foreach (HtmlParseError parseerror in _htmldoc.ParseErrors.ToList())
                    {
                        _loadResults.Add(parseerror.Code.ToString(), parseerror.Reason);
                    }
                }
                _loadResults.Add("load-html", "ok");
            }
            catch (Exception e)
            {
                _loadResults.Add("load-html", "failed");
                _loadResults.Add("exception", e.Message);
                if (e.InnerException.Message != null)
                    _loadResults.Add("inner-exception", e.InnerException.Message);
            }
        }

        private static void SetParseOptions()
        {
            if (_htmldoc != null)
            {
                _htmldoc.OptionUseIdAttribute = true;
                _htmldoc.OptionAddDebuggingAttributes = true;
                _htmldoc.OptionAutoCloseOnEnd = true;
                _htmldoc.OptionCheckSyntax = true;
                _htmldoc.OptionDefaultStreamEncoding = System.Text.Encoding.Default;
            }
        }

        public void DoParsing(String ParseAction)
        {
            switch (ParseAction)
            {
                case "ParseGameDate":
                    ParseGameDate();
                    break;
                case "ParsePageHeadings":
                    ParsePageHeadings();
                    break;
                case "DoParseWinningNumbers":
                    ParseWinningNumbers();
                    break;
                case "DoParseNumericalWinningNumbers":
                    ParseSortedWinningNumbers();
                    break;
                case "ParseDivisionalPayouts":
                    ParseDivisionalPayouts();
                    break;
                case "ParseGameStatistics":
                    ParseGameStatistics();
                    break;
                case "DoParseAll":
                    ParseGameDate();
                    ParseWinningNumbers();
                    ParseBonusNumber();
                    ParseSortedWinningNumbers();
                    ParseSortedBonusNumber();
                    ParseDivisionalPayouts();
                    ParseGameStatistics();
                    ParsePageHeadings();
                    break;
                default:
                    break;
            }

        }

        private static void ParseGameDate()
        {
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    _lotto.GameDateStr = _htmldoc.DocumentNode.SelectSingleNode("//table/tbody/tr[2]/td[1]/span[@class='onGreenBackground']").InnerText;
                    if ((_lotto.GameDateStr != null) && (_lotto.GameDateStr != ""))
                        _parseResults.Add("parse-gamedate", "ok");
                    else
                        _parseResults.Add("parse-gamedate", "failed-on-validation");
                }
                catch (Exception e)
                {
                    _parseResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        _parseResults.Add("inner-exception", e.InnerException.Message);
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lotto.GameDateStr = String.Empty;
            }
        }

        private static void ParseWinningNumbers()
        {
            HtmlAgilityPack.HtmlNode html_div;
            HtmlAgilityPack.HtmlNodeCollection html_imgs;
            

            int count = -1;
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    html_div = _htmldoc.DocumentNode.SelectSingleNode("//table/tbody/tr/td[@class='bbottomYellow']/div[@align='center']");
                    html_imgs = html_div.SelectNodes("img");
                    foreach (HtmlNode html_img in html_imgs)
                    {
                        count = count + 1;
                        if (html_img.Attributes["src"].Value.Contains(CommonController.StartLottoParseToken))
                        {
                            var url = new Uri(_baseUrl, html_img.Attributes["src"].Value);
                            _winimages.Add("ball-" + Convert.ToString(count + 1), GetImage(url.AbsoluteUri));
                            _lotto.Numbers[count] = Convert.ToInt32(html_img.Attributes["src"].Value.Replace(CommonController.StartLottoParseToken, "").Replace(CommonController.EndLottoParseToken, ""));
                        }
                    }
                    _parseResults.Add("inner-exception", "ok");
                }
                catch (Exception e)
                {
                    _parseResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        _parseResults.Add("inner-exception", e.InnerException.Message);
                    _lotto.Numbers = null;
                    _lotto.Numbers = new int[6];
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lotto.Numbers = null;
                _lotto.Numbers = new int[6];
            }
        }

        private static void ParseBonusNumber()
        {
            HtmlAgilityPack.HtmlNode html_img;
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    html_img = _htmldoc.DocumentNode.SelectSingleNode("//table/tbody/tr/td[3]/div[@align='center']/img");

                    if (html_img.Attributes["src"].Value.Contains(CommonController.StartLottoParseToken))
                    {
                        var url = new Uri(_baseUrl, html_img.Attributes["src"].Value);
                        _winimages.Add("bonusball", GetImage(url.AbsoluteUri));
                        _lotto.Bonus = Convert.ToInt32(html_img.Attributes["src"].Value.Replace(CommonController.StartLottoParseToken, "").Replace(CommonController.EndLottoParseToken, ""));
                    }
                    if (_lotto.Bonus != 0)
                    {
                        _parseResults.Add("parse-bonusnumber", "ok");
                    }
                    else
                    {
                        _parseResults.Add("parse-bonusnumber", "failed-on-validation");
                    }
                }
                catch (Exception e)
                {
                    _parseResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        _parseResults.Add("inner-exception", e.InnerException.Message);
                    _lotto.Bonus = 0;
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lotto.Bonus = 0;
            }
        }

        private static void ParseSortedWinningNumbers()
        {
            HtmlAgilityPack.HtmlNode html_div;
            HtmlAgilityPack.HtmlNodeCollection html_imgs;
            
            int count = -1;
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    html_div = _htmldoc.DocumentNode.SelectNodes("//table/tbody/tr/td[@class='bbottomYellow']/div[@align='center']")[2];
                    html_imgs = html_div.SelectNodes("img");

                    foreach (HtmlNode html_img in html_imgs)
                    {
                        count = count + 1;
                        if (html_img.Attributes["src"].Value.Contains(CommonController.StartLottoParseToken))
                        {
                            var url = new Uri(_baseUrl, html_img.Attributes["src"].Value);
                            _sortedwinimages.Add("ball-" + Convert.ToString(count + 1), GetImage(url.AbsoluteUri));
                            _lotto.SortedNumbers[count] = Convert.ToInt32(html_img.Attributes["src"].Value.Replace(CommonController.StartLottoParseToken, "").Replace(CommonController.EndLottoParseToken, ""));
                        }
                    }
                    _parseResults.Add("parse-sortedwinnumbers", "ok");
                    foreach (int i in _lotto.SortedNumbers)
                    {
                        if ((i < 0) || (i == 0))
                            _parseResults["parse-sortedwinnumbers"] = "failed-on-validation";
                    }
                }
                catch (Exception e)
                {
                    _parseResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        _parseResults.Add("inner-exception", e.InnerException.Message);
                    _lotto.SortedNumbers = null;
                    _lotto.SortedNumbers = new int[6];
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lotto.SortedNumbers = null;
                _lotto.SortedNumbers = new int[6];
            }
        }

        private static void ParseSortedBonusNumber()
        {
            HtmlAgilityPack.HtmlNode html_div;
            HtmlAgilityPack.HtmlNode html_img;
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    html_div = _htmldoc.DocumentNode.SelectNodes("//table/tbody/tr/td[@class='bbottomYellow']/div[@align='center']")[3];
                    html_img = html_div.SelectSingleNode("img");

                    if (html_img.Attributes["src"].Value.Contains(CommonController.StartLottoParseToken))
                    {
                        var url = new Uri(_baseUrl, html_img.Attributes["src"].Value);
                        _sortedwinimages.Add("bonusball", GetImage(url.AbsoluteUri));
                        _lotto.SortedBonus = Convert.ToInt32(html_img.Attributes["src"].Value.Replace(CommonController.StartLottoParseToken, "").Replace(CommonController.EndLottoParseToken, ""));
                    }
                    if (_lotto.Bonus != 0)
                    {
                        _parseResults.Add("parse-sortedbonusnumber", "ok");
                    }
                    else
                    {
                        _parseResults.Add("parse-sortedbonusnumber", "failed-on-validation");
                    }
                }
                catch (Exception e)
                {
                    _parseResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        _parseResults.Add("inner-exception", e.InnerException.Message);
                    _lotto.SortedBonus = 0;
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lotto.SortedBonus = 0;
            }
        }

        private static void ParseDivisionalPayouts()
        {
            HtmlAgilityPack.HtmlNodeCollection html_tds;
            HtmlAgilityPack.HtmlNodeCollection html_rows;

            int count = 0;

            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    html_rows = _htmldoc.DocumentNode.SelectNodes("//table[@id='Table26']/tbody/tr");
                    foreach (HtmlNode row in html_rows)
                    {
                        count = count + 1;
                        if (count > 1)
                        {
                            html_tds = row.SelectNodes("td");

                            if (html_tds.Count == 3)
                            {
                                Division paydiv = new Division
                                {
                                    Name = html_tds[0].InnerText,
                                    NumberOfWinners = Convert.ToInt32(html_tds[1].InnerText),
                                    PayoutPerWinner = html_tds[2].InnerText.Replace("&nbsp;", "")
                                };
                                _lotto.DivionalPayouts.AddPayout(paydiv);
                            }
                        }
                    }
                    if (_lotto.DivionalPayouts.PayoutList.Count > 0)
                        _parseResults.Add("parse-divpayouts", "ok");
                    else
                    {
                        _parseResults.Add("parse-divpayouts", "failed-on-validation");
                    }
                }
                catch (Exception e)
                {
                    _parseResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        _parseResults.Add("inner-exception", e.InnerException.Message);
                    _lotto.DivionalPayouts = null;
                    _lotto.DivionalPayouts = new DivisionalPayout();
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lotto.DivionalPayouts = null;
                _lotto.DivionalPayouts = new DivisionalPayout();
            }
        }

        private static void ParseGameStatistics()
        {
            HtmlAgilityPack.HtmlNodeCollection html_rows;
            HtmlAgilityPack.HtmlNodeCollection html_tds;
            int count = 0;
            int statcounter = 0;

            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    html_rows = _htmldoc.DocumentNode.SelectNodes("//table[@id='Table9']/tbody/tr");
                    foreach (HtmlNode row in html_rows)
                    {
                        count = count + 1;
                        html_tds = row.SelectNodes("td");
                        switch (count)
                        {
                            case 1:
                                _lotto.DrawStatistics.RollOverAmountDescr = html_tds[0].InnerText.Trim();
                                _lotto.DrawStatistics.RollOverAmount = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 2:
                                _lotto.DrawStatistics.RollOverNumberDesc = html_tds[0].InnerText.Trim();
                                _lotto.DrawStatistics.RollOverNumber = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 3:
                                _lotto.DrawStatistics.TotalPrizePoolDesc = html_tds[0].InnerText.Trim();
                                _lotto.DrawStatistics.TotalPrizePool = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 4:
                                _lotto.DrawStatistics.TotalSalesDesc = html_tds[0].InnerText.Trim();
                                _lotto.DrawStatistics.TotalSales = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 5:
                                _lotto.DrawStatistics.NextEstimatedJackpotDesc = html_tds[0].InnerText.Trim();
                                _lotto.DrawStatistics.NextEstimatedJackpot = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 6:
                                _lotto.DrawStatistics.DrawMachineUsedDesc = html_tds[0].InnerText.Trim();
                                _lotto.DrawStatistics.DrawMachineUsed = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 7:
                                _lotto.DrawStatistics.BallSetUsedDesc = html_tds[0].InnerText.Trim();
                                _lotto.DrawStatistics.BallSetUsed = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 8:
                                _lotto.DrawStatistics.DrawNumberDesc = html_tds[0].InnerText.Trim();
                                _lotto.DrawStatistics.DrawNumber = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            default:
                                break;
                        }
                    }
                    if (statcounter > 0)
                        _parseResults.Add("parse-drawstats", "ok");
                    else
                    {
                        _parseResults.Add("parse-drawstats", "failed-on-validation");
                    }

                }
                catch (Exception e)
                {
                    _parseResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        _parseResults.Add("inner-exception", e.InnerException.Message);
                    _lotto.DrawStatistics = null;
                    _lotto.DrawStatistics = new DrawStatistics();
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lotto.DrawStatistics = null;
                _lotto.DrawStatistics = new DrawStatistics();
            }
        }

        private static void ParsePageHeadings()
        {
            ParseLatestResultsHeading();
            WinningNumbersHeading();
        }

        private static void ParseLatestResultsHeading()
        {
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    _lotto.HeadingLastestResults = _htmldoc.DocumentNode.SelectSingleNode("//td[@class='onGreenBackground']").InnerText;
                    if ((_lotto.HeadingLastestResults != null) && (_lotto.HeadingLastestResults != ""))
                        _parseResults.Add("parse-heading-latestresults", "ok");
                    else
                        _parseResults.Add("parse-heading-latestresults", "failed-on-validation");
                }
                catch (Exception e)
                {
                    _parseResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        _parseResults.Add("inner-exception", e.InnerException.Message);
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lotto.HeadingLastestResults = "";
            }
        }

        private static void WinningNumbersHeading()
        {
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    _lotto.HeadingWinningNumbers = _htmldoc.DocumentNode.SelectSingleNode("//td[@class='btopbottomYellow']/strong").InnerText.Replace("&nbsp;", "");
                    if ((_lotto.HeadingWinningNumbers != null) && (_lotto.HeadingWinningNumbers != ""))
                        _parseResults.Add("parse-heading-winnumbers", "ok");
                    else
                        _parseResults.Add("parse-heading-winnumbers", "failed-on-validation");
                }
                catch (Exception e)
                {
                    _parseResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        _parseResults.Add("inner-exception", e.InnerException.Message);
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lotto.HeadingWinningNumbers = "";
            }
        }

        private static Dictionary<string, Image> DownloadImages(List<string> imageUrls)
        {
            Dictionary<string, Image> images = new Dictionary<string, Image>();
            int count = 0;

            foreach (string url in imageUrls)
            {
                count = count + 1;
                images.Add("ball-" + Convert.ToString(count), GetImage(url));
            }
            return images;
        }


        private static Image GetImage(string filePath)
        {
            WebClient l_WebClient = new WebClient();
            byte[] l_imageBytes = l_WebClient.DownloadData(filePath);
            MemoryStream l_stream = new MemoryStream(l_imageBytes);
            return Image.FromStream(l_stream);
        }
    }
}
