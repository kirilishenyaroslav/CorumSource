
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Corum.DAL.Entity
{

using System;
    
public partial class GetOrdersPipelineV3_Result
{

    public long Id { get; set; }

    public System.DateTime OrderDate { get; set; }

    public string CreatedByUser { get; set; }

    public System.DateTime CreateDatetime { get; set; }

    public int OrderType { get; set; }

    public int CurrentOrderStatus { get; set; }

    public string OrderDescription { get; set; }

    public long ClientId { get; set; }

    public Nullable<long> ClientDogId { get; set; }

    public Nullable<decimal> Summ { get; set; }

    public Nullable<bool> UseNotifications { get; set; }

    public string CreatorPosition { get; set; }

    public string CreatorContact { get; set; }

    public int PriotityType { get; set; }

    public Nullable<System.DateTime> OrderServiceDateTime { get; set; }

    public string OrderExecuter { get; set; }

    public Nullable<int> PayerId { get; set; }

    public string ProjectNum { get; set; }

    public Nullable<int> CarNumber { get; set; }

    public string DistanceDescription { get; set; }

    public Nullable<decimal> TotalPrice { get; set; }

    public Nullable<decimal> TotalDistanceLength { get; set; }

    public Nullable<bool> IsPrivateOrder { get; set; }

    public string Color { get; set; }

    public string OrderStatusName { get; set; }

    public string OrderStatusShortName { get; set; }

    public string FontColor { get; set; }

    public string BackgroundColor { get; set; }

    public string IconFile { get; set; }

    public string IconDescription { get; set; }

    public string ExecuterNotes { get; set; }

    public string ClientName { get; set; }

    public string CenterName { get; set; }

    public string CreatorDispalyName { get; set; }

    public string ExecutorDisplayName { get; set; }

    public string TypeName { get; set; }

    public string TypeShortName { get; set; }

    public Nullable<bool> IsTransportType { get; set; }

    public string PayerName { get; set; }

    public Nullable<int> ProjectId { get; set; }

    public string ProjectCode { get; set; }

    public string ProjectDescription { get; set; }

    public Nullable<bool> isFinishOfTheProcess { get; set; }

}

}
