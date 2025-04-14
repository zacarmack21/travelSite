import { FlightOption } from './flight-option.model';
import { PriceInsights } from './price-insights.model';

export interface FlightSearchResponse {
    bestFlights?: FlightOption[];
    otherFlights?: FlightOption[];
    priceInsights?: PriceInsights;
    errorMessage?: string;
} 