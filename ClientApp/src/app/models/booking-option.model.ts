export interface BookingOption {
    together?: BookingProviderDetails;
    // Add other potential structures like 'split' if needed
}

export interface BookingProviderDetails {
    book_with?: string;
    airline_logos?: string[];
    marketed_as?: string[];
    price?: number;
    local_prices?: LocalPrice[];
    option_title?: string;
    extensions?: string[];
    baggage_prices?: string[];
    booking_request?: BookingRequest;
}

export interface LocalPrice {
    currency?: string;
    price?: number;
}

export interface BookingRequest {
    url?: string;
    post_data?: string; // This is the important data for the booking step
} 