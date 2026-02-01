using System.Text.Json.Serialization;

namespace BackofficeCore.Infrastructure.Services.Hostaway;

public class HostawayResponse<T>
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("result")]
    public T? Result { get; set; }
}

public class HostawayPagedResponse<T>
{
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("result")]
    public T? Result { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("limit")]
    public int? Limit { get; set; }

    [JsonPropertyName("offset")]
    public int? Offset { get; set; }
}

public class HostawayAuthResponse
{
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
}

public class HostawayListingResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("propertyTypeId")]
    public int PropertyTypeId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("street")]
    public string? Street { get; set; }

    [JsonPropertyName("personCapacity")]
    public int? PersonCapacity { get; set; }

    [JsonPropertyName("bedroomsNumber")]
    public int? BedroomsNumber { get; set; }

    [JsonPropertyName("bedsNumber")]
    public int? BedsNumber { get; set; }

    [JsonPropertyName("bathroomsNumber")]
    public int? BathroomsNumber { get; set; }

    [JsonPropertyName("guestBathroomsNumber")]
    public int? GuestBathroomsNumber { get; set; }

    [JsonPropertyName("averageReviewRating")]
    public decimal? AverageReviewRating { get; set; }

    [JsonPropertyName("listingImages")]
    public List<HostawayListingImageResponse> ListingImages { get; set; } = [];
}

public class HostawayListingImageResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("caption")]
    public string? Caption { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("sortOrder")]
    public int? SortOrder { get; set; }
}

public class HostawayPropertyTypeResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class HostawayAmenityResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class HostawayBedTypeResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

public class HostawayCalendarResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("date")]
    public string? Date { get; set; }

    [JsonPropertyName("isAvailable")]
    public int IsAvailable { get; set; }

    [JsonPropertyName("isProcessed")]
    public int IsProcessed { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("reservations")]
    public List<HostawayCalendarReservationResponse> Reservations { get; set; } = [];
}

public class HostawayCalendarReservationResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("listingMapId")]
    public int ListingMapId { get; set; }

    [JsonPropertyName("guestName")]
    public string? GuestName { get; set; }

    [JsonPropertyName("guestFirstName")]
    public string? GuestFirstName { get; set; }

    [JsonPropertyName("guestLastName")]
    public string? GuestLastName { get; set; }

    [JsonPropertyName("guestEmail")]
    public string? GuestEmail { get; set; }

    [JsonPropertyName("phone")]
    public string? GuestPhone { get; set; }

    [JsonPropertyName("arrivalDate")]
    public string? ArrivalDate { get; set; }

    [JsonPropertyName("departureDate")]
    public string? DepartureDate { get; set; }

    [JsonPropertyName("checkInTime")]
    public int CheckInTime { get; set; }

    [JsonPropertyName("checkOutTime")]
    public int CheckOutTime { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("paymentStatus")]
    public string? PaymentStatus { get; set; }
}