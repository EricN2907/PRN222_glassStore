using System.ComponentModel.DataAnnotations;

namespace glassStore.Entites.NamNH.Models;

[MetadataType(typeof(OrdersNamNhMetadata))]
public partial class OrdersNamNh
{
}

public class OrdersNamNhMetadata
{
    [Required(ErrorMessage = "Vui lòng chọn khách hàng (Customer).")]
    public int? UserId { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn loại đơn hàng (Order Type).")]
    public string OrderType { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mã đơn hàng (Order Code).")]
    [StringLength(50, ErrorMessage = "Mã đơn hàng không được quá 50 ký tự.")]
    public string OrderCode { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán.")]
    public string PaymentMethod { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn trạng thái đơn hàng.")]
    public string Status { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên người nhận.")]
    public string ReceiverName { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại người nhận.")]
    [Phone(ErrorMessage = "Số điện thoại không đúng định dạng.")]
    public string ReceiverPhone { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ nhận hàng.")]
    public string ReceiverAddress { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập Subtotal.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn 0.")]
    public decimal? Subtotal { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập Grand Total.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn 0.")]
    public decimal? GrandTotal { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập Shipping Fee.")]
    [Range(0, double.MaxValue, ErrorMessage = "Phí giao hàng không được âm.")]
    public decimal? ShippingFee { get; set; }
}
