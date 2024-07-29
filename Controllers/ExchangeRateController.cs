using KursyWalut2.Data;
using KursyWalut2.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace KursyWalut2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRateController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly HttpClient _httpClient;
        public ExchangeRateController(DataContext context)
        {
            _httpClient = new HttpClient();
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddData([FromBody] Request request)
        {
            string url = $"https://api.nbp.pl/api/exchangerates/tables/{request.TableType}/{request.StartDate}/{request.EndDate}?format=json";
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseData = await response.Content.ReadAsStringAsync();

                var exchangeRateResponses = JsonSerializer.Deserialize<List<DeserializedRequest>>(responseData);

                if (exchangeRateResponses == null || !exchangeRateResponses.Any())
                {
                    return StatusCode(500, "Deserialization returned null or no data found.");
                }

                foreach (var exchangeRateResponse in exchangeRateResponses)
                {
                    var newExchangeRate = new ExchangeRate
                    {
                        Table = exchangeRateResponse.Table,
                        No = exchangeRateResponse.No,
                        EffectiveDate = exchangeRateResponse.EffectiveDate,
                        Rates = exchangeRateResponse.Rates.Select(rate => new Rate
                        {
                            Currency = rate.Currency,
                            Code = rate.Code,
                            Mid = rate.Mid,
                            Bid = rate.Bid,
                            Ask = rate.Ask
                        }).ToList()
                    };

                    _context.ExchangeRates.Add(newExchangeRate);
                }

                await _context.SaveChangesAsync();

                return Ok(await _context.ExchangeRates.ToListAsync());
            }
            catch (HttpRequestException httpEx)
            {
                return StatusCode(500, $"HTTP error occurred while retrieving data: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                return StatusCode(500, $"JSON error occurred while deserializing data: {jsonEx.Message}");
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Database error occurred while saving data: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing data: {ex.Message}");
            }
        }

        [HttpGet("{startDate},{endDate}")]
        public async Task<ActionResult<List<ExchangeRate>>> GetData(string startDate, string endDate)
        {
            DateOnly startDateParsed = DateOnly.Parse(startDate);
            DateOnly endDateParsed = DateOnly.Parse(endDate);

            // Pobierz ExchangeRates w danym zakresie dat
            var exchangeRates = await _context.ExchangeRates
                .Where(er => er.EffectiveDate >= startDateParsed && er.EffectiveDate <= endDateParsed)
                .Include(er => er.Rates) // Upewnij się, że Rates są również załadowane
                .ToListAsync();

            if (!exchangeRates.Any())
                return NotFound();

            return Ok(exchangeRates);
        }


    }
}
