// Define properties based on the bookingResponse.txt sample
export interface SearchParameters {
    engine?: string;
    hl?: string;
    gl?: string;
    departure_id?: string;
    arrival_id?: string;
    outbound_date?: string;
    return_date?: string;
    // Note: booking_token was in the sample, reflects the token used for *this* request
    booking_token?: string;
    currency?: string;
    // Add other potential parameters if applicable
} 