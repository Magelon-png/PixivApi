![Nuget](https://img.shields.io/nuget/v/Magelon-png.PixivApi)
![Nuget](https://img.shields.io/nuget/dt/Magelon-png.PixivApi)

# Pixiv Api

Expected breaking changes for V2:

- Changing multiple classes to use `uint` instead of `int` for properties that do not go into the negative and are at risk of overflowing (ie: illustration Ids)
- Various changes in namespaces

During the 0.x version period, the availability and stability of the API are not guaranteed, and method signatures may change at any time.

## Existing Features

- Illustration, Manga, Novel
  - Detailed information
  - Like and bookmark
  - Add follow notifications
  - Modify tags
  - Related recommendations
- User
  - Follow and unfollow
  - All works
  - Latest works
  - Bookmarked works
  - Related recommendations
- ...

## Getting Started

Pixiv's login process uses Cloudflare protection, which is basically impossible to bypass. For features that require an account, please log in through a browser and use the constructor containing cookie and user agent.

Pixiv's images use hotlink protection. When downloading images, you need to add `Referer`: `https://www.pixiv.net/`

> PixivClient is the request class for all APIs. Some APIs are listed below. Explore more during use or check the source code.

### Constructing Client

``` cs
using Scighost.PixivApi;

// Using account, can be combined with direct connection or proxy
PixivClient client = new PixivClient(cookie: "your cookie", userAgent: "your ua");

// Before performing non-GET operations such as following or bookmarking, you need to call this method to get a token. 
// A return value of true indicates successful acquisition. It is recommended to call it immediately after construction.
Debug.Assert(await client.GetTokenAsync());
```

### Illustration & Manga

``` CSharp
// Illustration and manga detailed information
IllustInfo _ = await client.GetIllustInfoAsync(illustId: 12345678);

// Illustration images
List<IllustImage> _ = await client.GetIllustPagesAsync(illustId: 12345678);

// Animation metadata
AnimateIllustMeta _ = await client.GetAnimateIllustMetaAsync(illustId: 12345678);

// Follow manga series
await client.WatchMangaSeriesAsync(mangaSeriesId: 123456, unWatch: false);

// Change follow notification status
await client.ChangeMangaSeriesWatchListNotification(mangaSeriesId: 123456, enable: true);

// Related recommendations. There are many recommendations but it is impossible to get them all at once, so asynchronous streams are used.
await foreach (IEnumerable<IllustProfile> illusts in client.GetRecommendIllustsAsync(illustId: 12345678, batchSize: 20))
{
    Debug.Assert(illusts.Count() == 20);
}
```

### Novel

``` CSharp
// Novel series & series chapters
NovelSeries _ = await client.GetNovelSeriesAsync(novelSeriesId: 123456);
List<NovelSeriesChapter> _ = await client.GetNovelSeriesChaptersAsync(novelSeriesId: 123456, offset: 0, limit: 10);

// Bookmark a page
await client.MarkerNovelPageAsync(myUserId: 1234567, novelId: 12345678, page: 1);

// Bookmark a novel
long bookmarkId = await client.AddBookmarkNovelAsync(novelId: 12345678, isPrivate: false, comment: "Comment", tags: "Custom tag");

// Batch change bookmark visibility
await client.ChangeBookmarkNovelVisibilityAsync(isPrivate: true, bookmarkIds: new long[] { 1, 2 });

// Batch add custom tags
await client.AddBookmarkNovelTagsAsync(bookmarkIds: new long[] { 1, 2, 3 }, tags: new string[] { "Tag 1", "Tag 2" });
```

### User & Bookmarks

``` CSharp
// My Uid
int myUid = await client.GetMyUserIdAsync();

// Followed users
List<FollowingUser> _ = await client.GetFollowingUsersAsync(userId: 123456, offset: 0, limit: 20, isPrivate: false);

// Latest illustration/manga works from followed users
List<IllustProfile> _ = await client.GetFollowingUserLatestIllustsAsync(page: 2, onlyR18: false);

// Related recommendations after following a new user
List<RecommendUser> _ = await client.GetRecommendAfterFollowingUserAsync(userId: 123456, userNumber: 20, workNumber: 3, allowR18: true);

// Number of bookmarked illustrations
int count = await client.GetUserBookmarkIllustCountAsync(userId: 123456, isPrivate: false);

// All custom tags of bookmarked illustrations
UserBookmarkTag _ = await client.GetUserBookmarkIllustTagsAsync(userId: 123456);
```

## PR Guidelines

- Submit code on a new branch
- Focus on one feature or fix
- Edits allowed

