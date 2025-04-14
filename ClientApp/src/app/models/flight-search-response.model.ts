import { FlightOption } from './flight-option.model';
import { PriceInsights } from './price-insights.model';

export interface FlightSearchResponse {
    best_flights?: FlightOption[];
    other_flights?: FlightOption[];
    price_insights?: PriceInsights;
    error_message?: string;
} 