using System.Diagnostics.CodeAnalysis;

namespace RestaurantAggregator.APIAuth.Models;

public class Response
{
    [MaybeNull]
    public string Status { get; set; }
    [MaybeNull]
    public string Message { get; set; }
}