using System.ComponentModel.DataAnnotations;

namespace ASE.EnterpriseApi.Options;

public class SearchOptions
{
    public const string SectionName = "Search";

    [Required]
    public string Environment { get; set; } = string.Empty;
}
