using CJTour.Contracts.Common;
using CJTour.Contracts.DTOs;

namespace CJTour.Application.Interfaces;

public interface IDestinationService
{
    Task<IEnumerable<DestinationDto>> GetAllDestinationsAsync(string languageCode = AppConstants.Culture.DefaultLanguage);
    Task<DestinationDto?> GetDestinationByIdAsync(int destinationId, string languageCode = AppConstants.Culture.DefaultLanguage);
    Task<DestinationDto?> GetDestinationBySlugAsync(string slug, string languageCode = AppConstants.Culture.DefaultLanguage);
}
