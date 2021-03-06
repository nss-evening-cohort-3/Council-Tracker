﻿using Council_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace Council_Tracker.DAL
{

    public class ResolutionData
    {
        public int numberOf2015Res = 0;
        public int numberOf2016Res = 0;
        public int numberOf2017Res = 0;
        public int highest2015ResNumber = 0;
        public int highest2016ResNumber = 0;
        public int highest2017ResNumber = 0;
        public void highestResNumCollector()
        {
            WebClient client = new WebClient();
            string indexHTML = client.DownloadString($"http://www.nashville.gov/Metro-Clerk/Legislative/Resolutions/2015-2019.aspx");
            string resNumberPattern2015 = @"www\.nashville\.gov\/mc\/resolutions\/term_2015_2019\/rs[0-9]{4}_[0-9]*.htm"">RESOLUTION\sRS2015-(?<num15>[0-9]*)<\/a>";
            string resNumberPattern2016 = @"www\.nashville\.gov\/mc\/resolutions\/term_2015_2019\/rs[0-9]{4}_[0-9]*.htm"">RESOLUTION\sRS2016-(?<num16>[0-9]*)<\/a>";
            string resNumberPattern2017 = @"www\.nashville\.gov\/mc\/resolutions\/term_2015_2019\/rs[0-9]{4}_[0-9]*.htm"">RESOLUTION\sRS2017-(?<num17>[0-9]*)<\/a>";
            Regex resNumberRgx2015 = new Regex(resNumberPattern2015);
            Regex resNumberRgx2016 = new Regex(resNumberPattern2016);
            Regex resNumberRgx2017 = new Regex(resNumberPattern2017);
            MatchCollection resNumberMatch2015 = resNumberRgx2015.Matches(indexHTML);
            MatchCollection resNumberMatch2016 = resNumberRgx2016.Matches(indexHTML);
            MatchCollection resNumberMatch2017 = resNumberRgx2017.Matches(indexHTML);
            string resNumber2015 = resNumberMatch2015[0].Groups["num15"].Value;
            string resNumber2016 = resNumberMatch2016[0].Groups["num16"].Value;
            string resNumber2017 = resNumberMatch2017[0].Groups["num17"].Value;
            highest2015ResNumber = Convert.ToInt32(resNumber2015);
            highest2016ResNumber = Convert.ToInt32(resNumber2016);
            highest2017ResNumber = Convert.ToInt32(resNumber2017);
            numberOf2015Res = highest2015ResNumber;
            numberOf2016Res = highest2016ResNumber - highest2015ResNumber;
            numberOf2017Res = highest2017ResNumber - highest2016ResNumber;
        }

        public Resolution[] resolutionScraper()
        {
            int allYears = numberOf2015Res + numberOf2016Res + numberOf2017Res;
            WebClient client = new WebClient();
            highestResNumCollector();
            Resolution[] scrapedResolutions = new Resolution[numberOf2017Res];
            for (var i = 503; i < highest2017ResNumber+1; i++) // 503 IS FIRST 20217 NUMBER
            {
                Resolution resolution = new Resolution();
                string rawHtml = client.DownloadString($"http://www.nashville.gov/mc/resolutions/term_2015_2019/rs2016_{i}.htm"); // THEY HAVEN'T CHANGED THE URL TO REFLECT 2017 YET. 
                
                resolution.ResNumber = i;
                resolution.Year = 2017;

                string captionPattern = @"<\/font><\/b>(?<caption>A resolution.*?)<\/p>";
                Regex captionRgx = new Regex(captionPattern);
                Match captionMatch = captionRgx.Match(rawHtml);
                string caption = captionMatch.Groups["caption"].Value;
                resolution.Caption = caption;

                string bodyTextPattern = @"<p class=""LEGISLATION"">(?<body>.)*<\/p>";
                Regex bodyTextRgx = new Regex(bodyTextPattern);
                MatchCollection bodyTextMatch = bodyTextRgx.Matches(rawHtml);
                string bodyText = "";
                for (var j = 0; j < bodyTextMatch.Count; j++)
                {
                    string match = bodyTextMatch[j].ToString();
                    bodyText += match;
                }
                resolution.Body = bodyText;

                scrapedResolutions[i - 503] = resolution;
            }
            return scrapedResolutions;
        }

        //public void billscrapeCleaner()
        //{
        //Ideally this goes in the middle of resolution scrape to clean up the body text. Keep an eye out for bills that include docs/graphs/tables, etc. Need to know how to handle. 
        //}
        }
}


///// MY OLDER SCRAPER --- To be implemented after I get some actual data in...
//public Resolution[] resolutionScraper()
//{
//    WebClient client = new WebClient();
//    highestResNumCollector();
//    Resolution[] scrapedResolutions = new Resolution[highest2017ResNumber];
//    for (var i = 77; i < highest2017ResNumber + 1; i++)
//    {
//        Resolution resolution = new Resolution();
//        string rawHtml = client.DownloadString($"http://www.nashville.gov/mc/resolutions/term_2015_2019/rs2016_{i}.htm");

