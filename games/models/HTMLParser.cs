using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.Net.Json;
using System.IO;
using HtmlAgilityPack;
using System.Drawing;
using ttpim.gamemodule.games.models;
using ttpim.gamemodule.lotto.controllers;
using ttpim.gamemodule.common;


namespace ttpim.gamemodule.games.controllers
{
    public class HTMLParser
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
        public  HTMLParsedResults _results;
        private string _savePath;

        public HtmlAgilityPack.HtmlDocument HTMLDocument
        {
            get
            {
                return _htmldoc;
            }
            set
            {
                _htmldoc = value;
            }
        }

        public Dictionary<string, string> LoadResults
        {
            get
            {
                return _loadResults;
            }
            set
            {
                _loadResults = value;
            }
        }

        public Dictionary<string, string> ParsedResults
        {
            get
            {
                return _parseResults;
            }
            set
            {
                _parseResults = value;
            }
        }
        public String SavePath
        {
            get 
            {
                return _savePath; 
            }
            set 
            {
                _savePath = value; 
            }
        }

        public Uri BaseURI
        {
            get
            {
                return _baseUrl;
            }
            set
            {
                _baseUrl = value;
            }
        }

        public Boolean IsParsed
        {
            get{return _parsed;}
            set{_parsed = value;}
        }

        public Boolean IsLoaded
        {
            get{return _loaded;}
            set{_loaded = value;}
        }

