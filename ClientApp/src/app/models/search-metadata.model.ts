// Define properties based on the bookingResponse.txt sample
// Add more specific types if known (e.g., Date for created_at)
export interface SearchMetadata {
    id?: string;
    status?: string;
    json_endpoint?: string;
    created_at?: string;
    processed_at?: string;
    google_flights_url?: string;
    raw_html_file?: string;
    prettify_html_file?: string;
    total_time_taken?: number;
} 