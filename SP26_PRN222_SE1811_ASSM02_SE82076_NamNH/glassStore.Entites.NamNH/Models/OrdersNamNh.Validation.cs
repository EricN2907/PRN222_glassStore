using System.ComponentModel.DataAnnotations;

namespace glassStore.Entites.NamNH.Models;

[MetadataType(typeof(OrdersNamNhMetadata))]
public partial class OrdersNamNh
{
}

public class OrdersNamNhMetadata
{
    [Required(ErrorMessage = "Customer is required.")]
    public int? UserId { get; set; }

    [Required(ErrorMessage = "Order Type is required.")]
    public string OrderType { get; set; }

    [Required(ErrorMessage = "Order Code is required.")]
    [StringLength(50, ErrorMessage = "Order Code cannot exceed 50 characters.")]
    public string OrderCode { get; set; }

    [Required(ErrorMessage = "Payment Method is required.")]
    public string PaymentMethod { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public string Status { get; set; }

    [Required(ErrorMessage = "Receiver Name is required.")]
    public string ReceiverName { get; set; }

    [Required(ErrorMessage = "Receiver Phone is required.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string ReceiverPhone { get; set; }

    [Required(ErrorMessage = "Receiver Address is required.")]
    public string ReceiverAddress { get; set; }

    [Required(ErrorMessage = "Subtotal is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Subtotal must be greater than 0.")]
    public decimal? Subtotal { get; set; }

    [Required(ErrorMessage = "Grand Total is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Grand Total must be greater than 0.")]
    public decimal? GrandTotal { get; set; }

    [Required(ErrorMessage = "Shipping Fee is required.")]
    [Range(0, double.MaxValue, ErrorMessage = "Shipping Fee cannot be negative.")]
    public decimal? ShippingFee { get; set; }
}
