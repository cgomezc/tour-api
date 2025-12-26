
namespace CJTour.Domain.Entities;

public partial class AdditionalInfo
{
    public int AdditionalInfoId { get; set; }

    public string InfoType { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AdditionalInfoTranslation> AdditionalInfoTranslations { get; set; } = new List<AdditionalInfoTranslation>();

    public virtual ICollection<TourAdditionalInfo> TourAdditionalInfos { get; set; } = new List<TourAdditionalInfo>();
}
