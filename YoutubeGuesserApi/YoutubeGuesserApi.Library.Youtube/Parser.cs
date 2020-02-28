using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.Library.Youtube
{
    public class Parser
    {
        private readonly Random rnd = new Random();

        public YoutubeVideo ParseYoutubeVideo(string jsonData)
        {
            try
            {
                var parsedData = JObject.Parse(jsonData);

                string resultsPerPage = parsedData["pageInfo"]["resultsPerPage"].ToString();
                int.TryParse(resultsPerPage, out int results);
                int elementIndex = rnd.Next(results); // number between 0 and (results - 1)

                string videoId = parsedData["items"][elementIndex]["id"]["videoId"].ToString();
                string title = parsedData["items"][elementIndex]["snippet"]["title"].ToString();
                string channelName = parsedData["items"][elementIndex]["snippet"]["channelTitle"].ToString();

                return new YoutubeVideo(videoId, title, channelName);
            }
            // parameter was not valid JSON
            catch (JsonReaderException)
            {
                return null;
            }
            // unable to get some of the values, e.g. title
            catch (NullReferenceException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public long ParseYoutubeVideoViewCount(string jsonData)
        {
            try
            {
                var parsedData = JObject.Parse(jsonData);
                string viewCount = parsedData["items"][0]["statistics"]["viewCount"].ToString();
                Int64.TryParse(viewCount, out long views);
                return views;
            }
            // parameter was not valid JSON
            catch (JsonReaderException)
            {
                return -1;
            }
            // unable to get some of the values, e.g. title
            catch (NullReferenceException)
            {
                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public List<YoutubeVideo> ParseYoutubeVideosFromTrending(string jsonData, int videoCount)
        {
            List<YoutubeVideo> youtubeVideos = new List<YoutubeVideo>();
            try
            {
                var parsedData = JObject.Parse(jsonData);
                for(int i = 0; i < videoCount; i++)
                {
                    string videoId = parsedData["items"][i]["id"].ToString();
                    string title = parsedData["items"][i]["snippet"]["title"].ToString();
                    string channelName = parsedData["items"][i]["snippet"]["channelTitle"].ToString();
                    string viewCountString = parsedData["items"][i]["statistics"]["viewCount"].ToString();
                    int.TryParse(viewCountString, out int viewCount);
                    youtubeVideos.Add(new YoutubeVideo(videoId, title, channelName, viewCount));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return youtubeVideos;
        }

        public List<YoutubeVideo> ParseYoutubeVideosFromSearch(string jsonData, int videoCount)
        {
            List<YoutubeVideo> youtubeVideos = new List<YoutubeVideo>();
            try
            {
                var parsedData = JObject.Parse(jsonData);
                for(int i = 0; i < videoCount; i++)
                {
                    string videoId = parsedData["items"][i]["id"]["videoId"].ToString();
                    string title = parsedData["items"][i]["snippet"]["title"].ToString();
                    string channelName = parsedData["items"][i]["snippet"]["channelTitle"].ToString();
                    youtubeVideos.Add(new YoutubeVideo(videoId, title, channelName));
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return youtubeVideos;
        }

        public string ParseTokenAndVideoCount(string jsonData, out int videoCount)
        {
            videoCount = -1;
            try
            {
                var parsedData = JObject.Parse(jsonData);
                string resultsPerPage = parsedData["pageInfo"]["resultsPerPage"].ToString();
                var nextPageToken = parsedData["nextPageToken"];

                int.TryParse(resultsPerPage, out videoCount);
                if (nextPageToken == null)
                    return null;
                else
                    return nextPageToken.ToString();
            }
            // parameter was not valid JSON
            catch (JsonReaderException)
            {
                return null;
            }
            // unable to get some of the values, e.g. title
            catch (NullReferenceException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public YoutubeError ParseYoutubeError(string jsonData)
        {
            try
            {
                var parsedData = JObject.Parse(jsonData);
                var status = parsedData["error"]["code"].ToString();
                int.TryParse(status, out int statusCode);

                var errors = parsedData["error"]["errors"][0];
                string domain = errors["domain"].ToString();
                string reason = errors["reason"].ToString();
                string message = errors["message"].ToString();

                return new YoutubeError(statusCode, domain, reason, message);
            }
            catch
            {
                return new YoutubeError(-1, null, null, null);
            }
        }
    }
}