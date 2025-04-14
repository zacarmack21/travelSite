import { FlightOption } from './flight-option.model';
import { PriceInsights } from './price-insights.model';
import { SelectedFlight } from './selected-flight.model';
import { BookingOption } from './booking-option.model';
import { BaggagePrices } from './baggage-prices.model';
import { SearchMetadata } from './search-metadata.model';
import { SearchParameters } from './search-parameters.model';

export interface FlightSearchResponse {
    best_flights?: FlightOption[];
    other_flights?: FlightOption[];
    price_insights?: PriceInsights;
    selected_flights?: SelectedFlight[];
    booking_options?: BookingOption[];
    baggage_prices?: BaggagePrices;
    search_metadata?: SearchMetadata;
    search_parameters?: SearchParameters;
    error?: string;
    error_message?: string;
} 