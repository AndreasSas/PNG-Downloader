// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using System.Net;
using System.Xml.Linq;

namespace SporeDownloader.Lib;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class SporeServer {
    private const string _endpointHttps = "http://www.spore.com";
    // private static string _endpointHttp = "http://www.spore.com";
    
    private HttpClient _httpClient = new ();

    public async Task<XDocument> GetData(string url) {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        
        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Failed to get data from server: {response.StatusCode}");
        }
        
        var content = await response.Content.ReadAsStringAsync();
        return XDocument.Parse(content);
    }
    

    /// <summary>
    /// Get daily stats about Spore.com
    /// </summary>
    public async Task<XDocument> GetStats() {
        return await GetData($"{_endpointHttps}/rest/stats");
    }

    
    
    public XDocument getStats()
    {
        return XDocument.Load($"{_endpointHttps}/rest/stats");
    }
    
    /// <summary>
    /// Get various stats like height, diet, abilities etc. for a creature, if you know it's asset Id
    /// </summary>
    public XDocument getCreatureStats(long assetId) {
        return XDocument.Load($"{_endpointHttps}/rest/creature/{assetId}");
    }

    /// <summary>
    /// Get profile pic, tagline, user id and creation date for a username
    /// </summary>
    public XDocument getProfileInfo(string username) {
        return XDocument.Load($"{_endpointHttps}/rest/user/{username}");
    }

    /// <summary>
    /// Get asset id, name, creation date, type, parent and rating for a list of assets created by a user
    /// </summary>
    public XDocument getAssetsForUser(string username, int startIndex, int length) {
        return XDocument.Load($"{_endpointHttps}/rest/assets/user/{username}/{startIndex}/{length}");
    }

    /// <summary>
    /// Get id, name, tags, subscription count, rating etc. for Sporecasts subscribed to by a user
    /// </summary>
    public XDocument getAssetsForSporecast(long sporecastId, int startIndex, int length)
    {
        return XDocument.Load($"{_endpointHttps}/rest/assets/sporecast/{sporecastId}/{startIndex}/{length}");
    }

    /// <summary>
    /// Get number of achievements for user and a list of achievement ids and unlock-dates
    /// </summary>
    public XDocument getAchievementsForUser(string username, int startIndex, int length)
    {
        return XDocument.Load($"{_endpointHttps}/rest/achievements/{username}/{startIndex}/{length}");
    }

    /// <summary>
    /// For a given asset id, get name, description, tags, 10 latest comments, type, parent, rating, creation date and author name/id
    /// </summary>
    public XDocument getAssetInfo(long assetId)
    {
        return XDocument.Load($"{_endpointHttps}/rest/asset/{assetId}");
    }

    /// <summary>
    /// For a given asset id, get a list of comments, sender names and comment dates
    /// </summary>
    public XDocument getCommentsForAsset(long assetId, int startIndex, int length)
    {
        return XDocument.Load($"{_endpointHttps}/rest/comments/{assetId}/{startIndex}/{length}");
    }

    /// <summary>
    /// For a given username, get a list of buddy names and ids and total buddy count
    /// </summary>
    public XDocument getBuddiesForUser(string username, int startIndex, int length)
    {
        return XDocument.Load($"{_endpointHttps}/rest/users/buddies/{username}/{startIndex}/{length}");
    }

    /// <summary>
    /// For a given username, get the list of users who have added that username as a buddy.
    /// </summary>
    public XDocument getSubscribersForUser(string username, int startIndex, int length)
    {
        return XDocument.Load($"{_endpointHttps}/rest/users/subscribers/{username}/{startIndex}/{length}");
    }

    /// <summary>
    /// List creations for a given view.
    /// View Types are: TOP_RATED, TOP_RATED_NEW, NEWEST, FEATURED, MAXIS_MADE, RANDOM, CUTE_AND_CREEPY
    /// For each asset you get id, name, author, creation date, rating, type and parent
    /// </summary>
    public XDocument search(string viewType, int startIndex, int length)
    {
        return XDocument.Load($"{_endpointHttps}/rest/assets/search/{viewType}/{startIndex}/{length}");
    }

    /// <summary>
    /// List creations for a given view.
    /// View Types are: TOP_RATED, TOP_RATED_NEW, NEWEST, FEATURED, MAXIS_MADE, RANDOM, CUTE_AND_CREEPY
    /// For each asset you get id, name, author, creation date, rating, type and parent
    /// Optionally, you can specify an asset type
    /// Asset types are: UFO, CREATURE, BUILDING, VEHICLE
    /// </summary>
    public XDocument search(string viewType, int startIndex, int length, string assetType)
    {
        return XDocument.Load($"{_endpointHttps}/rest/assets/search/{viewType}/{startIndex}/{length}/{assetType}");
    }



    /// <summary>
    /// Get XML for an asset id
    /// </summary>
    public XDocument getXmlAsset(long assetId)
    {
        string id = assetId.ToString();
        string subId1 = id.Substring(0, 3);
        string subId2 = id.Substring(3, 3);
        string subId3 = id.Substring(6, 3);

        return XDocument.Load($"{_endpointHttps}/static/model/{subId1}/{subId2}/{subId3}/{id}.xml");
    }

    /// <summary>
    /// Get large PNG for an asset id - for viewing only, not usable in game
    /// </summary>
    public void downloadLargeAssetPng(long assetId, string fileName)
    {
        string id = assetId.ToString();
        string subId1 = id.Substring(0, 3);
        string subId2 = id.Substring(3, 3);
        string subId3 = id.Substring(6, 3);

        var uri = new Uri($"{_endpointHttps}/static/image/{subId1}/{subId2}/{subId3}/{id}_lrg.png");

        using (var client = new WebClient())
        {
            client.DownloadFile(uri, fileName);
        }
    }

    /// <summary>
    /// Get small PNG for an asset id - usable in game
    /// </summary>
    public void downloadAssetPng(long assetId, string fileName)
    {
        string id = assetId.ToString();
        string subId1 = id.Substring(0, 3);
        string subId2 = id.Substring(3, 3);
        string subId3 = id.Substring(6, 3);

        var uri = new Uri($"{_endpointHttps}/static/thumb/{subId1}/{subId2}/{subId3}/{id}.png");

        using (var client = new WebClient())
        {
            client.DownloadFile(uri, fileName);
        }
    }



    /// <summary>
    /// Get a feed of all the assets for user. Asset name, description, type, creation date, PNG and XML links are included
    /// </summary>
    public XDocument getAssetsForUserFeed(string username)
    {
        return XDocument.Load($"{_endpointHttps}/atom/assets/user/{username}");
    }

    /// <summary>
    /// Get a feed of recent events for user
    /// </summary>
    public XDocument getEventsForUserFeed(string username)
    {
        return XDocument.Load($"{_endpointHttps}/atom/events/user/{username}");
    }

    /// <summary>
    /// Get a feed of recent events for given asset id
    /// </summary>
    public XDocument getEventsForAssetFeed(long assetId)
    {
        return XDocument.Load($"{_endpointHttps}/atom/events/asset/{assetId}");
    }

    /// <summary>
    /// Get a feed of all the assets in a sporecast, date they were added and links to PNG and XML
    /// </summary>
    public XDocument getSporecastFeed(long sporecastId)
    {
        return XDocument.Load($"{_endpointHttps}/atom/sporecast/{sporecastId}");
    }

    /// <summary>
    /// List creations for a given view.
    /// View Types are: TOP_RATED, TOP_RATED_NEW, NEWEST, FEATURED, MAXIS_MADE, RANDOM, CUTE_AND_CREEPY
    /// </summary>
    public XDocument searchFeed(string viewType, int startIndex, int length)
    {
        return XDocument.Load($"{_endpointHttps}/atom/assets/view/{viewType}/{startIndex}/{length}");
    }
}