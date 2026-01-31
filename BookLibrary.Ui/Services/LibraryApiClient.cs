using System.Net.Http.Json;
using BookLibrary.Ui.Models;

namespace BookLibrary.Ui.Services;

public class LibraryApiClient
{
    private readonly HttpClient _http;

    public LibraryApiClient(HttpClient http)
    {
        _http = http;
    }

    public Task<ApiResult<List<Book>>> GetBooksAsync() => GetAsync<List<Book>>("api/books");

    public Task<ApiResult<Book>> AddBookAsync(Book book) => PostAsync<Book>("api/books", book);

    public Task<ApiResult<List<Borrower>>> GetBorrowersAsync() => GetAsync<List<Borrower>>("api/borrowers");

    public Task<ApiResult<Borrower>> AddBorrowerAsync(Borrower borrower) => PostAsync<Borrower>("api/borrowers", borrower);

    public Task<ApiResult<List<Rental>>> GetRentalsAsync(bool activeOnly)
        => GetAsync<List<Rental>>($"api/rentals?activeOnly={activeOnly.ToString().ToLowerInvariant()}");

    public Task<ApiResult<Rental>> AddRentalAsync(CreateRentalRequest request)
        => PostAsync<Rental>("api/rentals", request);

    public Task<ApiResult<bool>> ReturnRentalAsync(int id)
        => PostAsync<bool>($"api/rentals/{id}/return", null);

    private async Task<ApiResult<T>> GetAsync<T>(string path)
    {
        try
        {
            var data = await _http.GetFromJsonAsync<T>(path);
            return data is null
                ? ApiResult<T>.Failure("Empty response.")
                : ApiResult<T>.Success(data);
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Failure(ex.Message);
        }
    }

    private async Task<ApiResult<T>> PostAsync<T>(string path, object? body)
    {
        try
        {
            HttpResponseMessage response = body is null
                ? await _http.PostAsync(path, null)
                : await _http.PostAsJsonAsync(path, body);

            if (response.IsSuccessStatusCode)
            {
                if (typeof(T) == typeof(bool))
                {
                    return ApiResult<T>.Success((T)(object)true);
                }

                if (response.Content.Headers.ContentLength == 0)
                {
                    return ApiResult<T>.Success(default);
                }

                var data = await response.Content.ReadFromJsonAsync<T>();
                return data is null
                    ? ApiResult<T>.Failure("Empty response.")
                    : ApiResult<T>.Success(data);
            }

            var errorText = await response.Content.ReadAsStringAsync();
            return ApiResult<T>.Failure(string.IsNullOrWhiteSpace(errorText)
                ? response.ReasonPhrase ?? "Request failed."
                : errorText);
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Failure(ex.Message);
        }
    }
}