        public HTMLParsedResults HTMLParsedResults
        {
            get
            { 
                return _results;
            }
            set
            { 
                _results = value;
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

        public void Initialise(String SavePath)
        {
            LoadResults = new Dictionary<string, string>();
            ParsedResults = new Dictionary<string, string>();
            _parsedUrl = String.Empty;
            HTMLParsedResults = new HTMLParsedResults();
            HTMLParsedResults.WinImages = new Dictionary<string, Image>();
            HTMLParsedResults.SortedWinImages = new Dictionary<string, Image>();
            WinImages = new Dictionary<string, Image>();
            SortedWinImages = new Dictionary<string, Image>();
            _savePath = SavePath;

            SetParseOptions();

            try
            {
                HTMLDocument.Load(SavePath);
                BaseURI = new Uri(SavePath);
                int errorcount = 0;
                DoParsing("DoParseAll");
                if (HTMLDocument.ParseErrors != null && HTMLDocument.ParseErrors.ToList().Count > 0)
                {
                    LoadResults.Add("parse-errors", Convert.ToString(HTMLDocument.ParseErrors.ToList().Count));
                    foreach (HtmlParseError parseerror in HTMLDocument.ParseErrors.ToList())
                    {
                        errorcount = errorcount + 1;
                        LoadResults.Add(parseerror.Code.ToString() + "-" + Convert.ToString(errorcount) , parseerror.Reason);
                    }
                }
                LoadResults.Add("load-html", "ok");
            }
            catch (Exception e)
            {
                LoadResults.Add("load-html", "failed");
                LoadResults.Add("exception", e.Message);
                if (e.InnerException != null)
                    LoadResults.Add("inner-exception", e.InnerException.Message);
            }
        }

        private void SetParseOptions()
        {
            if (HTMLDocument != null)
            {
                HTMLDocument.OptionUseIdAttribute = true;
                HTMLDocument.OptionAddDebuggingAttributes = true;
                HTMLDocument.OptionAutoCloseOnEnd = true;
                HTMLDocument.OptionCheckSyntax = true;
                HTMLDocument.OptionDefaultStreamEncoding = System.Text.Encoding.Default;
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
                    ParseGameLogo();
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


        private void ParseGameLogo()
        {
            HtmlAgilityPack.HtmlNode html_td = null;
            HtmlAgilityPack.HtmlNode child = null;
            if (HTMLDocument.DocumentNode != null)
            {
                try
                {
                    html_td = HTMLDocument.DocumentNode.SelectSingleNode("//td[@class='bbottomyello'][1]");

                    if ((html_td != null) && (html_td.HasChildNodes))
                    {
                        child = html_td.ChildNodes[0];
                        if (child.Attributes["src"].Value.Length > 0)
                        {
                            var url = new Uri(BaseURI, child.Attributes["src"].Value);
                            string localpath = url.LocalPath;
                            HTMLParsedResults.LogoImage = CommonWebController.GetLocalImage(localpath);
                        }
                    }
                    if ((HTMLParsedResults.LogoImage != null))
                        ParsedResults.Add("parse-logoimage", "ok");
                    else
                        ParsedResults.Add("parse-logoimage", "failed-on-validation");
                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.LogoImage = null;
            }
        }

        private void ParseGameImage()
        {
            HtmlAgilityPack.HtmlNode html_td = null;
            HtmlAgilityPack.HtmlNode child = null;
            if (HTMLDocument.DocumentNode != null)
            {
                try
                {
                    html_td = HTMLDocument.DocumentNode.SelectSingleNode("//div[@id='topNav']/table/tr[1]/td[1]");

                    if ((html_td != null) && (html_td.HasChildNodes))
                    {
                        child = html_td.ChildNodes[0];
                        if (child.Attributes["src"].Value.Length > 0)
                        {
                            var url = new Uri(BaseURI, child.Attributes["src"].Value);
                            string localpath = url.LocalPath;
                            HTMLParsedResults.GameImage = CommonWebController.GetLocalImage(localpath);
                        }
                    }
                    if ((HTMLParsedResults.GameImage != null))
                        ParsedResults.Add("parse-gameimage", "ok");
                    else
                        ParsedResults.Add("parse-gameimage", "failed-on-validation");
                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.GameDate = String.Empty;
            }
        }

        private void ParseGameName()
        {
            if (HTMLDocument.DocumentNode != null)
            {
                try
                {
                    HTMLParsedResults.GameName = "Powerball ® Results";
                    if ((HTMLParsedResults.GameName != null) && (HTMLParsedResults.GameName != ""))
                        ParsedResults.Add("parse-gamename", "ok");
                    else
                        ParsedResults.Add("parse-gamename", "failed-on-validation");
                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.GameName = String.Empty;
            }
        }

        private void ParseGameDate()
        {
            HtmlAgilityPack.HtmlNode html_span = null;
            HtmlAgilityPack.HtmlNode child = null;

            if (_htmldoc.DocumentNode != null)
            {
                try
                {
                    html_span = HTMLDocument.DocumentNode.SelectSingleNode("//span[@class='onGreenBackground']");
                    if ((html_span != null) && (html_span.HasChildNodes))
                    {
                        child = html_span.ChildNodes[0];
                        HTMLParsedResults.GameDate = child.InnerText;
                    }
                    if ((HTMLParsedResults.GameDate != null) && (HTMLParsedResults.GameDate != ""))
                        ParsedResults.Add("parse-gamedate", "ok");
                    else
                        ParsedResults.Add("parse-gamedate", "failed-on-validation");
                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.GameDate = String.Empty;
            }
        }

        private void ParseWinningNumbers()
        {
            HtmlAgilityPack.HtmlNode html_div;
            HtmlAgilityPack.HtmlNodeCollection html_imgs;
            
            int count = -1;
            if (HTMLDocument.DocumentNode != null)
            {
                try
                {
                    html_div = HTMLDocument.DocumentNode.SelectSingleNode("//td[@class='bbottomYellow']/div");
                    if (html_div != null)
                    {
                        html_imgs = html_div.SelectNodes("img");
                        if (html_imgs != null)
                        {
                            foreach (HtmlNode html_img in html_imgs)
                            {
                                count = count + 1;
                                if (html_img.Attributes["src"].Value.Length > 0)
                                {
                                    var uri = new Uri(BaseURI, html_img.Attributes["src"].Value);
                                    string path = uri.LocalPath;
                                    
                                    HTMLParsedResults.WinImages.Add("ball-" + Convert.ToString(count + 1), CommonWebController.GetLocalImage(path));
                                    int startindex = html_img.Attributes["src"].Value.IndexOf(".");
                                    int endindex = html_img.Attributes["src"].Value.IndexOf("/ball_") + 6;
                                    string ball = html_img.Attributes["src"].Value.Remove(startindex, endindex).Replace(".gif", "");
                                    HTMLParsedResults.Numbers.Add(Convert.ToInt32(ball));
                                }
                            }
                        }
                        ParsedResults.Add("parse-winnumbers", "ok");
                    }
                    else
                    {
                        ParsedResults.Add("parse-winnumbers", "not found");
                    }
                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                    HTMLParsedResults.Numbers = null;
                    HTMLParsedResults.Numbers = new ArrayList();
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.Numbers = null;
                HTMLParsedResults.Numbers = new ArrayList();
            }
        }

        private void ParseBonusNumber()
        {
            HtmlAgilityPack.HtmlNode html_img;
            if (HTMLDocument.DocumentNode != null)
            {
                try
                {
                    html_img = HTMLDocument.DocumentNode.SelectSingleNode("//td[@class='bbottomYellow'][3]/div/img");
                    if ((html_img != null) && (html_img.Attributes["src"].Value.Length > 0))
                    {
                        var uri = new Uri(BaseURI, html_img.Attributes["src"].Value);
                        string path = uri.LocalPath;
                        HTMLParsedResults.WinImages.Add("bonusball", CommonWebController.GetLocalImage(path));
                        int startindex = html_img.Attributes["src"].Value.IndexOf(".");
                        int endindex = 0;
                        if (html_img.Attributes["src"].Value.IndexOf("/ball_") > 0)
                        {
                            endindex = html_img.Attributes["src"].Value.IndexOf("/ball_") + 6;
                        }
                        else if (html_img.Attributes["src"].Value.IndexOf("/power_") > 0)
                        {
                            endindex = html_img.Attributes["src"].Value.IndexOf("/power_") + 7;
                        }
                        string ball = html_img.Attributes["src"].Value.Remove(startindex, endindex).Replace(".gif", "").Replace("power_", "").Replace("balls/", "");
                        HTMLParsedResults.Bonus = Convert.ToInt32(ball);
                    }
                    if (HTMLParsedResults.Bonus != 0)
                    {
                        ParsedResults.Add("parse-bonusnumber", "ok");
                    }
                    else
                    {
                        ParsedResults.Add("parse-bonusnumber", "failed-on-validation");
                    }
                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                    HTMLParsedResults.Bonus = 0;
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.Bonus = 0;
            }
        }

        private void ParseSortedWinningNumbers()
        {
            HtmlAgilityPack.HtmlNode html_div;
            HtmlAgilityPack.HtmlNodeCollection html_divs;
            HtmlAgilityPack.HtmlNodeCollection html_imgs;
            
            int count = -1;
            if (HTMLDocument.DocumentNode != null)
            {
                try
                {
                    html_divs = HTMLDocument.DocumentNode.SelectNodes("//td[@class='bbottomYellow']/div");
                    if (html_divs != null)
                    {
                        html_div = html_divs[2];
                        if (html_div != null)
                        {
                            html_imgs = html_div.SelectNodes("img");

                            foreach (HtmlNode html_img in html_imgs)
                            {
                                count = count + 1;
                                if (html_img.Attributes["src"].Value.Length > 0)
                                {
                                    var uri = new Uri(BaseURI, html_img.Attributes["src"].Value);
                                    string path = uri.LocalPath;
                                    HTMLParsedResults.SortedWinImages.Add("ball-" + Convert.ToString(count + 1), CommonWebController.GetLocalImage(path));
                                    int startindex = html_img.Attributes["src"].Value.IndexOf(".");
                                    int endindex = html_img.Attributes["src"].Value.IndexOf("/ball_") + 6;
                                    string ball = html_img.Attributes["src"].Value.Remove(startindex, endindex).Replace(".gif", "");
                                    HTMLParsedResults.SortedNumbers.Add(Convert.ToInt32(ball));
                                }
                            }
                            ParsedResults.Add("parse-sortedwinnumbers", "ok");
                        }
                        else
                        {
                            ParsedResults.Add("parse-sortedwinnumbers", "not found");
                        }
                    }
                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                    HTMLParsedResults.SortedNumbers = null;
                    HTMLParsedResults.SortedNumbers = new ArrayList();
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.SortedNumbers = null;
                HTMLParsedResults.SortedNumbers = new ArrayList();
            }
        }

        private void ParseSortedBonusNumber()
        {
            HtmlAgilityPack.HtmlNodeCollection html_divs;
            HtmlAgilityPack.HtmlNode html_div;
            HtmlAgilityPack.HtmlNode html_img;
            if (HTMLDocument.DocumentNode != null)
            {
                try
                {
                    html_divs = HTMLDocument.DocumentNode.SelectNodes("//td[@class='bbottomYellow']/div");
                    if (html_divs != null)
                    {
                        html_div = html_divs[3];
                        if (html_div != null)
                        {
                            html_img = html_div.SelectSingleNode("img");
                            if ((html_img != null) && ((html_img.Attributes["src"].Value.Length > 0)))
                            {
                                var uri = new Uri(BaseURI, html_img.Attributes["src"].Value);
                                string path = uri.LocalPath;
                                HTMLParsedResults.SortedWinImages.Add("bonusball", CommonWebController.GetLocalImage(path));
                                int startindex = html_img.Attributes["src"].Value.IndexOf(".");
                                int endindex = 0;
                                if (html_img.Attributes["src"].Value.IndexOf("/ball_") > 0)
                                {
                                    endindex = html_img.Attributes["src"].Value.IndexOf("/ball_") + 6;
                                }
                                else if (html_img.Attributes["src"].Value.IndexOf("/power_") > 0)
                                {
                                    endindex = html_img.Attributes["src"].Value.IndexOf("/power_") + 7;
                                }
                                string ball = html_img.Attributes["src"].Value.Remove(startindex, endindex).Replace(".gif", "").Replace("power_", "").Replace("balls/", "");
                                
                                HTMLParsedResults.SortedBonus = Convert.ToInt32(ball);
                            }
                        }
                        else
                        {
                            ParsedResults.Add("parse-sortedbonusnumber", "not found");
                        }
                    }

                    if (HTMLParsedResults.Bonus != 0)
                    {
                        ParsedResults.Add("parse-sortedbonusnumber", "ok");
                    }
                    else
                    {
                        ParsedResults.Add("parse-sortedbonusnumber", "failed-on-validation");
                    }
                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                    HTMLParsedResults.SortedBonus = 0;
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.SortedBonus = 0;
            }
        }

        private void ParseDivisionalPayouts()
        {
            HtmlAgilityPack.HtmlNodeCollection html_tds;
            HtmlAgilityPack.HtmlNodeCollection html_rows;

            int count = 0;

            if (HTMLDocument.DocumentNode != null)
            {
                try
                {
                    html_rows = HTMLDocument.DocumentNode.SelectNodes("//table[@id='Table26']/tbody/tr");
                    if (html_rows != null)
                    {
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
                                    HTMLParsedResults.DivPayouts.AddPayout(paydiv);
                                }
                            }
                        }
                    }
                    if ((HTMLParsedResults.DivPayouts != null) && (HTMLParsedResults.DivPayouts.PayoutList != null) && (HTMLParsedResults.DivPayouts.PayoutList.Count > 0))
                        ParsedResults.Add("parse-divpayouts", "ok");
                    else
                    {
                        ParsedResults.Add("parse-divpayouts", "failed-on-validation");
                    }
                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                    HTMLParsedResults.DivPayouts = null;
                    HTMLParsedResults.DivPayouts = new DivisionalPayout();
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.DivPayouts = null;
                HTMLParsedResults.DivPayouts = new DivisionalPayout();
            }
        }

        private void ParseGameStatistics()
        {
            HtmlAgilityPack.HtmlNodeCollection html_rows;
            HtmlAgilityPack.HtmlNodeCollection html_tds;
            int count = 0;
            int statcounter = 0;

            if (HTMLDocument.DocumentNode != null)
            {
                try
                {
                    html_rows = HTMLDocument.DocumentNode.SelectNodes("//table[@id='Table9']/tbody/tr");
                    if (html_rows != null)
                    {
                        foreach (HtmlNode row in html_rows)
                        {
                            count = count + 1;
                            html_tds = row.SelectNodes("td");
                            switch (count)
                            {
                                case 1:
                                    HTMLParsedResults.DrawStatistics.RollOverAmountDescr = html_tds[0].InnerText.Trim();
                                    HTMLParsedResults.DrawStatistics.RollOverAmount = html_tds[1].InnerText.Trim();
                                    statcounter = statcounter + 1;
                                    break;
                                case 2:
                                    HTMLParsedResults.DrawStatistics.RollOverNumberDesc = html_tds[0].InnerText.Trim();
                                    HTMLParsedResults.DrawStatistics.RollOverNumber = html_tds[1].InnerText.Trim();
                                    statcounter = statcounter + 1;
                                    break;
                                case 3:
                                    HTMLParsedResults.DrawStatistics.TotalPrizePoolDesc = html_tds[0].InnerText.Trim();
                                    HTMLParsedResults.DrawStatistics.TotalPrizePool = html_tds[1].InnerText.Trim();
                                    statcounter = statcounter + 1;
                                    break;
                                case 4:
                                    HTMLParsedResults.DrawStatistics.TotalSalesDesc = html_tds[0].InnerText.Trim();
                                    HTMLParsedResults.DrawStatistics.TotalSales = html_tds[1].InnerText.Trim();
                                    statcounter = statcounter + 1;
                                    break;
                                case 5:
                                    HTMLParsedResults.DrawStatistics.NextEstimatedJackpotDesc = html_tds[0].InnerText.Trim();
                                    HTMLParsedResults.DrawStatistics.NextEstimatedJackpot = html_tds[1].InnerText.Trim();
                                    statcounter = statcounter + 1;
                                    break;
                                case 6:
                                    HTMLParsedResults.DrawStatistics.DrawMachineUsedDesc = html_tds[0].InnerText.Trim();
                                    HTMLParsedResults.DrawStatistics.DrawMachineUsed = html_tds[1].InnerText.Trim();
                                    statcounter = statcounter + 1;
                                    break;
                                case 7:
                                    HTMLParsedResults.DrawStatistics.BallSetUsedDesc = html_tds[0].InnerText.Trim();
                                    HTMLParsedResults.DrawStatistics.BallSetUsed = html_tds[1].InnerText.Trim();
                                    statcounter = statcounter + 1;
                                    break;
                                case 8:
                                    HTMLParsedResults.DrawStatistics.DrawNumberDesc = html_tds[0].InnerText.Trim();
                                    HTMLParsedResults.DrawStatistics.DrawNumber = html_tds[1].InnerText.Trim();
                                    statcounter = statcounter + 1;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if (statcounter > 0)
                        ParsedResults.Add("parse-drawstats", "ok");
                    else
                    {
                        ParsedResults.Add("parse-drawstats", "failed-on-validation");
                    }

                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                    HTMLParsedResults.DrawStatistics = null;
                    HTMLParsedResults.DrawStatistics = new DrawStatistics();
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.DrawStatistics = null;
                HTMLParsedResults.DrawStatistics = new DrawStatistics();
            }
        }

        private void ParsePageHeadings()
        {
            ParseLatestResultsHeading();
            WinningNumbersHeading();
        }

        private void ParseLatestResultsHeading()
        {
            if (HTMLDocument.DocumentNode != null)
            {
                try
                {
                    HTMLParsedResults.HeadingLastestResults = HTMLDocument.DocumentNode.SelectSingleNode("//td[@class='onGreenBackground']").InnerText;
                    if ((HTMLParsedResults.HeadingLastestResults != null) && (HTMLParsedResults.HeadingLastestResults != ""))
                        ParsedResults.Add("parse-heading-latestresults", "ok");
                    else
                        ParsedResults.Add("parse-heading-latestresults", "failed-on-validation");
                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.HeadingLastestResults = "";
            }
        }

        private void WinningNumbersHeading()
        {
            if (HTMLDocument.DocumentNode != null)
            {
                try
                {
                    HTMLParsedResults.HeadingWinningNumbers = HTMLDocument.DocumentNode.SelectSingleNode("//td[@class='btopbottomYellow']/strong").InnerText.Replace("&nbsp;", "");
                    if ((HTMLParsedResults.HeadingWinningNumbers != null) && (HTMLParsedResults.HeadingWinningNumbers != ""))
                        ParsedResults.Add("parse-heading-winnumbers", "ok");
                    else
                        ParsedResults.Add("parse-heading-winnumbers", "failed-on-validation");
                }
                catch (Exception e)
                {
                    ParsedResults.Add("parse-exception", e.Message);
                    if (e.InnerException.Message != null)
                        ParsedResults.Add("inner-exception", e.InnerException.Message);
                }
            }
            else
            {
                ParsedResults.Add("parse-null", "Invalid html document node");
                HTMLParsedResults.HeadingWinningNumbers = "";
            }
        }

        private Dictionary<string, Image> DownloadRemoteImages(List<string> imageUrls)
        {
            Dictionary<string, Image> images = new Dictionary<string, Image>();
            int count = 0;

            foreach (string url in imageUrls)
            {
                count = count + 1;
                images.Add("ball-" + Convert.ToString(count), CommonWebController.GetRemoteImage(url));
            }
            return images;
        }
    }
}
