import { BaggagePrices } from './baggage-prices.model';
import { BookingOptionDetail } from './booking-option-detail.model';
import { FlightOption } from './flight-option.model';
import { PriceInsights } from './price-insights.model';

export interface BookingApiResponse {
  selectedFlights: FlightOption[] | null;
  baggagePrices: BaggagePrices | null;
  bookingOptions: BookingOptionDetail[] | null;
  priceInsights: PriceInsights | null;
  errorMessage?: string | null; // For potential errors from the service
} 