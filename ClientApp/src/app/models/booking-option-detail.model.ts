import { BookingTogether } from './booking-together.model';

export interface BookingOptionDetail {
  together: BookingTogether | null;
  provider_logo?: string | null;
  provider_name?: string | null;
  price?: number | null;
  rating?: number | null;
  rating_level?: string | null;
  description?: string | null;
  booking_link?: string | null;
} 