//        resolution.ResNumber = i;

//        string bodyTextPattern = @"<p class=""LEGISLATION"">(?<body>.)*<\/p>";
//        Regex bodyTextRgx = new Regex(bodyTextPattern);
//        MatchCollection bodyTextMatch = bodyTextRgx.Matches(rawHtml);
//        string bodyText = "";
//        for (var j = 0; j < bodyTextMatch.Count; j++)
//        {
//            string match = bodyTextMatch[j].ToString();
//            bodyText += match;
//        }
//        if (bodyText == "")
//        {
//            string altBodyTextPattern = @"<p><font\ssize=""2""(.*)?<\/p>";
//            Regex altBodyTextRgx = new Regex(altBodyTextPattern);
//            MatchCollection altBodyTextMatch = altBodyTextRgx.Matches(rawHtml);
//            for (var k = 0; k < altBodyTextMatch.Count; k++)
//            {
//                string altMatch = altBodyTextMatch[k].ToString();
//                bodyText += altMatch;
//            }
//        }
//        resolution.Body = bodyText;

//        string captionPattern = @"<p class=""SUMMARY"">.*\s\S?(?<summary>.*?)<\/p>";
//        Regex captionRgx = new Regex(captionPattern);
//        Match captionMatch = captionRgx.Match(rawHtml);
//        string summary = captionMatch.Groups["summary"].Value;
//        if (summary == "")
//        {
//            string altCaptionPattern = @"<p\salign=""center"">\s?(?<summary>.*?)<\/p>";
//            Regex altCaptionRgx = new Regex(altCaptionPattern);
//            Match altCaptionMatch = altCaptionRgx.Match(rawHtml);
//            summary = altCaptionMatch.Groups["summary"].Value;
//        }
//        if (summary == "")
//        {
//            string altCaptionPattern = @"<p><font\ssize=""-1"">(?<summary>.*?)?<\/p>";
//            Regex altCaptionRgx = new Regex(altCaptionPattern);
//            Match altCaptionMatch = altCaptionRgx.Match(rawHtml);
//            summary = altCaptionMatch.Groups["summary"].Value;
//        }
//        resolution.Caption = summary;

//        // NEEDS TO BE REFACTORED INTO A DICTIONARY SO THAT IT CAN CONTAIN A STRING URL AND A STRING NAME...
//        //string exhibitURLsPattern = @"<p class=""ordinancecontent""><a href=""(?< body >.)*"">(?<docName>.)*<\/a><\/p>";
//        //Regex exhibitURLsRgx = new Regex(exhibitURLsPattern);
//        //MatchCollection exhibitURLsMatch = exhibitURLsRgx.Matches(rawHtml);
//        ////List<string> textsList = new List<string>();
//        //string exhibitURLs = "";
//        //for (var j = 0; j < exhibitURLsMatch.Count; j++)
//        //{
//        //    string match = exhibitURLsMatch[j].ToString();
//        //    //textsList.Add(match);
//        //    exhibitURLs += match;
//        //}
//        //resolution.Body = exhibitURLs;

//        //string statusPattern = @"";
//        //Regex statusRgx = new Regex(statusPattern);
//        //Match statusMatch = statusRgx.Match(rawHtml);
//        //string status = statusMatch.Groups["status"].Value;
//        //resolution.CurrentStatus = status;

//        //string sponsorPattern = @"Sponsored by: (?<sponsor>.*?)<\/p>";
//        //Regex sponsorRgx = new Regex(sponsorPattern);
//        //Match sponsorMatch = sponsorRgx.Match(rawHtml);
//        //string sponsor = sponsorMatch.Groups["sponsor"].Value; // probably need .Trim() ?? 
//        //resolution.Sponsor = List<sponsor>;

//        //string codePattern = @"";
//        //Regex codeRgx = new Regex(codePattern);
//        //Match codeMatch = codeRgx.Match(rawHtml);
//        //string codeText = codeMatch.Groups["codes"].Value;
//        //resolution.CodeSections = codeText;


//        //string historyPattern = @"";
//        //Regex historyRgx = new Regex(historyPattern);
//        //Match historyMatch = historyRgx.Match(rawHtml);
//        //string history = historyMatch.Groups["history"].Value;
//        //resolution.History = history;

//        //string enactmentDatePattern = @"";
//        //Regex enactmentDateRgx = new Regex(enactmentDatePattern);
//        //Match enactmentDateMatch = enactmentDateRgx.Match(rawHtml);
//        //DateTime enactmentDate = enactmentDateMatch.Groups["name"].Value;
//        //resolution.EnactmentDate = enactmentDate;

//        scrapedResolutions[i - 77] = resolution;
//    }
//    return scrapedResolutions;
//}
