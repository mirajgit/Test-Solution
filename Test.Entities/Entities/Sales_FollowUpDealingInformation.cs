using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class Sales_FollowUpDealingInformation
{
    public long FollowUpDealingId { get; set; }

    public long FollowUpId { get; set; }

    public string DealingOfficer { get; set; } = null!;

    public DateOnly DealingDate { get; set; }

    public int ProjectId { get; set; }

    public string? Sector { get; set; }

    public string? Block { get; set; }

    public string? Road { get; set; }

    public string? Facing { get; set; }

    public int? AppType { get; set; }

    public string? Floor { get; set; }

    public int? AppNo { get; set; }

    public decimal? AppSize { get; set; }

    public string? Plot { get; set; }

    public string? PaymentType { get; set; }

    public string? RateType { get; set; }

    public decimal? RateAmount { get; set; }

    public decimal TotalArea { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal BookingMoney { get; set; }

    public decimal BookingDate { get; set; }

    public string? DownPayment { get; set; }

    public DateOnly? DownPaymentDate { get; set; }

    public decimal? CarParking { get; set; }

    public decimal? Utility { get; set; }

    public decimal? SpecialDiscount { get; set; }

    public decimal? ActualPrice { get; set; }

    public string? ClientProposal { get; set; }

    public string? ManagementOrder { get; set; }

    public string? ApprovedBy { get; set; }

    public DateOnly? ApprovedDate { get; set; }

    public string? DealingDescription { get; set; }

    public string? Remarks { get; set; }

    public bool Status { get; set; }

    public Guid CreateUser { get; set; }

    public DateTime CreateDate { get; set; }

    public Guid? EditUser { get; set; }

    public DateTime EditDate { get; set; }
}
