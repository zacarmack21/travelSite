export interface BaggagePrices {
  together: string[] | null; // List of strings like "1 free carry-on"
  first_bag_price?: number | null;
  second_bag_price?: number | null;
  carry_on_price?: number | null;
} 