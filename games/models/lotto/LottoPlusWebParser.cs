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
    public class LottoPlusWebParser
    {
        private HtmlAgilityPack.HtmlDocument _htmldoc = new HtmlAgilityPack.HtmlDocument();
        private Dictionary<string, string> _loadResults;
        private Dictionary<string, string> _parseResults;
        private Boolean _parsed;
        private Boolean _loaded;
        private string _parsedUrl;
        private Dictionary<string, Image> _winimages;
        private Dictionary<string, Image> _sortedwinimages;
        private Dictionary<string, string> _imageUrls = new Dictionary<string, string>();
        private Uri _baseUrl;
        public  LottoPlus _lottoplus;


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

        public LottoPlus LottoPlus
        {
            get
            {
                return _lottoplus;
            }
            set
            {
                _lottoplus = value;
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
            _lottoplus = new LottoPlus();
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

        private void SetParseOptions()
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
                    ParseGameName();
                    ParseGameImage();
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

        private void ParseGameImage()
        {
            HtmlAgilityPack.HtmlNode html_img;
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    html_img = _htmldoc.DocumentNode.SelectSingleNode("//table/tbody/tr[1]/td/table/tbody/tr/td[1]/img");
                    if (html_img.Attributes["src"].Value.Length > 0)
                    {
                        var url = new Uri(_baseUrl, html_img.Attributes["src"].Value);
                        _lottoplus.GameImage = GetImage(url.AbsoluteUri);
                    }
                    if ((_lottoplus.GameImage != null))
                        _parseResults.Add("parse-gameimage", "ok");
                    else
                        _parseResults.Add("parse-gameimage", "failed-on-validation");
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
                _lottoplus.GameImage = null;
            }
        }

        private void ParseGameName()
        {
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    _lottoplus.GameName = _htmldoc.DocumentNode.SelectSingleNode("//span[@class='jackPot'][1]").InnerText;
                    if ((_lottoplus.GameName != null) && (_lottoplus.GameName != ""))
                        _parseResults.Add("parse-gamename", "ok");
                    else
                        _parseResults.Add("parse-gamename", "failed-on-validation");
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
                _lottoplus.GameName = String.Empty;
            }
        }

        private void ParseGameDate()
        {
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    _lottoplus.GameDateStr = _htmldoc.DocumentNode.SelectSingleNode("//table/tbody/tr[2]/td[1]/span[@class='onGreenBackground']").InnerText;
                    if ((_lottoplus.GameDateStr != null) && (_lottoplus.GameDateStr != ""))
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
                _lottoplus.GameDateStr = String.Empty;
            }
        }

        private void ParseWinningNumbers()
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
                        if (html_img.Attributes["src"].Value.Contains(CommonController.StartLottoPlusParseToken))
                        {
                            var url = new Uri(_baseUrl, html_img.Attributes["src"].Value);
                            _winimages.Add("ball-" + Convert.ToString(count + 1), GetImage(url.AbsoluteUri));
                            _lottoplus.Numbers[count] = Convert.ToInt32(html_img.Attributes["src"].Value.Replace(CommonController.StartLottoPlusParseToken, "").Replace(CommonController.EndLottoPlusParseToken, ""));
                        }
                    }
                    _parseResults.Add("inner-exception", "ok");
                }
                catch (Exception e)
                {
                    _parseResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        _parseResults.Add("inner-exception", e.InnerException.Message);
                    _lottoplus.Numbers = null;
                    _lottoplus.Numbers = new int[6];
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lottoplus.Numbers = null;
                _lottoplus.Numbers = new int[6];
            }
        }

        private void ParseBonusNumber()
        {
            HtmlAgilityPack.HtmlNode html_img;
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    html_img = _htmldoc.DocumentNode.SelectSingleNode("//table/tbody/tr/td[3]/div[@align='center']/img");

                    if (html_img.Attributes["src"].Value.Contains(CommonController.StartLottoPlusParseToken))
                    {
                        var url = new Uri(_baseUrl, html_img.Attributes["src"].Value);
                        _winimages.Add("bonusball", GetImage(url.AbsoluteUri));
                        _lottoplus.GameBonus = Convert.ToInt32(html_img.Attributes["src"].Value.Replace(CommonController.StartLottoPlusParseToken, "").Replace(CommonController.EndLottoPlusParseToken, ""));
                    }
                    if (_lottoplus.GameBonus != 0)
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
                    _lottoplus.GameBonus = 0;
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lottoplus.GameBonus = 0;
            }
        }

        private void ParseSortedWinningNumbers()
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
                        if (html_img.Attributes["src"].Value.Contains(CommonController.StartLottoPlusParseToken))
                        {
                            var url = new Uri(_baseUrl, html_img.Attributes["src"].Value);
                            _sortedwinimages.Add("ball-" + Convert.ToString(count + 1), GetImage(url.AbsoluteUri));
                            _lottoplus.SortedNumbers[count] = Convert.ToInt32(html_img.Attributes["src"].Value.Replace(CommonController.StartLottoPlusParseToken, "").Replace(CommonController.EndLottoPlusParseToken, ""));
                        }
                    }
                    _parseResults.Add("parse-sortedwinnumbers", "ok");
                    foreach (int i in _lottoplus.SortedNumbers)
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
                    _lottoplus.SortedNumbers = null;
                    _lottoplus.SortedNumbers = new int[6];
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lottoplus.SortedNumbers = null;
                _lottoplus.SortedNumbers = new int[6];
            }
        }

        private void ParseSortedBonusNumber()
        {
            HtmlAgilityPack.HtmlNode html_div;
            HtmlAgilityPack.HtmlNode html_img;
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    html_div = _htmldoc.DocumentNode.SelectNodes("//table/tbody/tr/td[@class='bbottomYellow']/div[@align='center']")[3];
                    html_img = html_div.SelectSingleNode("img");

                    if (html_img.Attributes["src"].Value.Contains(CommonController.StartLottoPlusParseToken))
                    {
                        var url = new Uri(_baseUrl, html_img.Attributes["src"].Value);
                        _sortedwinimages.Add("bonusball", GetImage(url.AbsoluteUri));
                        _lottoplus.GameSortedBonus = Convert.ToInt32(html_img.Attributes["src"].Value.Replace(CommonController.StartLottoPlusParseToken, "").Replace(CommonController.EndLottoPlusParseToken, ""));
                    }
                    if (_lottoplus.GameBonus != 0)
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
                    _lottoplus.GameSortedBonus = 0;
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lottoplus.GameSortedBonus = 0;
            }
        }

        private void ParseDivisionalPayouts()
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
                                _lottoplus.DivisionalPayouts.AddPayout(paydiv);
                            }
                        }
                    }
                    if (_lottoplus.DivisionalPayouts.PayoutList.Count > 0)
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
                    _lottoplus.DivisionalPayouts = null;
                    _lottoplus.DivisionalPayouts = new DivisionalPayout();
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lottoplus.DivisionalPayouts = null;
                _lottoplus.DivisionalPayouts = new DivisionalPayout();
            }
        }

        private void ParseGameStatistics()
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
                                _lottoplus.DrawStatistics.RollOverAmountDescr = html_tds[0].InnerText.Trim();
                                _lottoplus.DrawStatistics.RollOverAmount = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 2:
                                _lottoplus.DrawStatistics.RollOverNumberDesc = html_tds[0].InnerText.Trim();
                                _lottoplus.DrawStatistics.RollOverNumber = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 3:
                                _lottoplus.DrawStatistics.TotalPrizePoolDesc = html_tds[0].InnerText.Trim();
                                _lottoplus.DrawStatistics.TotalPrizePool = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 4:
                                _lottoplus.DrawStatistics.TotalSalesDesc = html_tds[0].InnerText.Trim();
                                _lottoplus.DrawStatistics.TotalSales = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 5:
                                _lottoplus.DrawStatistics.NextEstimatedJackpotDesc = html_tds[0].InnerText.Trim();
                                _lottoplus.DrawStatistics.NextEstimatedJackpot = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 6:
                                _lottoplus.DrawStatistics.DrawMachineUsedDesc = html_tds[0].InnerText.Trim();
                                _lottoplus.DrawStatistics.DrawMachineUsed = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 7:
                                _lottoplus.DrawStatistics.BallSetUsedDesc = html_tds[0].InnerText.Trim();
                                _lottoplus.DrawStatistics.BallSetUsed = html_tds[1].InnerText.Trim();
                                statcounter = statcounter + 1;
                                break;
                            case 8:
                                _lottoplus.DrawStatistics.DrawNumberDesc = html_tds[0].InnerText.Trim();
                                _lottoplus.DrawStatistics.DrawNumber = html_tds[1].InnerText.Trim();
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
                    _lottoplus.DrawStatistics = null;
                    _lottoplus.DrawStatistics = new DrawStatistics();
                }
            }
            else
            {
                _parseResults.Add("parse-null", "Invalid html document node");
                _lottoplus.DrawStatistics = null;
                _lottoplus.DrawStatistics = new DrawStatistics();
            }
        }

        private void ParsePageHeadings()
        {
            ParseLatestResultsHeading();
            WinningNumbersHeading();
        }

        private void ParseLatestResultsHeading()
        {
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    _lottoplus.HeadingLastestResults = _htmldoc.DocumentNode.SelectSingleNode("//td[@class='onGreenBackground']").InnerText;
                    if ((_lottoplus.HeadingLastestResults != null) && (_lottoplus.HeadingLastestResults != ""))
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
                _lottoplus.HeadingLastestResults = "";
            }
        }

        private void WinningNumbersHeading()
        {
            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    _lottoplus.HeadingWinningNumbers = _htmldoc.DocumentNode.SelectSingleNode("//td[@class='btopbottomYellow']/strong").InnerText.Replace("&nbsp;", "");
                    if ((_lottoplus.HeadingWinningNumbers != null) && (_lottoplus.HeadingWinningNumbers != ""))
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
                _lottoplus.HeadingWinningNumbers = "";
            }
        }

        private Dictionary<string, Image> DownloadImages(List<string> imageUrls)
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


        private Image GetImage(string filePath)
        {
            WebClient l_WebClient = new WebClient();
            byte[] l_imageBytes = l_WebClient.DownloadData(filePath);
            MemoryStream l_stream = new MemoryStream(l_imageBytes);
            return Image.FromStream(l_stream);
        }
    }

 
}
