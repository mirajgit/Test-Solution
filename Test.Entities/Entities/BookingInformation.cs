using System;
using System.Collections.Generic;

namespace Test.Entities;

public partial class BookingInformation
{
    public int ParcelId { get; set; }

    public string ParcelType { get; set; } = null!;

    public string BookingType { get; set; } = null!;

    public string CNNO { get; set; } = null!;

    public DateOnly BookingDate { get; set; }

    public string BookingBranchEng { get; set; } = null!;

    public string BookingBranchBng { get; set; } = null!;

    public string ReceiveBranchEng { get; set; } = null!;

    public string ReceiveBranchBng { get; set; } = null!;

    public int Quantiry { get; set; }

    public string? Referance { get; set; }

    public string ItemDetails { get; set; } = null!;

    public string ReceiverName { get; set; } = null!;

    public string ReceiverContact { get; set; } = null!;

    public string ReceiverAddress { get; set; } = null!;

    public string? HV { get; set; }
}
