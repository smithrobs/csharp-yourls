using Xunit;

namespace Yourls.Test
{
    public class API
    {
        public readonly string yourls_url = "ph.ly";
        public readonly string yourls_api = "1a40d1e654";
        public readonly YourlsClient client;

        public API()
        {
            client = new YourlsClient(yourls_url, yourls_api);
        }

        [Fact]
        public void should_shorten_url()
        {
            var result = client.Shorten("http://www.amazon.com/Dusk-Subside-Inverloch/dp/B007O4UE42");

            //{"url":{"keyword":"1axuf","url":"http:\/\/www.amazon.com\/Dusk-Subside-Inverloch\/dp\/B007O4UE42","title":"http:\/\/www.amazon.com\/Dusk-Subside-Inverloch\/dp\/B007O4UE42","date":"2012-04-21 08:03:30","ip":"96.245.169.72"},"status":"success","message":"http:\/\/www.amazon.com\/Dusk-Subside-Inverloch\/dp\/B007O4UE42 added to database","title":"http:\/\/www.amazon.com\/Dusk-Subside-Inverloch\/dp\/B007O4UE42","shorturl":"http:\/\/ph.ly\/1axuf","statusCode":200}

            Assert.Equal(200, (int)result.statusCode);
        }

        [Fact]
        public void should_not_shorten_existing_url()
        {
            var result = client.Shorten("https://github.com/gabrielpreston/node-yourls");

            Assert.Equal("fail", (string)result.status);
            Assert.Equal(@"error:url", (string)result.code);
            Assert.Equal(200, (int)result.statusCode);
        }

        [Fact]
        public void should_expand_valid_full_url()
        {
            var result = client.Expand("http://ph.ly/dzg-v");

            Assert.Equal(200, (int)result.statusCode);
            Assert.Equal("dzg-v", (string)result.keyword);
            Assert.Equal(@"http://ph.ly/dzg-v", (string)result.shorturl);
            Assert.Equal(@"https://github.com/gabrielpreston/node-yourls", (string)result.longurl);
        }

        [Fact]
        public void should_expand_valid_hash()
        {
            var result = client.Expand("dzg-v");

            Assert.Equal(200, (int)result.statusCode);
            Assert.Equal("dzg-v", (string)result.keyword);
            Assert.Equal(@"http://ph.ly/dzg-v", (string)result.shorturl);
            Assert.Equal(@"https://github.com/gabrielpreston/node-yourls", (string)result.longurl);
        }

        [Fact]
        public void should_get_urlstats_valid_url()
        {
            var result = client.UrlStats("http://ph.ly/dzg-v");

            Assert.Equal(200, (int)result.statusCode);
            Assert.Equal(@"http://ph.ly/dzg-v", (string)result.link.shorturl);
            Assert.Equal(@"https://github.com/gabrielpreston/node-yourls", (string)result.link.url);
            Assert.Equal(@"gabrielpreston/node-yourls · GitHub", (string)result.link.title);
            Assert.Equal("2012-04-18 15:00:56", (string)result.link.timestamp);
            Assert.Equal("204.236.242.146", (string)result.link.ip);
            Assert.True((int)result.link.clicks > 30);
        }

        [Fact]
        public void should_get_urlstats_valid_hash()
        {
            var result = client.UrlStats("dzg-v");

            Assert.Equal(200, (int)result.statusCode);
            Assert.Equal(@"http://ph.ly/dzg-v", (string)result.link.shorturl);
            Assert.Equal(@"https://github.com/gabrielpreston/node-yourls", (string)result.link.url);
            Assert.Equal(@"gabrielpreston/node-yourls · GitHub", (string)result.link.title);
            Assert.Equal("2012-04-18 15:00:56", (string)result.link.timestamp);
            Assert.Equal("204.236.242.146", (string)result.link.ip);
            Assert.True((int)result.link.clicks > 30);
        }

        // do tests on invalid full url/hash?
    }
}
