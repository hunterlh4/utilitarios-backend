using Refit;

namespace UtilitariosCore.Infrastructure.Services.Hostaway;

public interface IHostawayAuthApi
{
    [Post("/v1/accessTokens")]
    [Headers("Cache-Control: no-cache", "Content-Type: application/x-www-form-urlencoded")]
    Task<HostawayAuthResponse> AccessTokens([Body(BodySerializationMethod.UrlEncoded)] HostawayAuthRequest request);
}

public interface IHostawayApi
{
    [Get("/v1/propertyTypes")]
    Task<HostawayResponse<List<HostawayPropertyTypeResponse>>> PropertyTypes();

    [Get("/v1/amenities")]
    Task<HostawayResponse<List<HostawayAmenityResponse>>> Amenities();

    [Get("/v1/bedTypes")]
    Task<HostawayResponse<List<HostawayBedTypeResponse>>> BedTypes();

    [Get("/v1/listings")]
    Task<HostawayPagedResponse<List<HostawayListingResponse>>> Listings();

    [Get("/v1/listings/{listingId}")]
    Task<HostawayResponse<HostawayListingResponse>> ListingById(int listingId);

    [Get("/v1/listings/{listingId}/calendar")]
    Task<HostawayResponse<List<HostawayCalendarResponse>>> CalendarByListingId(int listingId, [Query] HostawayCalendarQuery? query = null);
}