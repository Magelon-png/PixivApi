namespace Scighost.PixivApi.V2.Illust;

public record IllustSearchResponse(
    [property: JsonPropertyName("illusts")]
    List<IllustInfo> Illusts,
    [property: JsonPropertyName("next_url")]
    string NextUrl,
    [property: JsonPropertyName("search_span_limit")]
    uint SearchSpanLimit,
    [property: JsonPropertyName("show_ai")]
    bool ShowAi
    );


//Full response
// public record RootObject(
//     Illusts[] illusts,
//     string next_url,
//     int search_span_limit,
//     bool show_ai
// );
//
// public record Illusts(
//     int id,
//     string title,
//     string type,
//     Image_urls image_urls,
//     string caption,
//     int restrict,
//     User user,
//     Tags[] tags,
//     string[] tools,
//     string create_date,
//     int page_count,
//     int width,
//     int height,
//     int sanity_level,
//     int x_restrict,
//     Series series,
//     Meta_single_page meta_single_page,
//     Meta_pages[] meta_pages,
//     int total_view,
//     int total_bookmarks,
//     bool is_bookmarked,
//     bool visible,
//     bool is_muted,
//     int illust_ai_type,
//     int illust_book_style,
//     Request request,
//     string[] restriction_attributes
// );
//
// public record Image_urls(
//     string square_medium,
//     string medium,
//     string large
// );
//
// public record User(
//     int id,
//     string name,
//     string account,
//     Profile_image_urls profile_image_urls,
//     bool is_followed,
//     bool is_accept_request
// );
//
// public record Profile_image_urls(
//     string medium
// );
//
// public record Tags(
//     string name,
//     object translated_name
// );
//
// public record Series(
//     int id,
//     string title
// );
//
// public record Meta_single_page(
//     string original_image_url
// );
//
// public record Meta_pages(
//     Image_urls1 image_urls
// );
//
// public record Image_urls1(
//     string square_medium,
//     string medium,
//     string large,
//     string original
// );
//
// public record Request(
//     Request_info request_info,
//     object[] request_users
// );
//
// public record Request_info(
//     object fan_user_id,
//     Collaborate_status collaborate_status,
//     string role
// );
//
// public record Collaborate_status(
//     bool collaborating,
//     bool collaborate_anonymous_flag,
//     object[] collaborate_user_samples
// );

