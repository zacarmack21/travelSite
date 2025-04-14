import { BaggagePrices } from './baggage-prices.model';
import { BookingOptionDetail } from './booking-option-detail.model';
import { FlightOption } from './flight-option.model';
import { PriceInsights } from './price-insights.model';

export interface BookingApiResponse {
  selected_flights: FlightOption[] | null;
  baggage_prices: BaggagePrices | null;
  booking_options: BookingOptionDetail[] | null;
  price_insights: PriceInsights | null;
  error_message?: string | null; // For potential errors from the service
} 