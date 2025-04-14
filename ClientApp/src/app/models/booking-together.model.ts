import { BookingRequest } from './booking-request.model';
import { LocalPrice } from './local-price.model';

export interface BookingTogether {
  book_with: string | null;
  airline_logos: string[] | null; // URLs
  marketed_as: string[] | null; // Flight numbers
  price: number;
  option_title: string | null;
  extensions: string[] | null;
  local_prices: LocalPrice[] | null;
  baggage_prices: string[] | null; // Note: Duplicates BaggagePrices structure slightly
  booking_request: BookingRequest | null;
} 