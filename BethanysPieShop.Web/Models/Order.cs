using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BethanysPieShop.Web.Models;

public class Order
{
    [BindNever]
    public int OrderId { get; set; }
    
    public List<OrderDetail>? OrderDetails { get; set; }
    
    [Required(ErrorMessage = "Please enter your first name")]
    [Display(Name = "First Name")]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Please enter your last name")]
    [Display(Name = "Last Name")]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Please enter your address")]
    [Display(Name = "Address Line 1")]
    [StringLength(100)]
    public string AddressLine1 { get; set; } = string.Empty;
    
    [Display(Name = "Address Line 2")]
    public string? AddressLine2 { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Please enter your zip code")]
    [Display(Name = "Zip Code")]
    [StringLength(10, MinimumLength = 4)]
    public string ZipCode { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Please enter your city")]
    [StringLength(50)]
    public string City { get; set; } = string.Empty;
    
    [StringLength(10)]
    public string? State { get; set; }
    
    [Required(ErrorMessage = "Please enter your country")]
    [StringLength(50)]
    public string Country { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Please enter your phone number")]
    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;
    
    [BindNever]
    public decimal OrderTotal { get; set; }
    
    [BindNever]
    public DateTime OrderPlaced { get; set; }
}